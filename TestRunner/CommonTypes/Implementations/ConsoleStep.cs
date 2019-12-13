using System;
using System.Collections.Generic;
using System.Text;

namespace TestRunner.CommonTypes.Implementations
{
    public class ConsoleStep
    {
        public string Name { get; set; }        
        public IEnumerable<string> Input { get; set; }
        public IEnumerable<string> Output { get; set; }
    }

    public class ConsoleTest
    {
        public object[] MethodInputParameteres { get; set; }
        public IEnumerable<ConsoleStep> ConsoleSteps { get; set; }
    }
}
