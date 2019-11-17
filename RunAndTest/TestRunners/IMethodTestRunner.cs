using RunAndTest.Compilers;
using RunAndTest.Providers;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RunAndTest.TestRunners
{
    public interface IMethodTestRunner
    {
        IMethodCompiler MethodCompiler { set; }
        IMethodTestProvider MethodTestProvider { set; }
        IEnumerable<string> Run();
        Task<IEnumerable<string>> RunAsync(CancellationToken cancellationToken);
    }
}