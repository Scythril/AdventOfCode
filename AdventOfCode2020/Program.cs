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
            var day = new Day3.Main();
            var stopwatch = new Stopwatch();

            stopwatch.Start();            
            Console.WriteLine($"******************************  {day.DayName}  ******************************");
            Console.WriteLine("\n****************************** Part 1 ******************************");
            Console.WriteLine(await day.Part1());
            Console.WriteLine($"Part 1 took: {stopwatch.Elapsed}");

            stopwatch.Restart();
            Console.WriteLine("\n****************************** Part 2 ******************************");
            Console.WriteLine(await day.Part2());
            Console.WriteLine($"Part 2 took: {stopwatch.Elapsed}");
            Console.WriteLine($"End time: {DateTimeOffset.Now:O}");
        }
    }
}
