using RunAndTest.Providers;
using System;
using System.IO;
using TestRunner.CommonTypes.Implementations;
using TestRunner.Compilers.Implementations;
using TestRunner.Compilers.Interfaces;
using TestRunner.TestManagers.Implementations;
using TestRunner.TestRunners.Implementations;
using TestRunner.TestRunners.Interfaces;

namespace RunAndTest
{

    class Program
    {
        private static int timeout = 1000;
        private static string filePath = @"D:\Programming\VSProjects\IntroductionC#\Example\HappyTicket.cs";

        public static void Main(string[] args)
        {
            IMethodCompiler methodCompiler = new RoslynMethodCompiler
            {
                EntryMethod = "IsTicketHappy",
                EntryType = "Lecture1.Program",
            };
            IMethodTestRunner methodTestRunner = new MethodTestRunner
            {
                MethodCompiler = methodCompiler
            };

            var testManager = new MethodTestManager<MethodTestInfo>()
            {
                Timeout = timeout,
                TestInfoProvider = new DefaultMethodTestProvider(),
                SourceCode = File.ReadAllText(filePath),
                MethodTestRunner = methodTestRunner
            };
            Run(testManager);

            Console.ReadKey();
        }

        private static void Run(MethodTestManager<MethodTestInfo> testManager)
        {
            var task = testManager.RunAsync();
            Console.WriteLine("Press space key to cancel");
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Spacebar)
            {
                testManager.Cancel();
            }
            else
            {
                if (testManager.IsTimedOut)
                {
                    Console.WriteLine("Timeout");
                }
                else if (testManager.Exception != null)
                {
                    Console.WriteLine(testManager.Exception);
                }
                else
                {
                    foreach (var result in task.Result)
                    {
                        Console.WriteLine(result.Value.Message);
                    }
                }
            }
        }
    }
}