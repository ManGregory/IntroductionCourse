using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLMS.Models.ViewModel
{
    public class StudentCodingHomeworkViewModel
    {
        public int HomeworkId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string TemplateCode { get; set; }
        public int AttemptsCount { get; set; }
    }
}
