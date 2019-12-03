using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestRunner.CommonTypes.Implementations;
using TestRunner.Compilers.Implementations;
using TestRunner.Compilers.Interfaces;
using TestRunner.TestRunners.Implementations;
using TestRunner.TestRunners.Interfaces;
using WebLMS.Data;
using WebLMS.Models;
using WebLMS.Models.ViewModel;

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

            return View(codingHomeworkViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> TestUserSourceCode(int id, string sourceCode)
        {
            var codingHomework = await _context.CodingHomeworks.FirstOrDefaultAsync(ch => ch.Id == id);
            var codingTests = await _context.CodingTests.Where(test => test.CodingHomeworkId == codingHomework.Id).ToListAsync();
            string result = RunTests(sourceCode, codingHomework, codingTests);
            return new JsonResult(result);
        }

        private string RunTests(string sourceCode, CodingHomework codingHomework, List<CodingTest> codingTests)
        {
            IMethodCompiler methodCompiler = new RoslynMethodCompiler
            {
                EntryMethod = codingHomework.EntryMethodName,
                EntryType = codingHomework.EntryType,
            };
            IMethodTestRunner methodTestRunner = new MethodTestRunner
            {
                MethodCompiler = methodCompiler
            };
            var tests = codingTests.Select(ct => new MethodTestInfo()
            {
                Name = ct.Name,
                InputParameters = ct.InputParameters.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(s => (object) Convert.ToInt32(s)).ToArray(),
                ExpectedResult = ct.ExpectedResult
            });
            var result = methodTestRunner.Run(sourceCode, tests);

            return string.Join(Environment.NewLine, result.Values.Select(res => res.Message));
        }

        private async Task<ApplicationUser> GetCurrentUser()
        {
            return await _manager.GetUserAsync(HttpContext.User);
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