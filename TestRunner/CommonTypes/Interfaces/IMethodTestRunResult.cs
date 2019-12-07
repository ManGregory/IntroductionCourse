using System;

namespace TestRunner.CommonTypes.Interfaces
{
    public interface IMethodTestRunResult
    {
        TestRunStatus TestRunStatus { get; set; }
        object ActualResult { get; set; }
        string Message { get; set; }
        Exception Exception { get; set; }
        DateTime? StartTime { get; set; }
        DateTime? EndTime { get; set; }
    }
}
