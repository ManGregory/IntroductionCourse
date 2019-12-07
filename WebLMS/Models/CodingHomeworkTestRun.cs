using System;
using TestRunner.CommonTypes;

namespace WebLMS.Models
{
    public class CodingHomeworkTestRun
    {
        public int Id { get; set; }
        public int CodingHomeworkRunId { get; set; }
        public int CodingTestId { get; set; }
        public string Result { get; set; }
        public string Message { get; set; }
        public TestRunStatus TestRunStatus { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsCompilation { get; set; }

        public virtual CodingHomeworkRun CodingHomeworkRun { get; set; }
        public virtual CodingTest CodingTest { get; set; }
    }
}
