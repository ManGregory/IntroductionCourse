using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLMS.Data;
using WebLMS.Models;
using WebLMS.Models.ViewModel;

namespace WebLMS.Controllers
{
    [Authorize]
    public class StudentsHomeController : Controller
    {
        private readonly LMSDbContext _context;
        UserManager<ApplicationUser> _manager;

        public StudentsHomeController(LMSDbContext context, UserManager<ApplicationUser> manager)
        {
            _context = context;
            _manager = manager;
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
                IsAvailable = lecture.IsAvailable
            };
            var studentHomeworks = new List<StudentHomeworkViewModel>();
            foreach (var hom in homeworks)
            {
                studentHomeworks.Add(new StudentHomeworkViewModel()
                {
                    Id = hom.Id,
                    Title = hom.Subject,
                    Description = hom.Description,
                    HomeworkType = HomeworkType.Coding,
                    Status = await GetStatus(hom)
                });
            }
            model.StudentHomeworks = studentHomeworks;
            return PartialView("_HomeworkView", model);
        }

        private async Task<HomeworkStatus> GetStatus(CodingHomework homework)
        {
            var testsCount = await _context.CodingTests.AsNoTracking()
                .CountAsync(cod => cod.CodingHomeworkId == homework.Id && cod.Name != "Compilation");
            if (testsCount == 0) return HomeworkStatus.NoTests;

            var user = await GetCurrentUser();
            var hasRuns = await _context.CodingHomeworkRuns.AsNoTracking()
                .AnyAsync(homRun => homRun.CodingHomework.Id == homework.Id && homRun.User.Id == user.Id);
            if (!hasRuns) return HomeworkStatus.NoRun;

            var hasPassed = await _context.CodingHomeworkRuns.AsNoTracking()
                .AnyAsync(homRun => homRun.CodingHomework.Id == homework.Id && homRun.User.Id == user.Id &&
                    _context.CodingHomeworkTestRuns
                        .Where(testRun => testRun.CodingHomeworkRunId == homRun.Id)
                        .All(testRun => testRun.TestRunStatus == TestRunner.CommonTypes.TestRunStatus.Passed));
            return hasPassed ? HomeworkStatus.Passed : HomeworkStatus.Failed;
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _manager.GetUserAsync(HttpContext.User);
        }
    }
}