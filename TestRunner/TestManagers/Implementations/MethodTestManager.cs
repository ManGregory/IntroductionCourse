using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestRunner.CommonTypes;
using TestRunner.CommonTypes.Implementations;
using TestRunner.CommonTypes.Interfaces;
using TestRunner.Providers.Interfaces;
using TestRunner.TestManagers.Interfaces;
using TestRunner.TestRunners.Interfaces;

namespace TestRunner.TestManagers.Implementations
{
    public class MethodTestManager<T> : IDisposable, IMethodTestManager<T> where T : class
    {
        private bool disposed = false;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public int Timeout { get; set; }
        public string SourceCode { get; set; }
        public bool IsTimedOut { get; set; }
        public Exception Exception { get; set; }
        public IMethodTestInfoProvider<T> TestInfoProvider { get; set; }
        public IMethodTestRunner MethodTestRunner { get; set; }

        public async Task<IDictionary<IMethodTestInfo, IMethodTestRunResult>> RunAsync()
        {
            try
            {
                cancellationTokenSource.CancelAfter(Timeout);
                var methodTests = await TestInfoProvider.GetMethodTestsAsync();
                var testRunTask = MethodTestRunner.RunAsync(SourceCode, methodTests, cancellationTokenSource.Token);
                if (await Task.WhenAny(testRunTask, Task.Delay(Timeout, cancellationTokenSource.Token)) == testRunTask)
                {
                    return await testRunTask;
                }
                else
                {
                    IsTimedOut = true;
                }
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is TimeoutException)
                {
                    IsTimedOut = true;
                }
                else
                {
                    Exception = ex;
                }
            }

            var dict = new Dictionary<IMethodTestInfo, IMethodTestRunResult>();
            if (IsTimedOut)
            {
                dict.Add(new MethodTestInfo(), new MethodTestRunResult() { TestRunStatus = TestRunStatus.Timeout });
            }
            if (Exception != null)
            {
                dict.Add(new MethodTestInfo(), new MethodTestRunResult() { TestRunStatus = TestRunStatus.UnknownException, Exception = Exception });
            }
            return dict;
        }

        public void Cancel()
        {
            cancellationTokenSource.Cancel();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                cancellationTokenSource.Dispose();
            }

            disposed = true;
        }
    }
}