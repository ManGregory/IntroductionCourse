using RunAndTest.Compilers;
using RunAndTest.DTO;
using RunAndTest.Providers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RunAndTest.TestRunners
{
    public interface IMethodTestRunner
    {
        IMethodCompiler MethodCompiler { set; }
        IEnumerable<string> Run(string sourceCode, IEnumerable<IMethodTestInfo> tests);
        Task<IEnumerable<string>> RunAsync(string sourceCode, IEnumerable<IMethodTestInfo> tests, CancellationToken cancellationToken);
    }
}