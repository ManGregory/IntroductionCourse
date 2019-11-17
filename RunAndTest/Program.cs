using RunAndTest.Compilers;
using RunAndTest.Providers;
using RunAndTest.TestRunners;
using RunAndTest.Utils;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            IMethodTestRunner methodTestRunner = new MethodTestRunner
            {
                MethodTestProvider = new DefaultMethodTestProvider(),
                MethodCompiler = methodCompiler
            };

            using (var cancellationToken = new CancellationTokenSource())
            {
                try
                {
                    cancellationToken.CancelAfter(3000);
                    RunTests(methodTestRunner, cancellationToken).Wait();
                }
                catch (AggregateException ex)
                {
                    if (cancellationToken.Token.IsCancellationRequested)
                    {
                        Console.WriteLine("\rTimeout");
                    }                 
                }
            }

            Console.ReadKey();
        }

        private static async Task RunTests(IMethodTestRunner methodTestRunner, CancellationTokenSource cancellationSource)
        {
            Console.WriteLine("Running");
            var testRunTask = methodTestRunner.RunAsync(cancellationSource.Token);

            string symbol = "-";
            while (!testRunTask.IsCompleted)
            {
                Console.Write("\r" + symbol, 0);
                if (symbol == "-") symbol = @"\";
                else if (symbol == @"\") symbol = @"|";
                else if (symbol == @"|") symbol = @"/";
                else if (symbol == @"/") symbol = @"-";
                Thread.Sleep(100);
            }

            foreach (var result in testRunTask.Result)
            {
                Console.WriteLine(result);
            }
        }
    }
}