using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLMS.Models.ViewModel
{
    public class StudentHomeworkViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public HomeworkType HomeworkType { get; set; }
        public bool IsPassed { get; set; }
        public string Attempts { get; set; }
    }
}
