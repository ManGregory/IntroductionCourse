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
            methodTestInfoProvider.ConvertFunction = ConvertToCommonTest;
            return new DbTestManager
            {
                Timeout = timeout,
                SourceCode = sourceCode,
                TestInfoProvider = methodTestInfoProvider,
                MethodTestRunner = methodTestRunner
            };
        }

        private IMethodTestInfo ConvertToCommonTest(CodingTest codingTest)
        {
            return new MethodTestInfo()
            {
                Id = codingTest.Id,
                InputParameters = Convert(codingTest.InputParameters),
                ExpectedResult = Convert(codingTest.ExpectedResult)[0]
            };
        }

        private object[] Convert(string param)
        {
            var json = JObject.Parse(param);
            var result = new List<object>();
            foreach (var x in json)
            {
                object value = null;
                if (x.Key == "int")
                {
                    value = (int)x.Value;
                }
                else if (x.Key == "bool")
                {
                    value = (bool)x.Value;
                }
                result.Add(value);
            }
            return result.ToArray();
        }

        private async Task StoreResultsAsync(string sourceCode, IDictionary<IMethodTestInfo, IMethodTestRunResult> results)
        {            
            foreach (var result in results)
            {
                var codingHomeworkRun = new CodingHomeworkRun()
                {
                    User = _currentUser,
                    CodingHomework = _context.CodingHomeworks.FirstOrDefault(hw => hw.Id == _homeworkId),
                    SourceCode = sourceCode
                };
                var testRun = new CodingHomeworkTestRun()
                {
                    CodingHomeworkRun = codingHomeworkRun,
                    CodingTest = GetCodingTest(result.Key),
                    Result = result.Value.ActualResult?.ToString() ?? string.Empty,
                    Message = result.Value.Message,
                    TestRunStatus = result.Value.TestRunStatus,
                    IsCompilation = result.Key.IsCompilation
                };
                _context.Add(codingHomeworkRun);
                _context.Add(testRun);                
            };
            await _context.SaveChangesAsync();
        }

        private CodingTest GetCodingTest(IMethodTestInfo key)
        {
            var codingTest = new CodingTest();
            if (key.IsCompilation)
            {
                codingTest.CodingHomeworkId = _homeworkId;
                codingTest.Name = "Compilation";
                _context.Add(codingTest);
            }
            else
            {
                codingTest = _context.CodingTests.FirstOrDefault(ct => ct.Id == key.Id);
            }
            return codingTest;
        }

        public async Task<IDictionary<IMethodTestInfo, IMethodTestRunResult>> Run(string sourceCode)
        {
            var homework = await _context.CodingHomeworks.FirstOrDefaultAsync(homework => homework.Id == _homeworkId);
            dbTestManager = CreateDbTestManager(sourceCode, homework.EntryMethodName, homework.EntryType);
            var results = await dbTestManager.RunAsync();
            await StoreResultsAsync(sourceCode, results);
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