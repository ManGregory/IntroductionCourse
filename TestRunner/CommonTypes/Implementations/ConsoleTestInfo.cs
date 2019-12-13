using System.Collections.Generic;
using TestRunner.CommonTypes.Interfaces;

namespace TestRunner.CommonTypes.Implementations
{
    public class ConsoleTestInfo : IConsoleTestInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ConsoleTest ConsoleTest { get; set; }
        public object ExpectedResult { get; set; }
        public bool IsCompilation { get; set; }
    }
}
