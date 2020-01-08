using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebLMS.Models
{
    public enum CodingTestType
    {
        Method,
        Console
    }

    public class CodingHomework
    {
        public int Id { get; set; }
        public int LectureId { get; set; }

        [StringLength(30, MinimumLength = 5)]
        public string Subject { get; set; }

        [StringLength(1000, MinimumLength = 5)]
        public string Description { get; set; }

        [StringLength(1000, MinimumLength = 5)]
        public string TemplateCode { get; set; }
        public CodingTestType CodingTestType { get; set; }

        [StringLength(30, MinimumLength = 5)]
        public string EntryType { get; set; }

        [StringLength(30, MinimumLength = 5)]
        public string EntryMethodName { get; set; }
        [DefaultValue(0)]
        public int MaxAttempts { get; set; }
        
        public virtual Lecture Lecture { get; set; }
    }
}
