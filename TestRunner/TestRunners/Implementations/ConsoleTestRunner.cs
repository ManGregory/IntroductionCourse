using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TestRunner.CommonTypes.Interfaces;

namespace TestRunner.TestRunners.Implementations
{
    public class ConsoleTestRunner : BaseTestRunner
    {
        protected override string BuildMessage(ITestInfo test, ITestRunResult testRunResult)
        {
            throw new NotImplementedException();
        }

        protected override object Invoke(MethodInfo testMethod, ITestInfo test)
        {
            throw new NotImplementedException();
        }

        protected override bool IsTestPassed(ITestInfo test, ITestRunResult testRunResult)
        {
            throw new NotImplementedException();
        }
    }
}
