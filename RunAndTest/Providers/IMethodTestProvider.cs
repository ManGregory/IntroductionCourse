using RunAndTest.DTO;
using System.Collections.Generic;

namespace RunAndTest.Providers
{
    public interface IMethodTestProvider
    {
        IEnumerable<IMethodTestInfo> MethodTests { get; set; }
    }
}