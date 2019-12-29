using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebLMS.Assemblers;
using WebLMS.Common;
using WebLMS.Data;
using WebLMS.Models;
using WebLMS.Models.ViewModel;
using WebLMS.Services;

namespace WebLMS.Controllers
{
    [Authorize]
    public class StudentCodingHomeworkController : Controller
    {
        private readonly ILogger _logger = LogFactory.CreateLogger<StudentCodingHomeworkController>();
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
                Description = codingHomework.Description,
                MaxAttemptsCount = codingHomework.MaxAttempts
            };
            codingHomeworkViewModel.TemplateCode = await GetTemplateCode(codingHomework);
            codingHomeworkViewModel.AttemptsCount = await GetAttemptsCount(codingHomework);
            codingHomeworkViewModel.LastAttempt = await GetLastAttempt(codingHomework);

            return View(codingHomeworkViewModel);
        }

        private async Task<StudentCodingHomeworkResultViewModel> GetLastAttempt(CodingHomework codingHomework)
        {
            var lastRun = await GetLastCodingHomeworkRun(codingHomework);
            var lastTestRuns = new List<CodingHomeworkTestRun>();
            if (lastRun != null)
            {
                lastTestRuns = await _context.CodingHomeworkTestRuns
                    .AsNoTracking()
                    .Where(run => run.CodingHomeworkRunId == lastRun.Id)
                    .Include(run => run.CodingTest)
                    .Include(run => run.CodingTest.CodingHomework)
                    .ToListAsync();
            }
            return StudentCodingHomeworkResultAssembler.Assemble(lastTestRuns);
        }

        [HttpPost]
        public async Task<IActionResult> TestUserSourceCode(int id, string sourceCode)
        {
            _logger.LogInformation("SourceCode: {0}", sourceCode);
            var testManagerService = new TestManagerService(_context, id, await GetCurrentUser());
            var testRuns = await testManagerService.Run(sourceCode);
            string result = testManagerService.IsTimedOut ? 
                "Timeout" : 
                string.Join(Environment.NewLine, testRuns.Select(run => run.Message));
            _logger.LogInformation("Result: {0}", result);

            var homeworkResult = StudentCodingHomeworkResultAssembler.Assemble(testRuns);

            return PartialView("_CodingTestResultView", homeworkResult);
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

            CodingHomeworkRun codingHomeworkRun = await GetLastCodingHomeworkRun(codingHomework);
            if (codingHomeworkRun != null)
            {
                sourceCode = codingHomeworkRun.SourceCode;
            }

            return sourceCode;
        }

        private async Task<CodingHomeworkRun> GetLastCodingHomeworkRun(CodingHomework codingHomework)
        {
            var user = await GetCurrentUser();
            return await _context.CodingHomeworkRuns
                .OrderByDescending(homework => homework.StartTime)
                .FirstOrDefaultAsync(homework => homework.CodingHomework.Id == codingHomework.Id && homework.User.Id == user.Id);
        }
    }
}