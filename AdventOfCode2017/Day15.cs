using System;
using System.Collections.Generic;

namespace AdventOfCode2017
{
    public class Day15
    {
        public int Part1(int startA, int startB)
        {
            var matchCount = 0;
            var pairCount = 40000000;
            long prevA = startA;
            long prevB = startB;
            for (int i = 0; i < pairCount; i++)
            {
                prevA = (prevA * 16807) % 2147483647;
                prevB = (prevB * 48271) % 2147483647;
                var binA = Convert.ToString(prevA, 2).PadLeft(32, '0');
                var binB = Convert.ToString(prevB, 2).PadLeft(32, '0');
                if (binA.Substring(16) == binB.Substring(16))
                    matchCount++;
            }

            return matchCount;
        }

        public int Part2(int startA, int startB)
        {
            var matchCount = 0;
            var pairCount = 5000000;
            long prevA = startA;
            long prevB = startB;
            var valuesA = new List<long>();
            var valuesB = new List<long>();
            while (valuesA.Count < pairCount)
            {
                prevA = (prevA * 16807) % 2147483647;
                if (prevA % 4 == 0)
                    valuesA.Add(prevA);
            }
            while (valuesB.Count < pairCount)
            {
                prevB = (prevB * 48271) % 2147483647;
                if (prevB % 8 == 0)
                    valuesB.Add(prevB);
            }

            for (int i = 0; i < pairCount; i++)
            {
                var binA = Convert.ToString(valuesA[i], 2).PadLeft(32, '0');
                var binB = Convert.ToString(valuesB[i], 2).PadLeft(32, '0');
                if (binA.Substring(16) == binB.Substring(16))
                    matchCount++;
            }

            return matchCount;
        }

        public static void Run()
        {
            var day15 = new Day15();
            Console.WriteLine("\n###############\n###############\nDay 15\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(day15.Part1(65, 8921));
            Console.WriteLine(day15.Part1(703, 516));

            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(day15.Part2(65, 8921));
            Console.WriteLine(day15.Part2(703, 516));
        }
    }
}
