using System.Collections.Generic;
using TestRunner.CommonTypes.Implementations;

namespace TestRunner.CommonTypes.Interfaces
{
    public interface IConsoleTestInfo : ITestInfo
    {
        ConsoleTest ConsoleTest { get; set; }
    }
}
