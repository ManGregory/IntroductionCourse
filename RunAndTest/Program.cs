using RunAndTest.Compilers;
using RunAndTest.Providers;
using RunAndTest.TestRunners;
using System;

namespace RunAndTest
{

    class Program
    {
        public static void Main(string[] args)
        {
            const string filePath = @"D:\Programming\VSProjects\IntroductionC#\Example\HappyTicket.cs";
            ISourceCodeProvider sourceCodeProvider = new FileSourceCodeProvider { FilePath = filePath };
            IMethodCompiler methodCompiler = new RoslynMethodCompiler
            {
                EntryMethod = "IsTicketHappy",
                EntryType = "Lecture1.Program",
                SourceCodeProvider = sourceCodeProvider
            };
            IMethodTestRunner methodTestManager = new MethodTestRunner
            {
                MethodTestProvider = new DefaultMethodTestProvider(),
                MethodCompiler = methodCompiler
            };

            var results = methodTestManager.Run();
            
            foreach (var result in results)
            {
                Console.WriteLine(result);
            }

            Console.ReadKey();
        }
    }
}