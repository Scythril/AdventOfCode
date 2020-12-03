using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017
{
    public class Day6
    {
        public int Part1(String input)
        {
            var banks = input.Split('\t', StringSplitOptions.RemoveEmptyEntries).Select(x => Int32.Parse(x)).ToList();
            var seenConfigs = new HashSet<String>
            {
                String.Join('\t', banks)
            };

            while (true)
            {
                var index = 0;
                var blocks = 0;
                for (int i = 0; i < banks.Count; i++)
                {
                    if (banks[i] > blocks)
                    {
                        blocks = banks[i];
                        index = i;
                    }
                }
                banks[index] = 0;
                index = (index == banks.Count - 1) ? 0 : index + 1;
                for (; blocks > 0; index = (index == banks.Count - 1) ? 0 : index + 1)
                {
                    banks[index]++;
                    blocks--;
                }
                var config = String.Join('\t', banks);
                if (seenConfigs.Contains(config))
                    break;
                seenConfigs.Add(config);
            }
            return seenConfigs.Count;
        }

        public int Part2(String input)
        {
            var banks = input.Split('\t', StringSplitOptions.RemoveEmptyEntries).Select(x => Int32.Parse(x)).ToList();
            var seenConfigs = new Dictionary<String, int>
            {
                { String.Join('\t', banks), 0 }
            };

            var configIndex = 1;
            var config = "";
            while (true)
            {
                var index = 0;
                var blocks = 0;
                for (int i = 0; i < banks.Count; i++)
                {
                    if (banks[i] > blocks)
                    {
                        blocks = banks[i];
                        index = i;
                    }
                }
                banks[index] = 0;
                index = (index == banks.Count - 1) ? 0 : index + 1;
                for (; blocks > 0; index = (index == banks.Count - 1) ? 0 : index + 1)
                {
                    banks[index]++;
                    blocks--;
                }
                config = String.Join('\t', banks);
                if (seenConfigs.ContainsKey(config))
                    break;
                seenConfigs.Add(config, configIndex++);
            }
            return seenConfigs.Count - seenConfigs[config];
        }

        public static void Run()
        {
            var input = @"4	10	4	1	8	4	9	14	5	1	14	15	0	15	3	5";
            var day6 = new Day6();
            Console.WriteLine("\n###############\n###############\nDay 6\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(day6.Part1("0\t2\t7\t0"));
            Console.WriteLine(day6.Part1(input));

            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(day6.Part2("0\t2\t7\t0"));
            Console.WriteLine(day6.Part2(input));
        }
    }
}
