using System;
using System.Collections.Generic;
using System.Text;
using TestRunner.CommonTypes.Interfaces;

namespace TestRunner.CommonTypes.Implementations
{
    public class MethodTestInfo : IMethodTestInfo
    {
        public string Name { get; set; }
        public object[] InputParameters { get; set; }
        public object ExpectedResult { get; set; }        
        public bool IsCompilation { get; set; }
    }
}
