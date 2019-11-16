using RunAndTest.Providers;
using System.Collections.Generic;
using System.Reflection;

namespace RunAndTest.Compilers
{
    public interface IMethodCompiler
    {
        string EntryType { set; }
        string EntryMethod { set; }
        ISourceCodeProvider SourceCodeProvider { get; set; }
        IEnumerable<string> CompilationErrors { get; set; }
        MethodInfo Compile();
    }
}