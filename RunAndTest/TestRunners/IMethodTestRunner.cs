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
        IDictionary<IMethodTestInfo, IMethodTestRunResult> Run(string sourceCode, IEnumerable<IMethodTestInfo> tests);
        Task<IDictionary<IMethodTestInfo, IMethodTestRunResult>> RunAsync(string sourceCode, IEnumerable<IMethodTestInfo> tests, CancellationToken cancellationToken);
    }
}