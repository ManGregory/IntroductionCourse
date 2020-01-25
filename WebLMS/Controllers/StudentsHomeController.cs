using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebLMS.Data;
using WebLMS.Identity;
using WebLMS.Models;
using WebLMS.Models.ViewModel;

namespace WebLMS.Controllers
{
    [Authorize]
    public class StudentsHomeController : Controller
    {
        private readonly LMSDbContext _context;
        UserManager<ApplicationUser> _manager;
        IdentityUtils _identityUtils;

        public StudentsHomeController(LMSDbContext context, UserManager<ApplicationUser> manager)
        {
            _context = context;
            _manager = manager;
            _identityUtils = new IdentityUtils(_manager, HttpContext?.User);
        }

        public async Task<IActionResult> Index()
        {
            _identityUtils.CurrentUser = HttpContext.User;
            var lectures = await _context.Lectures.AsNoTracking().Select(lec => new StudentLectureViewModel 
            { 
                Id = lec.Id, 
                Title = lec.Title, 
                Description = lec.Description, 
                IsAvailable = lec.IsAvailable
            }).ToListAsync();

            var viewUsers = await _manager.Users.AsNoTracking()
                .Select(user => new CheckUserViewModel
                {
                    UserEmail = user.Email,
                    UserName = user.StudentName,
                    GroupNum = user.Group
                })
                .ToListAsync();

            ViewBag.Students = new SelectList(viewUsers.Select(user => new { Id = user.UserEmail, Name = $"{user.UserEmail} - {user.UserName}" }), "Id", "Name");

            return View(lectures);
        }

        public async Task<IActionResult> LectureContent(int? lectureId, string email)
        { 
            if (lectureId == null) return NotFound();

            _identityUtils.CurrentUser = HttpContext.User;

            if (!await _identityUtils.IsUserCanViewOtherUsers(email)) return Forbid();

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
                Email = email
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
                    Status = await GetStatus(hom, email)
                });
            }
            model.StudentHomeworks = studentHomeworks;
            return PartialView("_HomeworkView", model);
        }

        /*private async Task<bool> IsUserCanViewOtherUsers(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail)) return true;

            return await IsCurrentUserAdmin();            
        }

        private async Task<bool> IsCurrentUserAdmin()
        {
            var user = await GetCurrentUser();
            return await _manager.IsInRoleAsync(user, "Administrator");
        }*/

        private async Task<HomeworkStatus> GetStatus(CodingHomework homework, string userEmail)
        {
            var testsCount = await _context.CodingTests.AsNoTracking()
                .CountAsync(cod => cod.CodingHomeworkId == homework.Id && 
                                   cod.Name != "Compilation" && cod.Name != "Timeout" && cod.Name != "UnknownException");
            if (testsCount == 0) return HomeworkStatus.NoTests;

            var user = await _identityUtils.GetUser(userEmail);
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

        /*private async Task<ApplicationUser> GetUser(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail)) return await GetCurrentUser();
            return await _manager.FindByEmailAsync(userEmail);
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _manager.GetUserAsync(HttpContext.User);
        }*/
    }
}