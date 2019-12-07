using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestRunner.CommonTypes.Interfaces;

namespace TestRunner.Providers.Interfaces
{
    public interface IMethodTestInfoProvider<T> where T : class
    {
        Func<T, IMethodTestInfo> ConvertFunction { get; set; }
        Task<IEnumerable<IMethodTestInfo>> GetMethodTestsAsync();
    }
}
