using RunAndTest.Compilers;
using RunAndTest.Providers;
using RunAndTest.TestRunners;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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

            using (var cancellationToken = new CancellationTokenSource())
            {
                try
                {
                    cancellationToken.CancelAfter(timeout);
                    RunTests(methodTestRunner, cancellationToken).Wait();
                }
                catch (AggregateException ex)
                {
                    if (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is TimeoutException)
                    {
                        Console.WriteLine("Timeout");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception", ex);
                }
            }

            Console.ReadKey();
        }

        private static async Task RunTests(IMethodTestRunner methodTestRunner, CancellationTokenSource cancellationSource)
        {
            Console.WriteLine("Running");
            string sourceCode = File.ReadAllText(filePath);
            var testRunTask = methodTestRunner.RunAsync(sourceCode, new DefaultMethodTestProvider().MethodTests, cancellationSource.Token);

            if (await Task.WhenAny(testRunTask, Task.Delay(timeout, cancellationSource.Token)) == testRunTask)
            {
                await testRunTask;
                foreach (var result in testRunTask.Result)
                {
                    Console.WriteLine(result);
                }
            }
            else
            {
                throw new TimeoutException("Timeout");
            }
        }
    }
}