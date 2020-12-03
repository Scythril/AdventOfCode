using System;
using System.Diagnostics;

namespace AdventOfCode2019
{
    class Program
    {
        static void Main(string[] args)
        {
            var day = new Day14();
            Console.WriteLine(day.GetType().FullName);
            var divider = new string('#', 80);
            Console.WriteLine(divider);

            Console.WriteLine("Part 1");
            var sw = new Stopwatch();
            sw.Start();
            Console.WriteLine(day.Part1());
            sw.Stop();
            Console.WriteLine($"Elapsed time: {sw.Elapsed}\n{sw.ElapsedMilliseconds} ms\n{sw.ElapsedTicks} ticks");
            Console.WriteLine(divider);

            Console.WriteLine("Part 2");
            sw.Restart();
            Console.WriteLine(day.Part2());
            sw.Stop();
            Console.WriteLine($"Elapsed time: {sw.Elapsed}\n{sw.ElapsedMilliseconds} ms\n{sw.ElapsedTicks} ticks");
            Console.WriteLine(divider);
        }
    }
}
