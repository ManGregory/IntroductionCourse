using System.ComponentModel.DataAnnotations;

namespace WebLMS.Models.ViewModel
{
    public class StudentCodingHomeworkViewModel
    {
        private static string InfiniteAttempts = @"∞";

        public int HomeworkId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string UserEmail { get; set; }
        public string HomeworkType { get; set; }

        [StringLength(10000, MinimumLength = 20)]
        [Display(Name="Код")]
        public string TemplateCode { get; set; }
        public int AttemptsCount { get; set; }
        public int MaxAttemptsCount { get; set; }
        public StudentCodingHomeworkResultViewModel LastAttempt { get; set; }
        public string MaxAttempts
        {
            get
            {
                return MaxAttemptsCount == 0 ? InfiniteAttempts : MaxAttemptsCount.ToString();
            }
        }

        public bool ExceededAttempts
        {
            get
            {
                return AttemptsCount >= MaxAttemptsCount && MaxAttemptsCount != 0;
            }
        }
    }
}
