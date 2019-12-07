using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
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
        DbTestManager dbTestManager;

        public TestManagerService(LMSDbContext context, int homeworkId)
        {
            _context = context;
            _homeworkId = homeworkId;            
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

        public async Task<IDictionary<IMethodTestInfo, IMethodTestRunResult>> Run(string sourceCode)
        {
            var homework = await _context.CodingHomeworks.FirstOrDefaultAsync(homework => homework.Id == _homeworkId);
            dbTestManager = CreateDbTestManager(sourceCode, homework.EntryMethodName, homework.EntryType);
            return await dbTestManager.RunAsync();
        }

        public void Cancel()
        {
            dbTestManager.Cancel();
        }
    }
}