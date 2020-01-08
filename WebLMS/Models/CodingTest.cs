using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [StringLength(50, MinimumLength = 5)]
        public string Name { get; set; }
        [StringLength(4000, MinimumLength = 5)]
        public string InputParameters { get; set; }
        [StringLength(4000, MinimumLength = 5)]
        public string ExpectedResult { get; set; }

        public virtual CodingHomework CodingHomework { get; set; }
    }
}
