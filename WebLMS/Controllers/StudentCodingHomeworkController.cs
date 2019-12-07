using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLMS.Data;
using WebLMS.Models;
using WebLMS.Models.ViewModel;
using WebLMS.Services;

namespace WebLMS.Controllers
{
    public class StudentCodingHomeworkController : Controller
    {
        private LMSDbContext _context;
        UserManager<ApplicationUser> _manager;

        public StudentCodingHomeworkController(LMSDbContext context, UserManager<ApplicationUser> manager)
        {
            _context = context;
            _manager = manager;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var codingHomework = await _context.CodingHomeworks.FirstOrDefaultAsync(ch => ch.Id == id);
            if (codingHomework == null)
            {
                return NotFound();
            }

            var codingHomeworkViewModel = new StudentCodingHomeworkViewModel()
            {
                HomeworkId = codingHomework.Id,
                Subject = codingHomework.Subject,
                Description = codingHomework.Description
            };
            codingHomeworkViewModel.TemplateCode = await GetTemplateCode(codingHomework);
            codingHomeworkViewModel.AttemptsCount = await GetAttemptsCount(codingHomework);

            return View(codingHomeworkViewModel);
        }        

        [HttpPost]
        public async Task<IActionResult> TestUserSourceCode(int id, string sourceCode)
        {
            var testManagerService = new TestManagerService(_context, id, await GetCurrentUser());
            var testResult = await testManagerService.Run(sourceCode);
            string result = testManagerService.IsTimedOut ? 
                "Timeout" : 
                string.Join(Environment.NewLine, testResult.Values.Select(res => res.Message));
            return new JsonResult(result);
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _manager.GetUserAsync(HttpContext.User);
        }

        private async Task<int> GetAttemptsCount(CodingHomework codingHomework)
        {
            var user = await GetCurrentUser();
            return await _context.CodingHomeworkRuns
                .CountAsync(homeworkRun => homeworkRun.User.Id == user.Id && homeworkRun.CodingHomework.Id == codingHomework.Id);
        }

        private async Task<string> GetTemplateCode(CodingHomework codingHomework)
        {
            string sourceCode = codingHomework.TemplateCode;

            var user = await GetCurrentUser();
            var codingHomeworkRun = await _context.CodingHomeworkRuns
                .OrderByDescending(homework => homework.StartTime)
                .FirstOrDefaultAsync(homework => homework.CodingHomework.Id == codingHomework.Id && homework.User.Id == user.Id);
            if (codingHomeworkRun != null)
            {
                sourceCode = codingHomeworkRun.SourceCode;
            }

            return sourceCode;
        }
    }
}