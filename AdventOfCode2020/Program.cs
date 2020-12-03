using System;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var day = new Day1();
            Console.WriteLine($"***** {day.GetType().Name} *****");
            Console.WriteLine("***** Part 1 *****");
            Console.WriteLine(await day.Part1());

            Console.WriteLine("***** Part 2 *****");
            Console.WriteLine(await day.Part2());
        }
    }
}
