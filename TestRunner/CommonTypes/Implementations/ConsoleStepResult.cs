using System.Collections.Generic;

namespace TestRunner.CommonTypes.Implementations
{
    public class ConsoleStepResult
    {
        public string Name { get; set; }
        public IEnumerable<string> Expected { get; set; }
        public IEnumerable<string> Actual { get; set; }
        public bool IsPassed { get; set; }
    }
}
