using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRunner.CommonTypes;
using TestRunner.CommonTypes.Implementations;

namespace WebLMS.Models.ViewModel
{
    public class TestRunResultViewModel
    {
        public string TestName { get; set; }
        public TestRunStatus TestRunStatus { get; set; }
        public CodingTestType TestType { get; set; }
        public IEnumerable<string> InputParams { get; set; }
        public string Expected { get; set; }
        public string Actual { get; set; }
        public IEnumerable<ConsoleStepResult> StepResults { get; set; }
        public int ExecutionTime { get; set; }
    }

    public class StudentCodingHomeworkResultViewModel
    {
        public bool IsTimedOut { get; set; }
        public int TimeoutPeriod { get; set; }
        public bool IsCompilationFailed { get; set; }
        public string CompilationErrors { get; set; }
        public bool IsUnknownException { get; set; }
        public string ExceptionText { get; set; }
        public bool IsPassed { get; set; }
        public IEnumerable<TestRunResultViewModel> TestRunResults { get; set; }
    }
}
