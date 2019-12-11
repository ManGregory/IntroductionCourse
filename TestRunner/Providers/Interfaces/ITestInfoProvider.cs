using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestRunner.CommonTypes.Interfaces;

namespace TestRunner.Providers.Interfaces
{
    public interface ITestInfoProvider<T> 
        where T : class
    {
        Func<T, ITestInfo> ConvertFunction { get; set; }
        Task<IEnumerable<ITestInfo>> GetMethodTestsAsync();
    }
}
