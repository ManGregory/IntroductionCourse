﻿using TestRunner.CommonTypes.Interfaces;

namespace TestRunner.CommonTypes.Implementations
{
    public class MethodTestInfo : IMethodTestInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public object[] InputParameters { get; set; }
        public object ExpectedResult { get; set; }        
        public bool IsCompilation { get; set; }
    }
}
