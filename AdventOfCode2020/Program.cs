using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Start time: {DateTimeOffset.Now:O}");
            var day = new Day8.Main { Debug = true };
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"******************************  {day.DayName}  ******************************");
            Console.WriteLine("\n****************************** Part 1 ******************************");
            Console.ResetColor();
            Console.WriteLine(await day.Part1());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Part 1 took: {stopwatch.Elapsed}");

            stopwatch.Restart();
            Console.WriteLine("\n****************************** Part 2 ******************************");
            Console.ResetColor();
            Console.WriteLine(await day.Part2());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Part 2 took: {stopwatch.Elapsed}");
            Console.WriteLine($"End time: {DateTimeOffset.Now:O}");
            Console.ResetColor();
        }
    }
}
