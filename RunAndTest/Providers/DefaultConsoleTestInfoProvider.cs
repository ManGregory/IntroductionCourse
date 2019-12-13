using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestRunner.CommonTypes.Implementations;
using TestRunner.CommonTypes.Interfaces;
using TestRunner.Providers.Interfaces;

namespace RunAndTest.Providers
{
    class DefaultConsoleTestInfoProvider : ITestInfoProvider<ConsoleTestInfo>
    {
        public Func<ConsoleTestInfo, ITestInfo> ConvertFunction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<IEnumerable<ITestInfo>> GetMethodTestsAsync()
        {
            return new ConsoleTestInfo[]
            {
                new ConsoleTestInfo()
                {
                    Id = 1,
                    Name = "Test 1",
                    ConsoleTest = new ConsoleTest()
                    {
                        MethodInputParameteres = new object[] {"123" },
                        ConsoleSteps = new ConsoleStep[]
                        {
                            new ConsoleStep()
                            {
                                Name = "Step 1 - Enter 2 number",
                                Input = new string[] { "1", "2" },
                                Output = new string[] { "3", "9" }
                            },
                            new ConsoleStep()
                            {
                                Name = "Step 2 - Enter Your Name",
                                Input = new string[] { "Vasya" },
                                Output = new string[] { "Hello, Vasya" }
                            }
                        }
                    }
                }
            };
        }
    }
}
