using RunAndTest.Compilers;
using RunAndTest.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RunAndTest.TestRunners
{
    public class MethodTestRunner : IMethodTestRunner
    {
        public IMethodCompiler MethodCompiler { get; set; }

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

        private string Execute(MethodInfo testMethod, IMethodTestInfo test, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = TryInvoke(testMethod, test);
            if (result.hasException) test.AdditionalMessage = "There is an TargetInvocationException during performing of the test";
            return Equals(result.actualResult, test.ExpectedResult) ? BuildSuccessfullMessage(test) : BuildErrorMessage(test, result.actualResult);
        }

        private static (object actualResult, bool hasException) TryInvoke(MethodInfo testMethod, IMethodTestInfo test)
        {
            try
            {
                return (testMethod.Invoke(null, test.InputParameters), false);
            }
            catch (TargetInvocationException)
            {
                return ("exception", true);
            }
        }

        private IEnumerable<string> ExecuteTests(IEnumerable<IMethodTestInfo> tests, MethodInfo testMethod, CancellationToken cancellationToken = default)
        {
            return new List<string>(tests.Select((test) => Execute(testMethod, test, cancellationToken)));
        }

        public IEnumerable<string> Run(string sourceCode, IEnumerable<IMethodTestInfo> tests)
        {
            var testMethod = MethodCompiler.Compile(sourceCode);
            return testMethod == null
                ? MethodCompiler.CompilationErrors
                : ExecuteTests(tests, testMethod);
        }

        public async Task<IEnumerable<string>> RunAsync(string sourceCode, IEnumerable<IMethodTestInfo> tests, CancellationToken cancellationToken)
        {
            var testMethod = await MethodCompiler.CompileAsync(sourceCode, cancellationToken);
            return testMethod == null
                ? MethodCompiler.CompilationErrors
                : await Task.Run(() => ExecuteTests(tests, testMethod, cancellationToken));           
        }
    }
}