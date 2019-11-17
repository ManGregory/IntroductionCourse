using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using RunAndTest.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace RunAndTest.Compilers
{
    public class RoslynMethodCompiler : IMethodCompiler
    {
        public string EntryType { get; set; }
        public string EntryMethod { get; set; }
        public ISourceCodeProvider SourceCodeProvider { get; set; }
        public IEnumerable<string> CompilationErrors { get; set; } = new List<string>();

        public MethodInfo Compile()
        {
            return Compile(default);
        }

        public MethodInfo Compile(CancellationToken cancellationToken)
        {
            MethodInfo methodInfo = null;

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(text: SourceCodeProvider.SourceCode, cancellationToken: cancellationToken);

            string assemblyName = Path.GetRandomFileName();
            var refPaths = new[] {
                typeof(System.Object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll")
            };
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(peStream: ms, cancellationToken: cancellationToken);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    CompilationErrors = new List<string>(failures.Select(diagnostic => $"\t{diagnostic.Id}: {diagnostic.GetMessage()}"));
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
                    var type = assembly.GetType(EntryType);
                    methodInfo = type.GetMember(EntryMethod).First() as MethodInfo;
                }
            }
            return methodInfo;
        }

        public async Task<MethodInfo> CompileAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(() => Compile(cancellationToken), cancellationToken);
        }
    }
}