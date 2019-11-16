using RunAndTest.Compilers;
using RunAndTest.Providers;
using System.Collections.Generic;

namespace RunAndTest.TestRunners
{
    public interface IMethodTestRunner
    {
        IMethodCompiler MethodCompiler { set; }
        IMethodTestProvider MethodTestProvider { set; }
        IEnumerable<string> Run();
    }
}