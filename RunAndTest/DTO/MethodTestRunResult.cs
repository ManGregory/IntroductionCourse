using System;
using System.Collections.Generic;
using System.Text;

namespace RunAndTest.DTO
{
    public class MethodTestRunResult : IMethodTestRunResult
    {
        public TestRunStatus TestRunStatus { get; set; }
        public object ActualResult { get; set; }
        public string Message { get; set; }
    }
}
