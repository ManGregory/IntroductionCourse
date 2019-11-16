using RunAndTest.Compilers;
using RunAndTest.DTO;
using RunAndTest.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RunAndTest.TestRunners
{
    public class MethodTestRunner : IMethodTestRunner
    {
        public IMethodCompiler MethodCompiler { get; set; }
        public IMethodTestProvider MethodTestProvider { get; set; }

        #region Message Builder
        private string BuildSuccessfullMessage(IMethodTestInfo test)
        {
            return BuildMessage(test, true);
        }

        private string BuildErrorMessage(IMethodTestInfo test, object actualResult)
        {
            return BuildMessage(test, false, actualResult);
        }

        private string BuildMessage(IMethodTestInfo test, bool passed, object actualResult = null)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Test {test.Name} {(passed ? "Passed" : "Failed")}");
            sb.AppendLine($"Input params: {string.Join(", ", test.InputParameters)}");
            sb.AppendLine($"Expected result: {test.ExpectedResult}{(passed ? "" : $", but get {actualResult}")}");
            if (!string.IsNullOrEmpty(test.AdditionalMessage)) sb.AppendLine($"Additional message: {test.AdditionalMessage}");

            return sb.ToString();
        }
        #endregion

        private string Execute(MethodInfo testMethod, IMethodTestInfo test)
        {
            var actualResult = testMethod.Invoke(null, test.InputParameters);
            return Equals(actualResult, test.ExpectedResult) ? BuildSuccessfullMessage(test) : BuildErrorMessage(test, actualResult);
        }

        public IEnumerable<string> Run()
        {
            var testResults = new List<string>();
            var testMethod = MethodCompiler.Compile();
            return testMethod == null 
                ? MethodCompiler.CompilationErrors
                : new List<string>(MethodTestProvider.MethodTests.Select((test) => Execute(testMethod, test)));
        }
    }
}