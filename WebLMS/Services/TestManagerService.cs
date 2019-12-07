using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestRunner.CommonTypes.Implementations;
using TestRunner.CommonTypes.Interfaces;
using TestRunner.Compilers.Implementations;
using TestRunner.Compilers.Interfaces;
using TestRunner.TestRunners.Implementations;
using TestRunner.TestRunners.Interfaces;
using WebLMS.Data;
using WebLMS.Models;
using WebLMS.Services.TestManager;
using WebLMS.TestManager;
using WebLMS.TestManager.Providers;

namespace WebLMS.Services
{
    public class TestManagerService
    {
        private static int timeout = 10000;
        LMSDbContext _context;
        int _homeworkId;
        ApplicationUser _currentUser;
        DbTestManager dbTestManager;

        public TestManagerService(LMSDbContext context, int homeworkId, ApplicationUser currentUser)
        {
            _context = context;
            _homeworkId = homeworkId;
            _currentUser = currentUser;
        }

        private DbTestManager CreateDbTestManager(string sourceCode, string methodName, string typeName)
        {
            IMethodCompiler methodCompiler = new RoslynMethodCompiler
            {
                EntryMethod = methodName,
                EntryType = typeName,
            };
            IMethodTestRunner methodTestRunner = new MethodTestRunner
            {
                MethodCompiler = methodCompiler
            };
            var methodTestInfoProvider = new DbMethodTestInfoProvider(_context, _homeworkId);
            methodTestInfoProvider.ConvertFunction = JsonConverter.ConvertToCommonTest;
            return new DbTestManager
            {
                Timeout = timeout,
                SourceCode = sourceCode,
                TestInfoProvider = methodTestInfoProvider,
                MethodTestRunner = methodTestRunner
            };
        }        

        private async Task StoreResultsAsync(string sourceCode, IDictionary<IMethodTestInfo, IMethodTestRunResult> results, DateTime startTime, DateTime endTime)
        {
            var codingHomeworkRun = new CodingHomeworkRun()
            {
                User = _currentUser,
                CodingHomework = _context.CodingHomeworks.FirstOrDefault(hw => hw.Id == _homeworkId),
                SourceCode = sourceCode,
                StartTime = startTime,
                EndTime = endTime
            };
            _context.Add(codingHomeworkRun);
            foreach (var result in results)
            {                
                var testRun = new CodingHomeworkTestRun()
                {
                    CodingHomeworkRun = codingHomeworkRun,
                    CodingTest = GetCodingTest(result),
                    Result = result.Value.ActualResult?.ToString() ?? string.Empty,
                    Message = result.Value.Message,
                    TestRunStatus = result.Value.TestRunStatus,
                    IsCompilation = result.Key.IsCompilation,
                    StartTime = result.Value.StartTime,
                    EndTime = result.Value.EndTime
                };                
                _context.Add(testRun);
            };
            await _context.SaveChangesAsync();
        }

        private CodingTest GetCodingTest(KeyValuePair<IMethodTestInfo, IMethodTestRunResult> pair)
        {
            CodingTest codingTest;
            if (pair.Key.IsCompilation || pair.Value.TestRunStatus == TestRunner.CommonTypes.TestRunStatus.Timeout)
            {
                string name = pair.Key.IsCompilation ? "Compilation" : "Timeout";
                codingTest = _context.CodingTests.FirstOrDefault(ct => ct.CodingHomeworkId == _homeworkId && ct.Name == name);
                if (codingTest == null)
                {
                    codingTest = new CodingTest();
                    codingTest.CodingHomeworkId = _homeworkId;
                    codingTest.Name = name;
                    _context.Add(codingTest);
                }
            }
            else
            {
                codingTest = _context.CodingTests.FirstOrDefault(ct => ct.Id == pair.Key.Id);
            }
            return codingTest;
        }

        public async Task<IDictionary<IMethodTestInfo, IMethodTestRunResult>> Run(string sourceCode)
        {
            var homework = await _context.CodingHomeworks.FirstOrDefaultAsync(homework => homework.Id == _homeworkId);
            dbTestManager = CreateDbTestManager(sourceCode, homework.EntryMethodName, homework.EntryType);
            var startTime = DateTime.Now;
            var results = await dbTestManager.RunAsync();
            var endTime = DateTime.Now;
            await StoreResultsAsync(sourceCode, results, startTime, endTime);
            return results;
        }

        public bool IsTimedOut
        {
            get { return dbTestManager.IsTimedOut; }
        }

        public void Cancel()
        {
            dbTestManager.Cancel();
        }
    }
}