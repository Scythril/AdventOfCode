using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017
{
    public class Day13
    {
        public int Part1(String input)
        {
            var layers = new Dictionary<int, int>();
            foreach (var line in input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var elements = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
                layers.Add(Int32.Parse(elements[0]), Int32.Parse(elements[1]));
            }
            var length = layers.Keys.Max();
            var severity = 0;
            for (var pos = 0; pos <= length; pos++)
            {
                if (!layers.ContainsKey(pos))
                    continue;
                if (pos % (2 * layers[pos] - 2) == 0)
                    severity += layers[pos] * pos;
            }
            return severity;
        }

        public int Part2(String input)
        {
            var layers = new Dictionary<int, int>();
            foreach (var line in input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var elements = line.Split(':', StringSplitOptions.RemoveEmptyEntries);
                layers.Add(Int32.Parse(elements[0]), Int32.Parse(elements[1]));
            }
            var length = layers.Keys.Max();
            for (var delay = 0; delay < Int32.MaxValue; delay++)
            {                
                var caught = false;
                for (var pos = 0; pos <= length && !caught; pos++)
                {
                    if (!layers.ContainsKey(pos))
                        continue;
                    if ((pos + delay) % (2 * layers[pos] - 2) == 0)
                        caught = true;
                }
                if (!caught)
                    return delay;
            }

            throw new NotFiniteNumberException();
        }

        public static void Run()
        {
            var day13 = new Day13();
            Console.WriteLine("\n###############\n###############\nDay 13\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(day13.Part1(@"0: 3
1: 2
4: 4
6: 4"));
            Console.WriteLine(day13.Part1(Input));

            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(day13.Part2(@"0: 3
1: 2
4: 4
6: 4"));
            Console.WriteLine(day13.Part2(Input));
        }

        private static String Input
        {
            get
            {
                return @"0: 4
1: 2
2: 3
4: 4
6: 8
8: 5
10: 8
12: 6
14: 6
16: 8
18: 6
20: 6
22: 12
24: 12
26: 10
28: 8
30: 12
32: 8
34: 12
36: 9
38: 12
40: 8
42: 12
44: 17
46: 14
48: 12
50: 10
52: 20
54: 12
56: 14
58: 14
60: 14
62: 12
64: 14
66: 14
68: 14
70: 14
72: 12
74: 14
76: 14
80: 14
84: 18
88: 14";
            }
        }
    }
}
