using System;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var day = new Day3.Main();
            Console.WriteLine($"***** {day.DayName} *****");
            Console.WriteLine("\n***** Part 1 *****");
            Console.WriteLine(await day.Part1());

            Console.WriteLine("\n***** Part 2 *****");
            Console.WriteLine(await day.Part2());
        }
    }
}
