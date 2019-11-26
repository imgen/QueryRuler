using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace QueryRuler
{
    public static class TinyProfiler
    {
        public static T Profile<T>(string operationName, Func<T> func, Action<string> printer = null, Action<TimeSpan> elapsedTimeProcessor = null)
        {
            var watch = new Stopwatch();
            watch.Start();
            try
            {
                var result = func();
                PrintProfileMessage(watch, operationName, printer);
                elapsedTimeProcessor?.Invoke(watch.Elapsed);
                return result;
            }
            catch
            {
                PrintProfileMessage(watch, operationName, printer);
                throw;
            }
        }

        public static void Profile(string operationName, Action action, 
            Action<string> printer = null, 
            Action<TimeSpan> elapsedTimeProcessor = null)
        {
            Profile<object>(operationName, () =>
            {
                action();
                return null;
            }, printer, elapsedTimeProcessor);
        }

        public static async Task<T> ProfileAsync<T>(string operationName, 
            Func<Task<T>> func,
            Action<string> printer = null,
            Action<TimeSpan> elapsedTimeProcessor = null)
        {
            var watch = new Stopwatch();
            watch.Start();
            try
            {
                var result = await func();
                PrintProfileMessage(watch, operationName, printer);
                elapsedTimeProcessor?.Invoke(watch.Elapsed);
                return result;
            }
            catch
            {
                PrintProfileMessage(watch, operationName, printer);
                throw;
            }
        }

        public static async Task ProfileAsync(string operationName, 
            Func<Task> func,
            Action<string> printer = null,
            Action<TimeSpan> elapsedTimeProcessor = null)
        {
            await ProfileAsync<object>(operationName,
                async () =>
                {
                    await func();
                    return null;
                }, printer, elapsedTimeProcessor);
        }

        private static void PrintProfileMessage(Stopwatch watch, string operationName, Action<string> printer = null)
        {
            watch.Stop();
            printer ??= Console.WriteLine;
            var oldForeColor = Console.ForegroundColor;
            var isConsoleWriteLine = printer == Console.WriteLine;
            var duration = FormatTimeSpan(watch.Elapsed);
            var message = $"\tThe operation {operationName} took {duration}";
            var newLine = Environment.NewLine;
            if (isConsoleWriteLine)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            printer(newLine + newLine + "PROFILING MESSAGE START" + newLine + message + newLine + "PROFILING MESSAGE END" + newLine + newLine);
            if (isConsoleWriteLine)
            {
                Console.ForegroundColor = oldForeColor;
            }
        }

        public static string FormatTimeSpan(TimeSpan timeSpan)
        {
            var (minutes, seconds, milliseconds, microseconds) = (
                (int)timeSpan.TotalMinutes,
                timeSpan.Seconds,
                timeSpan.Milliseconds,
                int.Parse(timeSpan.ToString("ffffff").Substring(3))
            );

            return $"{minutes}m {seconds}s {milliseconds}ms {microseconds}μs";
        }
    }
}
