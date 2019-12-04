using System;
using TestRunner.CommonTypes.Interfaces;

namespace TestRunner.CommonTypes.Implementations
{
    public class MethodTestRunResult : IMethodTestRunResult
    {
        public TestRunStatus TestRunStatus { get; set; }
        public object ActualResult { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}
