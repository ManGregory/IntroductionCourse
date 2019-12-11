using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestRunner.CommonTypes.Implementations;
using TestRunner.CommonTypes.Interfaces;
using TestRunner.Providers.Interfaces;

namespace RunAndTest.Providers
{
    public class DefaultMethodTestProvider : ITestInfoProvider<MethodTestInfo>
    {
        public Func<MethodTestInfo, ITestInfo> ConvertFunction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<IEnumerable<ITestInfo>> GetMethodTestsAsync()
        {
            return new MethodTestInfo[]
            {
                //new MethodTestInfo() { Name = "1", InputParameters = new object[] { "123456" }, ExpectedResult = false },
                new MethodTestInfo() { Name = "Def 1, Test 1", InputParameters = new object[] { 123123 }, ExpectedResult = true },
                new MethodTestInfo() { Name = "Def 1, Test 2", InputParameters = new object[] { 678852 }, ExpectedResult = false },
                new MethodTestInfo() { Name = "Def 1, Test 3", InputParameters = new object[] { 31548 }, ExpectedResult = false },
                new MethodTestInfo() { Name = "Def 1, Test 4", InputParameters = new object[] { 909090 }, ExpectedResult = false }
            };
        }
    }

    public class DefaultMethodTestProvider2 : ITestInfoProvider<MethodTestInfo>
    {
        public Func<MethodTestInfo, ITestInfo> ConvertFunction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<IEnumerable<ITestInfo>> GetMethodTestsAsync()
        {
            return new MethodTestInfo[]
            {
                new MethodTestInfo() { Name = "Def 2, Test 1", InputParameters = new object[] { 888888 }, ExpectedResult = true },
                new MethodTestInfo() { Name = "Def 2, Test 2", InputParameters = new object[] { 777777 }, ExpectedResult = false },
                new MethodTestInfo() { Name = "Def 2, Test 3", InputParameters = new object[] { 111111 }, ExpectedResult = false },
                new MethodTestInfo() { Name = "Def 2, Test 4", InputParameters = new object[] { 555555 }, ExpectedResult = false }
            };
        }
    }

    public class DefaultMethodTestProvider3 : ITestInfoProvider<MethodTestInfo>
    {
        public Func<MethodTestInfo, ITestInfo> ConvertFunction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<IEnumerable<ITestInfo>> GetMethodTestsAsync()
        {
            return new MethodTestInfo[]
            {
                new MethodTestInfo() { Name = "Def 3, Test 1", InputParameters = new object[] { 987654 }, ExpectedResult = true },
                new MethodTestInfo() { Name = "Def 3, Test 2", InputParameters = new object[] { 456789 }, ExpectedResult = false },
                new MethodTestInfo() { Name = "Def 3, Test 3", InputParameters = new object[] { 015245 }, ExpectedResult = false },
                new MethodTestInfo() { Name = "Def 3, Test 4", InputParameters = new object[] { 985412 }, ExpectedResult = false }
            };
        }
    }
}