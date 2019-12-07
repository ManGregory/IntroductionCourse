using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestRunner.CommonTypes.Implementations;
using TestRunner.CommonTypes.Interfaces;
using TestRunner.Providers.Interfaces;

namespace RunAndTest.Providers
{
    public class DefaultMethodTestProvider : IMethodTestInfoProvider<MethodTestInfo>
    {
        public Func<MethodTestInfo, IMethodTestInfo> ConvertFunction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<IEnumerable<IMethodTestInfo>> GetMethodTestsAsync()
        {
            return new MethodTestInfo[]
            {
                new MethodTestInfo() { Name = "1", InputParameters = new object[] { "123456" }, ExpectedResult = false },
                new MethodTestInfo() { Name = "1", InputParameters = new object[] { 123123 }, ExpectedResult = true },
                new MethodTestInfo() { Name = "1", InputParameters = new object[] { 678852 }, ExpectedResult = false },
                new MethodTestInfo() { Name = "1", InputParameters = new object[] { 31548 }, ExpectedResult = false },
                new MethodTestInfo() { Name = "1", InputParameters = new object[] { 909090 }, ExpectedResult = false }
            };
        }
    }
}