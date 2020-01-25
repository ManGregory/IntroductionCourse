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
using WebLMS.Identity;
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
        IdentityUtils _identityUtils;

        public StudentCodingHomeworkController(LMSDbContext context, UserManager<ApplicationUser> manager)
        {
            _context = context;
            _manager = manager;
            _identityUtils = new IdentityUtils(manager, HttpContext?.User);
        }

        public async Task<IActionResult> Index(int? id, string email)
        {
            if (id == null) return NotFound();

            _identityUtils.CurrentUser = HttpContext.User;

            if (!await _identityUtils.IsUserCanViewOtherUsers(email)) return Forbid();

            var codingHomework = await _context.CodingHomeworks.FirstOrDefaultAsync(ch => ch.Id == id);
            if (codingHomework == null) return NotFound();

            var user = await _identityUtils.GetUser(email);

            var codingHomeworkViewModel = new StudentCodingHomeworkViewModel()
            {
                HomeworkId = codingHomework.Id,
                Subject = codingHomework.Subject,
                Description = codingHomework.Description,
                MaxAttemptsCount = codingHomework.MaxAttempts,
                UserEmail = string.IsNullOrEmpty(email) ? string.Empty : $"{user.Email} - {user.StudentName}"
            };
            codingHomeworkViewModel.TemplateCode = await GetTemplateCode(codingHomework, email);
            codingHomeworkViewModel.AttemptsCount = await GetAttemptsCount(codingHomework, email);
            codingHomeworkViewModel.LastAttempt = await GetLastAttempt(codingHomework, email);

            return View(codingHomeworkViewModel);
        }

        private async Task<StudentCodingHomeworkResultViewModel> GetLastAttempt(CodingHomework codingHomework, string email)
        {
            var lastRun = await GetLastCodingHomeworkRun(codingHomework, email);
            var lastTestRuns = new List<CodingHomeworkTestRun>();
            if (lastRun != null)
            {
                lastTestRuns = await _context.CodingHomeworkTestRuns
                    .AsNoTracking()
                    .Where(run => run.CodingHomeworkRunId == lastRun.Id)
                    .Include(run => run.CodingTest)
                    .Include(run => run.CodingTest.CodingHomework)
                    .OrderBy(run => run.EndTime)
                    .ToListAsync();
            }
            return StudentCodingHomeworkResultAssembler.Assemble(lastTestRuns);
        }

        [HttpPost]
        public async Task<IActionResult> TestUserSourceCode(int id, string sourceCode)
        {
            _logger.LogInformation("SourceCode: {0}", sourceCode);

            _identityUtils.CurrentUser = HttpContext.User;

            var testManagerService = new TestManagerService(_context, id, await _identityUtils.GetUser(string.Empty));
            var testRuns = await testManagerService.Run(sourceCode);
            string result = testManagerService.IsTimedOut ? 
                "Timeout" : 
                string.Join(Environment.NewLine, testRuns.Select(run => run.Message));
            _logger.LogInformation("Result: {0}", result);

            var homeworkResult = StudentCodingHomeworkResultAssembler.Assemble(testRuns);

            return PartialView("_CodingTestResultView", homeworkResult);
        }

        private async Task<int> GetAttemptsCount(CodingHomework codingHomework, string email)
        {
            var user = await _identityUtils.GetUser(email);
            return await _context.CodingHomeworkRuns
                .CountAsync(homeworkRun => homeworkRun.User.Id == user.Id && homeworkRun.CodingHomework.Id == codingHomework.Id);
        }

        private async Task<string> GetTemplateCode(CodingHomework codingHomework, string email)
        {
            string sourceCode = codingHomework.TemplateCode;

            CodingHomeworkRun codingHomeworkRun = await GetLastCodingHomeworkRun(codingHomework, email);
            if (codingHomeworkRun != null)
            {
                sourceCode = codingHomeworkRun.SourceCode;
            }

            return sourceCode;
        }

        private async Task<CodingHomeworkRun> GetLastCodingHomeworkRun(CodingHomework codingHomework, string email)
        {
            var user = await _identityUtils.GetUser(email);
            return await _context.CodingHomeworkRuns
                .OrderByDescending(homework => homework.StartTime)
                .FirstOrDefaultAsync(homework => homework.CodingHomework.Id == codingHomework.Id && homework.User.Id == user.Id);
        }
    }
}