using Newtonsoft.Json.Linq;
using RunAndTest.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            /*IMethodCompiler methodCompiler = new RoslynMethodCompiler
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
            Run(testManager);*/

            var result = Convert1(@"{types: ['int', 'int'], values: ['5', '5']}");

            Console.ReadKey();
        }

        private static object[] Convert1(string param)
        {
            var json = JObject.Parse(param);
            var result = new List<object>();
            var types = json["types"].Values().ToArray();
            var values = json["values"].ToArray();
            for (int i = 0; i < types.Length; i++)
            {
                string type = types[i].Value<string>();
                if (type == "int")
                {
                    result.Add(Convert.ToInt32(values[i].Value<string>()));
                }
                else if (type == "bool")
                {
                    result.Add(Convert.ToBoolean(values[i].Value<string>()));
                }
            }
            return result.ToArray();
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