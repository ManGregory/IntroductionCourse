using System.ComponentModel;

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
        public string Subject { get; set; }
        public string Description { get; set; }
        public string TemplateCode { get; set; }
        public CodingTestType CodingTestType { get; set; }
        public string EntryType { get; set; }
        public string EntryMethodName { get; set; }
        [DefaultValue(0)]
        public int MaxAttempts { get; set; }
        
        public virtual Lecture Lecture { get; set; }
    }
}
