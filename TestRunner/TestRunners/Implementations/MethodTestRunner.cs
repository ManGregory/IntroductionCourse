using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestRunner.CommonTypes;
using TestRunner.CommonTypes.Implementations;
using TestRunner.CommonTypes.Interfaces;
using TestRunner.Compilers.Interfaces;
using TestRunner.TestRunners.Interfaces;

namespace TestRunner.TestRunners.Implementations
{
    public class MethodTestRunner : IMethodTestRunner
    {
        public IMethodCompiler MethodCompiler { get; set; }

        #region Message Builder
        private string BuildMessage(IMethodTestInfo test, IMethodTestRunResult testRunResult)
        {
            var sb = new StringBuilder();
            bool isPassed = testRunResult.TestRunStatus == TestRunStatus.Passed;
            sb.AppendLine($"Test {test.Name} {(isPassed ? "Passed" : "Failed")}");
            sb.AppendLine($"Input params: {string.Join(", ", test.InputParameters)}");
            sb.AppendLine($"Expected result: {test.ExpectedResult}{(isPassed ? "" : $", but get {testRunResult.ActualResult}")}");
            if (testRunResult.TestRunStatus == TestRunStatus.TargetException) sb.AppendLine($"There was an exception during executing of the test");

            return sb.ToString();
        }
        #endregion

        private IMethodTestRunResult Execute(MethodInfo testMethod, IMethodTestInfo test, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();            
            var testRunResult = TryInvoke(testMethod, test);
            if (testRunResult.TestRunStatus != TestRunStatus.TargetException)
            {
                testRunResult.TestRunStatus = Equals(testRunResult.ActualResult, test.ExpectedResult) ? TestRunStatus.Passed : TestRunStatus.Failed;
            }
            testRunResult.Message = BuildMessage(test, testRunResult);
            return testRunResult;
        }

        private IMethodTestRunResult TryInvoke(MethodInfo testMethod, IMethodTestInfo test)
        {
            var result = new MethodTestRunResult();
            try
            {
                result.StartTime = DateTime.Now;
                result.ActualResult = testMethod.Invoke(null, test.InputParameters);
            }
            catch (Exception ex)
            {
                result.TestRunStatus = ex is TargetInvocationException ? TestRunStatus.TargetException : TestRunStatus.UnknownException;
                result.Exception = ex;
            }
            finally
            {
                result.EndTime = DateTime.Now;
            }
            return result;
        }

        private IDictionary<IMethodTestInfo, IMethodTestRunResult> ExecuteTests(IEnumerable<IMethodTestInfo> tests, MethodInfo testMethod, CancellationToken cancellationToken = default)
        {
            var testResults = new Dictionary<IMethodTestInfo, IMethodTestRunResult>();
            foreach (var test in tests)
            {
                testResults.Add(test, Execute(testMethod, test, cancellationToken));
            }
            return testResults;
        }

        private (IMethodTestInfo test, IMethodTestRunResult run, MethodInfo testMethod) TryCompile(string sourceCode)
        {
            var testMethod = MethodCompiler.Compile(sourceCode);
            var compilationTest = new MethodTestInfo() { IsCompilation = true };
            var testRun = new MethodTestRunResult()
            {
                TestRunStatus = testMethod == null ? TestRunStatus.CompilationFailed : TestRunStatus.Passed,
                Message = string.Join(Environment.NewLine, MethodCompiler.CompilationErrors)
            };
            return (compilationTest, testRun, testMethod);
        }

        private async Task<(IMethodTestInfo test, IMethodTestRunResult run, MethodInfo testMethod)> TryCompileAsync(string sourceCode, CancellationToken token)
        {
            var startTime = DateTime.Now;
            var testMethod = await MethodCompiler.CompileAsync(sourceCode, token);
            var compilationTest = new MethodTestInfo() { IsCompilation = true };
            var testRun = new MethodTestRunResult()
            {
                TestRunStatus = testMethod == null ? TestRunStatus.CompilationFailed : TestRunStatus.Passed,
                Message = string.Join(Environment.NewLine, MethodCompiler.CompilationErrors),
                StartTime = startTime,
                EndTime = DateTime.Now
            };
            return (compilationTest, testRun, testMethod);
        }

        public IDictionary<IMethodTestInfo, IMethodTestRunResult> Run(string sourceCode, IEnumerable<IMethodTestInfo> tests)
        {
            var testResults = new Dictionary<IMethodTestInfo, IMethodTestRunResult>();
            var compilationResult = TryCompile(sourceCode);
            testResults.Add(compilationResult.test, compilationResult.run);
            if (compilationResult.testMethod != null)
            {
                foreach (var testAndRun in ExecuteTests(tests, compilationResult.testMethod))
                {
                    testResults.Add(testAndRun.Key, testAndRun.Value);
                }
            }
            return testResults;
        }

        public async Task<IDictionary<IMethodTestInfo, IMethodTestRunResult>> RunAsync(string sourceCode, IEnumerable<IMethodTestInfo> tests, CancellationToken cancellationToken)
        {
            var testResults = new Dictionary<IMethodTestInfo, IMethodTestRunResult>();
            var compilationResult = await TryCompileAsync(sourceCode, cancellationToken);
            testResults.Add(compilationResult.test, compilationResult.run);
            if (compilationResult.testMethod != null)
            {
                var execTestResults = await Task.Run(() => ExecuteTests(tests, compilationResult.testMethod));
                foreach (var testAndRun in execTestResults)
                {
                    testResults.Add(testAndRun.Key, testAndRun.Value);
                }
            }
            return testResults;
        }
    }
}
