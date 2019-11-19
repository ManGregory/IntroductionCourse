using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLMS.Models
{
    public class CodingTest
    {
        public int Id { get; set; }
        public int HomeworkId { get; set; }
        public string Name { get; set; }
        public string InputParameters { get; set; }
        public string ExpectedResult { get; set; }
    }
}
