using System;
using System.Collections.Generic;

namespace WebLMS.Models.ViewModel
{
    public class StudentLectureViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime AvailableFrom { get; set; }
        public IEnumerable<StudentHomeworkViewModel> StudentHomeworks { get; set; }
        public string Email { get; set; }
    }
}
