using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TestRunner.CommonTypes;
using TestRunner.CommonTypes.Interfaces;

namespace TestRunner.TestRunners.Implementations
{
    public class MethodTestRunner : BaseTestRunner
    {
        protected override string BuildMessage(ITestInfo test, ITestRunResult testRunResult)
        {
            var sb = new StringBuilder();
            bool isPassed = testRunResult.TestRunStatus == TestRunStatus.Passed;
            sb.AppendLine($"Test {test.Name} {(isPassed ? "Passed" : "Failed")}");
            sb.AppendLine($"Input params: {string.Join(", ", ((IMethodTestInfo) test).InputParameters)}");
            sb.AppendLine($"Expected result: {test.ExpectedResult}{(isPassed ? "" : $", but get {testRunResult.ActualResult}")}");
            if (testRunResult.TestRunStatus == TestRunStatus.TargetException) sb.AppendLine($"There was an exception during executing of the test");

            return sb.ToString();
        }

        protected override object Invoke(MethodInfo testMethod, ITestInfo test)
        {
            return testMethod.Invoke(null, ((IMethodTestInfo)test).InputParameters);
        }

        protected override bool IsTestPassed(ITestInfo test, ITestRunResult testRunResult)
        {
            return Equals(testRunResult.ActualResult, test.ExpectedResult);
        }
    }
}
