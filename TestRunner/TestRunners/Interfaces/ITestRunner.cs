using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestRunner.CommonTypes.Interfaces;
using TestRunner.Compilers.Interfaces;

namespace TestRunner.TestRunners.Interfaces
{
    public interface ITestRunner
    {
        IMethodCompiler MethodCompiler { set; }
        Task<IDictionary<ITestInfo, ITestRunResult>> RunAsync(string sourceCode, IEnumerable<ITestInfo> tests, CancellationToken cancellationToken);
    }
}