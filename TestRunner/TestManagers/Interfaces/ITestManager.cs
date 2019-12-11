using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TestRunner.CommonTypes.Interfaces;
using TestRunner.Providers.Interfaces;
using TestRunner.TestRunners.Interfaces;

namespace TestRunner.TestManagers.Interfaces
{
    public interface ITestManager<T> 
        where T : class
    {
        int Timeout { get; set; }
        string SourceCode { get; set; }
        bool IsTimedOut { get; set; }
        Exception Exception { get; set; }
        ITestInfoProvider<T> TestInfoProvider { get; set; }
        ITestRunner TestRunner { get; set; }
        Task<IDictionary<ITestInfo, ITestRunResult>> RunAsync();
        void Cancel();
    }
}
