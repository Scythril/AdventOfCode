using System;
using System.Collections.Generic;

namespace AdventOfCode2017
{
    public class Day17
    {
        public int Part1(int input)
        {
            var steps = 2017;
            var numbers = new List<int>(steps + 1) { 0 };
            var curPos = 0;
            for (var i = 1; i <= steps; i++)
            {
                curPos = ((curPos + input) % i) + 1;
                numbers.Insert(curPos, i);
            }
            return numbers[curPos + 1];
        }

        public int Part2(int input)
        {
            var steps = 50000000;
            var curPos = 0;
            var number = 0;
            for (var i = 1; i <= steps; i++)
            {
                curPos = ((curPos + input) % i) + 1;
                if (curPos == 1)
                    number = i;
            }
            return number;
        }

        public static void Run()
        {
            var day17 = new Day17();
            Console.WriteLine("\n###############\n###############\nDay 17\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(day17.Part1(3));
            Console.WriteLine(day17.Part1(382));

            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(day17.Part2(382));
        }
    }
}
