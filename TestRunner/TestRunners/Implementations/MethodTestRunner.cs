using System.Linq;
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
            var resultType = test.ExpectedResult.GetType();
            return resultType.IsArray 
                ? CompareArrays(testRunResult.ActualResult, test.ExpectedResult)
                : Equals(testRunResult.ActualResult, test.ExpectedResult);
        }

        private bool CompareArrays(object actualResult, object expectedResult)
        {
            bool areEqual = false;
            var resultType = expectedResult.GetType();
            if (resultType.GetElementType() == typeof(int))
            {
                areEqual = Enumerable.SequenceEqual((int[])actualResult, (int[])expectedResult);
            }
            else if (resultType.GetElementType() == typeof(double))
            {
                areEqual = Enumerable.SequenceEqual((double[])actualResult, (double[])expectedResult);
            }
            else if (resultType.GetElementType() == typeof(decimal))
            {
                areEqual = Enumerable.SequenceEqual((decimal[])actualResult, (decimal[])expectedResult);
            }
            return areEqual;
        }
    }
}
