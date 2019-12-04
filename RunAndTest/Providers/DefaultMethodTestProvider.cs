using System;
using System.Collections.Generic;
using TestRunner.CommonTypes.Implementations;
using TestRunner.CommonTypes.Interfaces;

namespace RunAndTest.Providers
{
    public class DefaultMethodTestProvider
    {
        public IEnumerable<IMethodTestInfo> MethodTests 
        { 
            get 
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
            set => throw new NotImplementedException(); 
        }
    }
}