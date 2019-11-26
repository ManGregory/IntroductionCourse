using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebLMS.Models
{
    public class CodingTest
    {
        public int Id { get; set; }
        [ForeignKey("CodingHomework")]
        public int CodingHomeworkId { get; set; }
        public string Name { get; set; }
        public string InputParameters { get; set; }
        public string ExpectedResult { get; set; }

        public virtual CodingHomework CodingHomework { get; set; }
    }
}
