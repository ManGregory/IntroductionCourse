using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index()
        {
            var lectures = await _context.Lectures.AsNoTracking().Select(lec => new StudentLectureViewModel 
            { 
                Id = lec.Id, 
                Title = lec.Title, 
                Description = lec.Description, 
                IsAvailable = lec.IsAvailable
            }).ToListAsync();
            return View(lectures);
        }

        public async Task<IActionResult> LectureContent(int? lectureId)
        {
            if (lectureId == null) return NotFound();

            var lecture = _context.Lectures.AsNoTracking().FirstOrDefault(lec => lec.Id == lectureId);
            if (lecture == null) return NotFound();

            var homeworks = await _context.CodingHomeworks.AsNoTracking()
                .Where(homework => homework.LectureId == lectureId).ToListAsync();

            var model = new StudentLectureViewModel()
            {
                Id = lecture.Id,
                Title = lecture.Title,
                Description = lecture.Description,
                IsAvailable = lecture.IsAvailable,
                StudentHomeworks = homeworks.Select(hom => new StudentHomeworkViewModel()
                {
                    Id = hom.Id,
                    Title = hom.Subject,
                    Description = hom.Description,
                    HomeworkType = HomeworkType.Coding
                })
            };
            return PartialView("_HomeworkView", model);
        }
    }
}