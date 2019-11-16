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
            MethodInfo methodInfo = null;

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(SourceCodeProvider.SourceCode);

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
                EmitResult result = compilation.Emit(ms);

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
                    var type = assembly.GetType("Lecture1.Program");
                    //var instance = assembly.CreateInstance("RoslynCompileSample.Writer");
                    methodInfo = type.GetMember("IsTicketHappy").First() as MethodInfo;
                }
            }
            return methodInfo;
        }
    }
}