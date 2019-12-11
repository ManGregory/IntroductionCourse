﻿using Newtonsoft.Json.Linq;
using RunAndTest.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestRunner.CommonTypes.Implementations;
using TestRunner.CommonTypes.Interfaces;
using TestRunner.Compilers.Implementations;
using TestRunner.Compilers.Interfaces;
using TestRunner.TestManagers.Implementations;
using TestRunner.TestManagers.Interfaces;
using TestRunner.TestRunners.Implementations;
using TestRunner.TestRunners.Interfaces;

namespace RunAndTest
{

    class Program
    {
        private static int timeout = 100000;
        private static string filePath = @"D:\Programming\VSProjects\IntroductionC#\Example\HappyTicket.cs";

        public static void Main(string[] args)
        {
            var testManager1 = CreateTestManager();
            var testManager2 = CreateTestManager();
            var testManager3 = CreateTestManager();
            testManager2.TestInfoProvider = new DefaultMethodTestProvider2();
            testManager3.TestInfoProvider = new DefaultMethodTestProvider3();

            /*var sb = new StringBuilder();
            string s = "123";
            using (StringWriter writer = new StringWriter(sb))
            {
                using (StringReader reader = new StringReader(s))
                {
                    Console.SetOut(writer);
                    Console.SetIn(reader);*/
            //Parallel.ForEach(new[] { testManager1, testManager2, testManager3 }, (t) => Run(t));
            Run(testManager1);
            //}
            /*var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);*/
            //}                

            //var result = Convert1(@"{types: ['int', 'int'], values: ['5', '5']}");

            TextReader reader = new StreamReader(Console.OpenStandardInput());
            Console.SetIn(reader);
            Console.ReadKey();
        }

        private static ITestManager<MethodTestInfo> CreateTestManager()
        {
            IMethodCompiler methodCompiler = new RoslynMethodCompiler
            {
                EntryMethod = "IsTicketHappy",
                EntryType = "Lecture1.Program",
            };
            ITestRunner methodTestRunner = new MethodTestRunner
            {
                MethodCompiler = methodCompiler
            };

            var testManager = new TestManager<MethodTestInfo>()
            {
                Timeout = timeout,
                TestInfoProvider = new DefaultMethodTestProvider(),
                SourceCode = File.ReadAllText(filePath),
                TestRunner = methodTestRunner
            };
            return testManager;
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

        private static void Run<T>(ITestManager<T> testManager)
            where T : class
        {
            var task = testManager.RunAsync();
            /*Console.WriteLine("Press space key to cancel");
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Spacebar)
            {
                testManager.Cancel();
            }
            else
            {*/
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
                        var standardOutput = new StreamWriter(Console.OpenStandardOutput());
                        standardOutput.AutoFlush = true;
                        Console.SetOut(standardOutput);

                        Console.WriteLine(result.Value.Message);
                    }
                }
            //}
        }
    }
}