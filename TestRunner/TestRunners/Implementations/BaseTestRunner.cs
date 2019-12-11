using System;
using System.Collections.Generic;
using System.IO;
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
    public abstract class BaseTestRunner : ITestRunner
    {
        public IMethodCompiler MethodCompiler { get; set; }

        #region Abstract methods
        protected abstract string BuildMessage(ITestInfo test, ITestRunResult testRunResult);        
        protected abstract bool IsTestPassed(ITestInfo test, ITestRunResult testRunResult);
        protected abstract object Invoke(MethodInfo testMethod, ITestInfo test);
        #endregion

        private ITestRunResult Execute(MethodInfo testMethod, ITestInfo test, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();            
            var testRunResult = TryInvoke(testMethod, test);
            if (testRunResult.TestRunStatus != TestRunStatus.TargetException)
            {
                testRunResult.TestRunStatus = IsTestPassed(test, testRunResult) ? TestRunStatus.Passed : TestRunStatus.Failed;
            }
            testRunResult.Message = BuildMessage(test, testRunResult);
            return testRunResult;
        }

        private ITestRunResult TryInvoke(MethodInfo testMethod, ITestInfo test)
        {
            var result = new TestRunResult();
            try
            {
                result.StartTime = DateTime.Now;
                result.ActualResult = Invoke(testMethod, test);
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

        private IDictionary<ITestInfo, ITestRunResult> ExecuteTests(IEnumerable<ITestInfo> tests, MethodInfo testMethod, CancellationToken cancellationToken = default)
        {
            var testResults = new Dictionary<ITestInfo, ITestRunResult>();
            foreach (var test in tests)
            {
                testResults.Add(test, Execute(testMethod, test, cancellationToken));
            }
            return testResults;
        }


        private async Task<(ITestInfo test, ITestRunResult run, MethodInfo testMethod)> TryCompileAsync(string sourceCode, CancellationToken token)
        {
            var startTime = DateTime.Now;
            var testMethod = await MethodCompiler.CompileAsync(sourceCode, token);
            ITestInfo compilationTest = new MethodTestInfo() { IsCompilation = true };
            var testRun = new TestRunResult()
            {
                TestRunStatus = testMethod == null ? TestRunStatus.CompilationFailed : TestRunStatus.Passed,
                Message = string.Join(Environment.NewLine, MethodCompiler.CompilationErrors),
                StartTime = startTime,
                EndTime = DateTime.Now
            };
            return (compilationTest, testRun, testMethod);
        }

        public async Task<IDictionary<ITestInfo, ITestRunResult>> RunAsync(string sourceCode, IEnumerable<ITestInfo> tests, CancellationToken cancellationToken)
        {
            var testResults = new Dictionary<ITestInfo, ITestRunResult>();
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