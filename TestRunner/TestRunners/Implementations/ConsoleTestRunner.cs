using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TestRunner.CommonTypes;
using TestRunner.CommonTypes.Implementations;
using TestRunner.CommonTypes.Interfaces;

namespace TestRunner.TestRunners.Implementations
{
    public class ConsoleTestRunner : BaseTestRunner
    {
        private void RestoreConsole()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            var standartInput = new StreamReader(Console.OpenStandardInput());
            Console.SetIn(standartInput);
        }

        private string BuildConsoleInput(IConsoleTestInfo test)
        {
            return string.Join(Environment.NewLine, test.ConsoleTest.ConsoleSteps.SelectMany(step => step.Input));
        }

        protected override object Invoke(MethodInfo testMethod, ITestInfo test)
        {
            IConsoleTestInfo consoleTestInfo = (IConsoleTestInfo) test;
            var consoleOutput = new StringBuilder();
            string consoleInput = BuildConsoleInput(consoleTestInfo);
            string result = string.Empty;
            using (StringWriter writer = new StringWriter(consoleOutput))
            {
                using (StringReader reader = new StringReader(consoleInput))
                {
                    Console.SetOut(writer);
                    Console.SetIn(reader);
                    testMethod.Invoke(null, consoleTestInfo.ConsoleTest.MethodInputParameteres);
                    result = consoleOutput.ToString();
                }
            }
            RestoreConsole();
            return result;
        }        

        protected override bool IsTestPassed(ITestInfo test, ITestRunResult testRunResult)
        {
            IConsoleTestInfo consoleTestInfo = (IConsoleTestInfo) test;
            IEnumerable<string> allActualOuput = testRunResult.ActualResult?.ToString().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries) 
                ?? new string[0];
            var actualOutputStack = new Stack<string>(allActualOuput
                .Where(output => !output.StartsWith("*") && !string.IsNullOrWhiteSpace(output.Trim()))
                .Reverse());

            var stepResults = consoleTestInfo.ConsoleTest.ConsoleSteps.Select(step => CheckConsoleStep(step, actualOutputStack)).ToList();
            testRunResult.ActualResult = stepResults;
            return stepResults.All(stepResult => stepResult.IsPassed);
        }

        private ConsoleStepResult CheckConsoleStep(ConsoleStep step, Stack<string> actualOutputStack)
        {
            var actual = new List<string>();
            bool isPassed = true;
            foreach (var expected in step.Output)
            {
                if (actualOutputStack.Count == 0)
                {
                    isPassed = false;
                    break;
                }
                var actualOutput = actualOutputStack.Pop();
                actual.Add(actualOutput);
                if (actualOutput != expected)
                {
                    isPassed = false;
                }
            }
            return new ConsoleStepResult()
            {
                Name = step.Name,
                Expected = step.Output,
                Actual = actual,
                IsPassed = isPassed
            };
        }

        protected override string BuildMessage(ITestInfo test, ITestRunResult testRunResult)
        {
            var stepResults = testRunResult.ActualResult as IEnumerable<ConsoleStepResult>;

            var sb = new StringBuilder();
            bool isPassed = testRunResult.TestRunStatus == TestRunStatus.Passed;
            sb.AppendLine($"Test {test.Name} {(isPassed ? "Passed" : "Failed")}");
            foreach (var stepResult in stepResults)
            {
                sb.AppendLine($"{stepResult.Name} is {(stepResult.IsPassed ? "passed" : "failed")}");
                sb.AppendLine($"Expected: {string.Join(",", stepResult.Expected)}");
                sb.AppendLine($"Actual: {string.Join(",", stepResult.Actual)}");
            }
            if (testRunResult.TestRunStatus == TestRunStatus.TargetException) sb.AppendLine($"There was an exception during executing of the test");

            return sb.ToString();
        }
    }
}
