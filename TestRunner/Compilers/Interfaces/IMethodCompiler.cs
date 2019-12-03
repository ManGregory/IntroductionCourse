using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace TestRunner.Compilers.Interfaces
{
    public interface IMethodCompiler
    {
        string EntryType { set; }
        string EntryMethod { set; }
        IEnumerable<string> CompilationErrors { get; set; }
        MethodInfo Compile(string sourceCode);
        Task<MethodInfo> CompileAsync(string sourceCode, CancellationToken cancellationToken);
    }
}
