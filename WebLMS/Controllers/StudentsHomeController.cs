using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebLMS.Data;
using WebLMS.Models.ViewModel;

namespace WebLMS.Controllers
{
    [Authorize]
    public class StudentsHomeController : Controller
    {
        private readonly LMSDbContext _context;

        public StudentsHomeController(LMSDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var lectures = _context.Lectures.Select(lec => new StudentLectureViewModel 
            { 
                Id = lec.Id, 
                Title = lec.Title, 
                Description = lec.Description, 
                IsAvailable = lec.IsAvailable,
                StudentHomeworks = _context.CodingHomeworks.Where(coding => coding.LectureId == lec.Id)
                    .Select(coding => new StudentHomeworkViewModel()
                    {
                        Id = coding.Id,
                        Title = coding.Subject,
                        Description = coding.Description,
                        HomeworkType = HomeworkType.Coding
                    })
            });
            return View(lectures);
        }
    }
}