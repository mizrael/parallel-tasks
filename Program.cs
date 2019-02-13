using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace parallel_tasks
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await RunSerialAsync();

            Console.WriteLine();

            await RunParallelAsync();

            Console.WriteLine();
            Log("done, press a key to exit...", ConsoleColor.Yellow);
            Console.ReadLine();
        }

        private static Task RunSerialAsync()
        {
            return LogExecutionTimeAsync(async () =>
            {
                await LogExecutionTimeAsync(() => WaitAsync(1000), "first serial task", ConsoleColor.Magenta);
                await LogExecutionTimeAsync(() => WaitAsync(2000), "second serial task");
            }, "serial execution", ConsoleColor.Green);
        }

        private static Task RunParallelAsync()
        {
            return LogExecutionTimeAsync(() =>
            {
                return Task.WhenAll(LogExecutionTimeAsync(() => WaitAsync(1000), "first parallel task", ConsoleColor.Magenta),
                                    LogExecutionTimeAsync(() => WaitAsync(2000), "second parallel task"));
            }, "parallel execution", ConsoleColor.Green);
        }

        static Task LogExecutionTimeAsync(Func<Task> action, string title, ConsoleColor textColor = ConsoleColor.Gray){
            Log($"running {title}...", textColor);

            return MeasureAsync(action).ContinueWith(t =>{
                Log($"{title} took: {t.Result}", textColor);
            });
        }

        static void Log(string text, ConsoleColor textColor){
            var tmpCol = Console.ForegroundColor;
            Console.ForegroundColor = textColor;
            Console.WriteLine(text);
            Console.ForegroundColor = tmpCol;
        }

        static Task<TimeSpan> MeasureAsync(Func<Task> action){
            var timer = new Stopwatch();
            timer.Start();

            var t = action().ContinueWith(t1 =>{
                timer.Stop();
                return timer.Elapsed;
            });
            t.ConfigureAwait(false);
            return t;
        }

        static Task WaitAsync(int milliseconds){
            return Task.Run(() =>{
                System.Threading.Thread.Sleep(milliseconds);
            });
        }
    }
}
