using System;
using System.Collections.Generic;
using System.Text;

namespace RunAndTest.DTO
{
    public interface IMethodTestRunResult
    {
        TestRunStatus TestRunStatus { get; set; }
        object ActualResult { get; set; }
        string Message { get; set; }
    }
}
