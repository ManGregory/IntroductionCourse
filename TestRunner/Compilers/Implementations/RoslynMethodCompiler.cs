using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestRunner.Compilers.Interfaces;

namespace TestRunner.Compilers.Implementations
{
    public class RoslynMethodCompiler : IMethodCompiler
    {
        public string EntryType { get; set; }
        public string EntryMethod { get; set; }
        public IEnumerable<string> CompilationErrors { get; set; } = new List<string>();

        public MethodInfo Compile(string sourceCode)
        {
            return Compile(sourceCode, default);
        }

        public MethodInfo Compile(string sourceCode, CancellationToken cancellationToken)
        {
            MethodInfo methodInfo = null;

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(text: sourceCode, cancellationToken: cancellationToken);

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

                    Func<Location, string> errorPosFunc = (Location location) => $"(строка: {location.GetMappedLineSpan().EndLinePosition.Line + 1}, столбец: {location.GetMappedLineSpan().EndLinePosition.Character})";

                    CompilationErrors = new List<string>(failures.Select(diagnostic => $"\t{diagnostic.Id} {errorPosFunc(diagnostic.Location)} : {diagnostic.GetMessage()}"));
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

        public async Task<MethodInfo> CompileAsync(string sourceCode, CancellationToken cancellationToken)
        {
            return await Task.Run(() => Compile(sourceCode, cancellationToken));
        }
    }
}
