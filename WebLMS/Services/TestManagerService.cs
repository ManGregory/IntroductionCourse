using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
using WebLMS.Common;
using WebLMS.Data;
using WebLMS.Models;
using WebLMS.Services.TestManager;
using WebLMS.TestManager;
using WebLMS.TestManager.Providers;

namespace WebLMS.Services
{
    public class TestManagerService
    {
        private readonly ILogger _logger = LogFactory.CreateLogger<TestManagerService>();
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

        private DbTestManager CreateDbTestManager(string sourceCode, CodingHomework homework)
        {
            IMethodCompiler methodCompiler = new RoslynMethodCompiler
            {
                EntryMethod = homework.EntryMethodName,
                EntryType = homework.EntryType,
            };
            ITestRunner testRunner = CreateTestRunner(homework.CodingTestType);
            testRunner.MethodCompiler = methodCompiler;
            var testInfoProvider = new DbMethodTestInfoProvider(_context, _homeworkId);
            if (homework.CodingTestType == CodingTestType.Method)
            {
                testInfoProvider.ConvertFunction = JsonTypesConverter.ConvertToMethodTest;
            }
            else
            {
                testInfoProvider.ConvertFunction = JsonTypesConverter.ConvertToConsoleTest;
            }                           
            return new DbTestManager
            {
                Timeout = Constants.TimeoutPeriod,
                SourceCode = sourceCode,
                TestInfoProvider = testInfoProvider,
                TestRunner = testRunner
            };
        }

        private static ITestRunner CreateTestRunner(CodingTestType codingTestType)
        {
            ITestRunner runner;
            if (codingTestType == CodingTestType.Method)
            {
                runner = new MethodTestRunner();
            }
            else
            {
                runner = new ConsoleTestRunner();
            }
            return runner;
        }

        private async Task<IEnumerable<CodingHomeworkTestRun>> StoreResultsAsync(CodingHomework homework, string sourceCode, IDictionary<ITestInfo, ITestRunResult> results, DateTime startTime, DateTime endTime)
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

            var testRuns = new List<CodingHomeworkTestRun>(results.Count);
            foreach (var result in results)
            {
                var testRun = new CodingHomeworkTestRun()
                {
                    CodingHomeworkRun = codingHomeworkRun,
                    CodingTest = GetCodingTest(result),
                    Result = GetResult(homework.CodingTestType, result),
                    Message = result.Value.Message,
                    TestRunStatus = result.Value.TestRunStatus,
                    IsCompilation = result.Key.IsCompilation,
                    StartTime = result.Value.StartTime,
                    EndTime = result.Value.EndTime
                };
                if (result.Value.TestRunStatus == TestRunner.CommonTypes.TestRunStatus.UnknownException)
                {
                    testRun.Exception = $"{result.Value.Exception.Message} {Environment.NewLine}{result.Value.Exception.StackTrace}";
                }
                _context.Add(testRun);
                testRuns.Add(testRun);
            };
            await _context.SaveChangesAsync();
            return testRuns;
        }

        private string GetResult(CodingTestType codingTestType, KeyValuePair<ITestInfo, ITestRunResult> pair)
        {
            string result = string.Empty;
            if (codingTestType == CodingTestType.Method)
            {
                result = pair.Value.ActualResult.ToHumanView();
            }
            else
            {
                var results = pair.Value.ActualResult as IEnumerable<ConsoleStepResult>;
                if (results != null)
                {
                    result = JsonConvert.SerializeObject(results);
                }
            }
            return result;
        }

        private bool IsPredefinedTest(KeyValuePair<ITestInfo, ITestRunResult> pair)
        {
            return
                pair.Key.IsCompilation ||
                pair.Value.TestRunStatus == TestRunner.CommonTypes.TestRunStatus.Timeout ||
                pair.Value.TestRunStatus == TestRunner.CommonTypes.TestRunStatus.UnknownException;
        }

        private string GetPredefinedTestName(KeyValuePair<ITestInfo, ITestRunResult> pair)
        {
            if (pair.Key.IsCompilation) return "Compilation";
            if (pair.Value.TestRunStatus == TestRunner.CommonTypes.TestRunStatus.Timeout) return "Timeout";
            if (pair.Value.TestRunStatus == TestRunner.CommonTypes.TestRunStatus.UnknownException) return "UnknownException";

            throw new InvalidOperationException($"{pair.Key.ToString()}, {pair.Value.ToString()}");
        }

        private CodingTest GetCodingTest(KeyValuePair<ITestInfo, ITestRunResult> testRun)
        {
            CodingTest codingTest;
            if (IsPredefinedTest(testRun))
            {
                string name = GetPredefinedTestName(testRun);
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
                codingTest = _context.CodingTests.FirstOrDefault(ct => ct.Id == testRun.Key.Id);
            }
            return codingTest;
        }

        public async Task<IEnumerable<CodingHomeworkTestRun>> Run(string sourceCode)
        {
            _logger.LogInformation("Invoke Run method");

            _logger.LogInformation("Get homework with id = {0}", _homeworkId);
            var homework = await _context.CodingHomeworks.FirstOrDefaultAsync(homework => homework.Id == _homeworkId);
            dbTestManager = CreateDbTestManager(sourceCode, homework);
            var startTime = DateTime.Now;

            _logger.LogInformation("Run test manager");
            var results = await dbTestManager.RunAsync();
            var endTime = DateTime.Now;

            _logger.LogInformation("Store results");
            var testRuns = await StoreResultsAsync(homework, sourceCode, results, startTime, endTime);

            return testRuns;
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