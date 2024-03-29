﻿using System;
using TestRunner.CommonTypes.Interfaces;

namespace TestRunner.CommonTypes.Implementations
{
    public class TestRunResult : ITestRunResult
    {
        public TestRunStatus TestRunStatus { get; set; }
        public object ActualResult { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
