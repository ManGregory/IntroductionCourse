using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TestRunner.CommonTypes;
using TestRunner.CommonTypes.Implementations;
using WebLMS.Common;
using WebLMS.Models;
using WebLMS.Models.ViewModel;
using WebLMS.Services.TestManager;

namespace WebLMS.Assemblers
{
    public static class StudentCodingHomeworkResultAssembler
    {
        public static StudentCodingHomeworkResultViewModel Assemble(IEnumerable<CodingHomeworkTestRun> testRuns)
        {
            var codingHomeworkResult = new StudentCodingHomeworkResultViewModel();
            if (!testRuns.Any()) return codingHomeworkResult;

            var runCount = testRuns.Count();
            var firstRun = testRuns.First();
            if (runCount == 1)
            {
                codingHomeworkResult.IsPassed = false;
                codingHomeworkResult.IsTimedOut = firstRun.TestRunStatus == TestRunStatus.Timeout;
                codingHomeworkResult.TimeoutPeriod = Constants.TimeoutPeriod / 1000;
                codingHomeworkResult.IsUnknownException = firstRun.TestRunStatus == TestRunStatus.UnknownException;
                // todo: add exception to coding homework test run
                codingHomeworkResult.ExceptionText = firstRun.Message;
                codingHomeworkResult.IsCompilationFailed = firstRun.TestRunStatus == TestRunStatus.CompilationFailed;
                codingHomeworkResult.CompilationErrors = firstRun.Message;
            }
            else
            {
                var testRunResults = new List<TestRunResultViewModel>(
                    testRuns
                        .Skip(1)
                        .Select(run => new TestRunResultViewModel()
                        {
                            TestName = run.CodingTest.Name,
                            TestType = run.CodingTest.CodingHomework.CodingTestType,
                            TestRunStatus = run.TestRunStatus,
                            ExecutionTime = CalcExecutionTime(run),
                            Expected = GetExpected(run),
                            Actual = GetActual(run),
                            InputParams = GetInputParams(run),
                            StepResults = GetStepResults(run)
                        })
                );
                codingHomeworkResult.TestRunResults = testRunResults;
                codingHomeworkResult.IsPassed = testRunResults.All(run => run.TestRunStatus == TestRunStatus.Passed);
            }
            return codingHomeworkResult;
        }

        private static IEnumerable<string> GetInputParams(CodingHomeworkTestRun run)
        {
            var inputParameteres = new List<string>();
            if (run.CodingTest.CodingHomework.CodingTestType == CodingTestType.Method)
            {
                var testInfo = (MethodTestInfo) JsonTypesConverter.ConvertToMethodTest(run.CodingTest);
                inputParameteres.AddRange(testInfo.InputParameters.Select(param => param.ToString()));
            }
            else
            {
                var testInfo = (ConsoleTestInfo) JsonTypesConverter.ConvertToConsoleTest(run.CodingTest);
                inputParameteres.AddRange(testInfo.ConsoleTest.MethodInputParameteres.Select(param => param.ToString()));
            }
            return inputParameteres;
        }

        private static IEnumerable<ConsoleStepResult> GetStepResults(CodingHomeworkTestRun run)
        {
            return run.CodingTest.CodingHomework.CodingTestType == CodingTestType.Console 
                ? JsonConvert.DeserializeObject<IEnumerable<ConsoleStepResult>>(run.Result)
                : new List<ConsoleStepResult>();
        }

        private static string GetActual(CodingHomeworkTestRun run)
        {
            return run.CodingTest.CodingHomework.CodingTestType == CodingTestType.Method
                ? run.Result
                : string.Empty;
        }

        private static string GetExpected(CodingHomeworkTestRun run)
        {
            var expected = string.Empty;
            if (run.CodingTest.CodingHomework.CodingTestType == CodingTestType.Method)
            {
                var testInfo = (MethodTestInfo)JsonTypesConverter.ConvertToMethodTest(run.CodingTest);
                expected = testInfo.ExpectedResult.ToString();
            }
            return expected;
        }

        private static int CalcExecutionTime(CodingHomeworkTestRun run)
        {
            return (run.EndTime - run.StartTime)?.Milliseconds ?? 0;
        }
    }
}
