using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestRunner.CommonTypes.Interfaces;
using TestRunner.Compilers.Interfaces;

namespace TestRunner.TestRunners.Interfaces
{
    public interface IMethodTestRunner
    {
        IMethodCompiler MethodCompiler { set; }
        IDictionary<IMethodTestInfo, IMethodTestRunResult> Run(string sourceCode, IEnumerable<IMethodTestInfo> tests);
        Task<IDictionary<IMethodTestInfo, IMethodTestRunResult>> RunAsync(string sourceCode, IEnumerable<IMethodTestInfo> tests, CancellationToken cancellationToken);
    }
}
