using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day16
{
    class Day16
    {
        public Dictionary<string, Action<Dictionary<int, int>, int, int, int>> Instructions = new Dictionary<string, Action<Dictionary<int, int>, int, int, int>>
        {
            { "addr", (registers, param1, param2, param3) => { registers[param3] = registers[param1] + registers[param2]; } },
            { "addi", (registers, param1, param2, param3) => { registers[param3] = registers[param1] + param2; } },
            { "mulr", (registers, param1, param2, param3) => { registers[param3] = registers[param1] * registers[param2]; } },
            { "muli", (registers, param1, param2, param3) => { registers[param3] = registers[param1] * param2; } },
            { "banr", (registers, param1, param2, param3) => { registers[param3] = registers[param1] & registers[param2]; } },
            { "bani", (registers, param1, param2, param3) => { registers[param3] = registers[param1] & param2; } },
            { "borr", (registers, param1, param2, param3) => { registers[param3] = registers[param1] | registers[param2]; } },
            { "bori", (registers, param1, param2, param3) => { registers[param3] = registers[param1] | param2; } },
            { "setr", (registers, param1, param2, param3) => { registers[param3] = registers[param1]; } },
            { "seti", (registers, param1, param2, param3) => { registers[param3] = param1; } },
            { "gtir", (registers, param1, param2, param3) => { registers[param3] = param1 > registers[param2] ? 1 : 0; } },
            { "gtri", (registers, param1, param2, param3) => { registers[param3] = registers[param1] > param2 ? 1 : 0; } },
            { "gtrr", (registers, param1, param2, param3) => { registers[param3] = registers[param1] > registers[param2] ? 1 : 0; } },
            { "eqir", (registers, param1, param2, param3) => { registers[param3] = param1 == registers[param2] ? 1 : 0; } },
            { "eqri", (registers, param1, param2, param3) => { registers[param3] = registers[param1] == param2 ? 1 : 0; } },
            { "eqrr", (registers, param1, param2, param3) => { registers[param3] = registers[param1] == registers[param2] ? 1 : 0; } },
        };

        public int Part1(string input)
        {
            var samplesWithThreeOrMore = 0;
            var samples = GetSamples(input);            
            foreach (var sample in samples)
            {
                var opCodes = GetPossibleOpCodes(sample);
                if (opCodes.Count >= 3)
                    samplesWithThreeOrMore++;
            }

            return samplesWithThreeOrMore;
        }

        public int Part2(string input)
        {
            var samples = GetSamples(input);
            var opCodeMapping = GetOpCodeMapping(samples);
            var registers = new Dictionary<int, int>
            {
                { 0, 0 },
                { 1, 0 },
                { 2, 0 },
                { 3, 0 }
            };
            var instructions = input.Substring(input.LastIndexOf("\r\n\r\n\r\n") + 6).Split("\r\n", StringSplitOptions.RemoveEmptyEntries).ToList();
            ExecuteInstructions(registers, instructions, opCodeMapping);

            return registers[0];
        }

        private void ExecuteInstructions(Dictionary<int, int> registers, List<string> instructions, Dictionary<int, string> opCodeMapping)
        {
            foreach (var instruction in instructions)
            {
                var parameters = instruction.Split(' ').Select(x => int.Parse(x)).ToList();
                Instructions[opCodeMapping[parameters[0]]](registers, parameters[1], parameters[2], parameters[3]);
            }
        }

        private Dictionary<int, string> GetOpCodeMapping(List<Sample> samples)
        {
            var opCodeMapping = new Dictionary<int, List<string>>();
            foreach (var sample in samples)
            {
                var opCodes = GetPossibleOpCodes(sample);
                if (!opCodeMapping.ContainsKey(sample.Instruction[0]))
                {
                    opCodeMapping.Add(sample.Instruction[0], opCodes);
                    continue;
                }

                for (var i = 0; i < opCodeMapping[sample.Instruction[0]].Count; i++)
                {
                    if (!opCodes.Contains(opCodeMapping[sample.Instruction[0]][i]))
                    {
                        opCodeMapping[sample.Instruction[0]].RemoveAt(i);
                        i--;
                    }
                }
            }

            var opCodeSingleMapping = new Dictionary<int, string>();
            while (opCodeSingleMapping.Count < 16)
            {
                var single = opCodeMapping.First(x => x.Value.Count == 1);
                var code = single.Value[0];
                opCodeSingleMapping.Add(single.Key, code);
                opCodeMapping.Remove(single.Key);
                foreach (var oldMap in opCodeMapping)
                    oldMap.Value.Remove(code);
            }

            return opCodeSingleMapping;
        }

        private List<Sample> GetSamples(string input)
        {
            var matches = Regex.Matches(input, @"Before:\s*\[(\d*), (\d*), (\d*), (\d*)\]\r?\n(\d*) (\d*) (\d*) (\d*)\r?\nAfter:\s*\[(\d*), (\d*), (\d*), (\d*)\]");
            return matches.Select(x => new Sample
            {
                BeforeState = new Dictionary<int, int>
                {
                    { 0, int.Parse(x.Groups[1].Value) },
                    { 1, int.Parse(x.Groups[2].Value) },
                    { 2, int.Parse(x.Groups[3].Value) },
                    { 3, int.Parse(x.Groups[4].Value) }
                },
                Instruction = new List<int>
                {
                    int.Parse(x.Groups[5].Value),
                    int.Parse(x.Groups[6].Value),
                    int.Parse(x.Groups[7].Value),
                    int.Parse(x.Groups[8].Value)
                },
                AfterState = new Dictionary<int, int>
                {
                    { 0, int.Parse(x.Groups[9].Value) },
                    { 1, int.Parse(x.Groups[10].Value) },
                    { 2, int.Parse(x.Groups[11].Value) },
                    { 3, int.Parse(x.Groups[12].Value) }
                }
            }).ToList();
        }

        private List<string> GetPossibleOpCodes(Sample sample)
        {
            var opCodes = new List<string>();
            foreach (var op in Instructions)
            {
                var testRegisters = new Dictionary<int, int>(sample.BeforeState);
                op.Value(testRegisters, sample.Instruction[1], sample.Instruction[2], sample.Instruction[3]);
                if (testRegisters.SequenceEqual(sample.AfterState))
                    opCodes.Add(op.Key);
            }

            return opCodes;
        }

        public static void Run()
        {
            var input = GetInput();
            var exampleInput = @"Before: [3, 2, 1, 1]
9 2 1 2
After:  [3, 2, 2, 1]

Before: [2, 2, 1, 3]
3 1 0 2
After:  [2, 2, 1, 3]";
            var Day16 = new Day16();
            Console.WriteLine("\n###############\n###############\nDay16\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(Day16.Part1(exampleInput));
            Console.WriteLine(Day16.Part1(input));
            
            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(Day16.Part2(input));
        }

        private static string GetInput()
        {
            return @"Before: [0, 2, 2, 2]
4 2 3 2
After:  [0, 2, 5, 2]

Before: [2, 2, 1, 3]
3 1 0 2
After:  [2, 2, 1, 3]

Before: [0, 2, 0, 0]
8 1 2 0
After:  [4, 2, 0, 0]

Before: [2, 2, 2, 0]
2 1 2 2
After:  [2, 2, 1, 0]

Before: [3, 1, 2, 2]
11 1 0 3
After:  [3, 1, 2, 3]

Before: [1, 1, 2, 3]
12 0 3 0
After:  [0, 1, 2, 3]

Before: [3, 1, 2, 2]
15 0 3 3
After:  [3, 1, 2, 6]

Before: [1, 2, 2, 2]
6 1 0 1
After:  [1, 1, 2, 2]

Before: [0, 1, 3, 1]
5 1 0 2
After:  [0, 1, 1, 1]

Before: [2, 2, 1, 0]
3 1 0 1
After:  [2, 1, 1, 0]

Before: [3, 3, 1, 2]
6 3 3 1
After:  [3, 0, 1, 2]

Before: [2, 2, 3, 0]
3 1 1 0
After:  [1, 2, 3, 0]

Before: [0, 2, 2, 1]
15 3 1 0
After:  [2, 2, 2, 1]

Before: [0, 1, 3, 1]
5 1 0 0
After:  [1, 1, 3, 1]

Before: [0, 1, 0, 3]
5 1 0 0
After:  [1, 1, 0, 3]

Before: [3, 2, 2, 3]
7 1 0 3
After:  [3, 2, 2, 1]

Before: [0, 2, 3, 0]
10 0 0 1
After:  [0, 0, 3, 0]

Before: [0, 1, 1, 0]
4 2 3 0
After:  [4, 1, 1, 0]

Before: [2, 1, 2, 3]
14 0 2 1
After:  [2, 4, 2, 3]

Before: [2, 1, 3, 2]
1 3 1 2
After:  [2, 1, 3, 2]

Before: [2, 2, 2, 1]
1 3 2 0
After:  [3, 2, 2, 1]

Before: [0, 0, 3, 1]
10 0 0 0
After:  [0, 0, 3, 1]

Before: [0, 2, 2, 3]
2 1 2 3
After:  [0, 2, 2, 1]

Before: [2, 1, 0, 3]
12 0 3 2
After:  [2, 1, 0, 3]

Before: [3, 0, 3, 3]
4 2 2 1
After:  [3, 5, 3, 3]

Before: [0, 3, 3, 1]
3 1 2 0
After:  [1, 3, 3, 1]

Before: [2, 0, 0, 3]
12 0 3 1
After:  [2, 0, 0, 3]

Before: [2, 0, 3, 2]
4 2 2 2
After:  [2, 0, 5, 2]

Before: [1, 3, 3, 0]
3 1 2 1
After:  [1, 1, 3, 0]

Before: [1, 1, 2, 2]
4 0 3 0
After:  [4, 1, 2, 2]

Before: [1, 2, 2, 3]
15 0 1 3
After:  [1, 2, 2, 2]

Before: [3, 2, 3, 0]
7 1 0 3
After:  [3, 2, 3, 1]

Before: [0, 1, 2, 0]
10 0 0 2
After:  [0, 1, 0, 0]

Before: [0, 3, 2, 1]
10 0 0 0
After:  [0, 3, 2, 1]

Before: [2, 1, 3, 2]
9 1 3 0
After:  [3, 1, 3, 2]

Before: [1, 1, 3, 0]
13 1 0 2
After:  [1, 1, 1, 0]

Before: [3, 1, 2, 2]
6 3 3 2
After:  [3, 1, 0, 2]

Before: [3, 1, 3, 2]
15 2 3 2
After:  [3, 1, 6, 2]

Before: [3, 2, 2, 2]
7 1 0 1
After:  [3, 1, 2, 2]

Before: [1, 1, 0, 2]
13 1 0 2
After:  [1, 1, 1, 2]

Before: [1, 1, 2, 3]
13 1 0 0
After:  [1, 1, 2, 3]

Before: [1, 1, 2, 1]
9 2 1 2
After:  [1, 1, 3, 1]

Before: [1, 2, 2, 2]
2 1 2 1
After:  [1, 1, 2, 2]

Before: [1, 0, 1, 2]
0 1 0 3
After:  [1, 0, 1, 1]

Before: [1, 2, 2, 2]
6 3 3 0
After:  [0, 2, 2, 2]

Before: [2, 2, 2, 2]
3 1 1 1
After:  [2, 1, 2, 2]

Before: [1, 3, 0, 3]
9 0 3 0
After:  [3, 3, 0, 3]

Before: [0, 2, 2, 1]
2 1 2 1
After:  [0, 1, 2, 1]

Before: [3, 2, 1, 0]
7 1 0 0
After:  [1, 2, 1, 0]

Before: [2, 0, 3, 1]
6 3 3 0
After:  [0, 0, 3, 1]

Before: [2, 2, 2, 2]
6 3 3 1
After:  [2, 0, 2, 2]

Before: [1, 1, 2, 0]
13 1 0 1
After:  [1, 1, 2, 0]

Before: [3, 3, 1, 3]
11 2 0 2
After:  [3, 3, 3, 3]

Before: [2, 2, 3, 0]
3 1 0 2
After:  [2, 2, 1, 0]

Before: [1, 2, 0, 3]
12 0 3 2
After:  [1, 2, 0, 3]

Before: [0, 2, 3, 1]
10 0 0 0
After:  [0, 2, 3, 1]

Before: [1, 0, 2, 2]
0 1 0 3
After:  [1, 0, 2, 1]

Before: [1, 0, 1, 0]
0 1 0 3
After:  [1, 0, 1, 1]

Before: [0, 1, 1, 2]
5 1 0 3
After:  [0, 1, 1, 1]

Before: [0, 3, 0, 2]
11 0 3 2
After:  [0, 3, 2, 2]

Before: [3, 2, 3, 2]
15 2 3 1
After:  [3, 6, 3, 2]

Before: [1, 1, 2, 3]
13 1 0 1
After:  [1, 1, 2, 3]

Before: [3, 1, 0, 1]
11 1 0 2
After:  [3, 1, 3, 1]

Before: [1, 1, 0, 0]
13 1 0 0
After:  [1, 1, 0, 0]

Before: [0, 2, 2, 1]
2 1 2 3
After:  [0, 2, 2, 1]

Before: [3, 1, 2, 1]
1 3 2 3
After:  [3, 1, 2, 3]

Before: [3, 3, 3, 2]
6 3 3 3
After:  [3, 3, 3, 0]

Before: [1, 0, 3, 2]
11 1 2 0
After:  [3, 0, 3, 2]

Before: [1, 0, 1, 2]
0 1 0 1
After:  [1, 1, 1, 2]

Before: [1, 3, 0, 3]
9 0 3 2
After:  [1, 3, 3, 3]

Before: [3, 3, 3, 3]
3 1 2 0
After:  [1, 3, 3, 3]

Before: [3, 2, 1, 1]
7 1 0 3
After:  [3, 2, 1, 1]

Before: [3, 2, 2, 3]
7 1 0 0
After:  [1, 2, 2, 3]

Before: [3, 1, 2, 0]
14 2 2 2
After:  [3, 1, 4, 0]

Before: [0, 0, 1, 2]
10 0 0 0
After:  [0, 0, 1, 2]

Before: [2, 3, 2, 3]
2 1 3 3
After:  [2, 3, 2, 1]

Before: [3, 2, 3, 1]
4 0 3 2
After:  [3, 2, 6, 1]

Before: [2, 1, 0, 2]
6 3 3 3
After:  [2, 1, 0, 0]

Before: [3, 2, 0, 2]
7 1 0 0
After:  [1, 2, 0, 2]

Before: [1, 0, 0, 1]
0 1 0 2
After:  [1, 0, 1, 1]

Before: [1, 1, 0, 0]
4 0 3 2
After:  [1, 1, 4, 0]

Before: [2, 2, 2, 3]
8 2 3 1
After:  [2, 6, 2, 3]

Before: [1, 0, 2, 2]
0 1 0 0
After:  [1, 0, 2, 2]

Before: [0, 2, 1, 0]
10 0 0 1
After:  [0, 0, 1, 0]

Before: [0, 3, 2, 3]
14 2 2 3
After:  [0, 3, 2, 4]

Before: [1, 3, 0, 3]
12 0 3 3
After:  [1, 3, 0, 0]

Before: [3, 2, 3, 3]
7 1 0 0
After:  [1, 2, 3, 3]

Before: [1, 1, 1, 3]
9 2 3 2
After:  [1, 1, 3, 3]

Before: [3, 2, 0, 1]
7 1 0 3
After:  [3, 2, 0, 1]

Before: [0, 2, 1, 3]
11 2 1 1
After:  [0, 3, 1, 3]

Before: [0, 0, 0, 1]
4 3 1 3
After:  [0, 0, 0, 2]

Before: [1, 1, 0, 2]
1 3 1 3
After:  [1, 1, 0, 3]

Before: [0, 1, 2, 1]
5 1 0 2
After:  [0, 1, 1, 1]

Before: [0, 2, 1, 0]
4 1 3 1
After:  [0, 5, 1, 0]

Before: [3, 3, 0, 3]
2 1 3 1
After:  [3, 1, 0, 3]

Before: [1, 0, 2, 1]
0 1 0 0
After:  [1, 0, 2, 1]

Before: [1, 1, 2, 1]
13 1 0 1
After:  [1, 1, 2, 1]

Before: [0, 3, 2, 1]
4 1 1 2
After:  [0, 3, 4, 1]

Before: [0, 3, 0, 3]
9 2 3 2
After:  [0, 3, 3, 3]

Before: [2, 2, 0, 3]
8 1 2 2
After:  [2, 2, 4, 3]

Before: [2, 2, 0, 0]
3 1 1 2
After:  [2, 2, 1, 0]

Before: [0, 1, 2, 3]
5 1 0 1
After:  [0, 1, 2, 3]

Before: [1, 1, 1, 1]
13 1 0 3
After:  [1, 1, 1, 1]

Before: [3, 1, 3, 1]
6 3 3 2
After:  [3, 1, 0, 1]

Before: [0, 2, 0, 2]
6 3 3 2
After:  [0, 2, 0, 2]

Before: [0, 3, 0, 2]
10 0 0 0
After:  [0, 3, 0, 2]

Before: [2, 3, 2, 3]
14 2 2 0
After:  [4, 3, 2, 3]

Before: [1, 2, 1, 0]
3 1 1 0
After:  [1, 2, 1, 0]

Before: [2, 0, 1, 1]
11 0 2 3
After:  [2, 0, 1, 3]

Before: [2, 2, 1, 3]
12 0 3 2
After:  [2, 2, 0, 3]

Before: [0, 2, 0, 3]
8 1 2 3
After:  [0, 2, 0, 4]

Before: [2, 1, 3, 2]
8 3 2 0
After:  [4, 1, 3, 2]

Before: [1, 0, 0, 2]
0 1 0 1
After:  [1, 1, 0, 2]

Before: [1, 1, 1, 0]
13 1 0 2
After:  [1, 1, 1, 0]

Before: [3, 1, 2, 2]
14 2 2 3
After:  [3, 1, 2, 4]

Before: [1, 2, 2, 1]
14 1 2 2
After:  [1, 2, 4, 1]

Before: [3, 3, 2, 3]
2 1 3 0
After:  [1, 3, 2, 3]

Before: [3, 2, 2, 0]
11 3 1 3
After:  [3, 2, 2, 2]

Before: [2, 2, 0, 2]
3 1 1 0
After:  [1, 2, 0, 2]

Before: [1, 3, 3, 2]
4 2 1 3
After:  [1, 3, 3, 4]

Before: [1, 3, 2, 3]
2 1 3 1
After:  [1, 1, 2, 3]

Before: [1, 0, 3, 1]
0 1 0 3
After:  [1, 0, 3, 1]

Before: [1, 0, 1, 3]
0 1 0 0
After:  [1, 0, 1, 3]

Before: [3, 2, 0, 1]
7 1 0 0
After:  [1, 2, 0, 1]

Before: [3, 2, 1, 2]
7 1 0 0
After:  [1, 2, 1, 2]

Before: [0, 2, 2, 2]
2 1 2 1
After:  [0, 1, 2, 2]

Before: [0, 1, 2, 3]
5 1 0 2
After:  [0, 1, 1, 3]

Before: [1, 0, 2, 3]
0 1 0 1
After:  [1, 1, 2, 3]

Before: [0, 1, 0, 2]
5 1 0 3
After:  [0, 1, 0, 1]

Before: [2, 1, 3, 3]
11 1 0 3
After:  [2, 1, 3, 3]

Before: [1, 3, 1, 1]
6 3 3 1
After:  [1, 0, 1, 1]

Before: [3, 0, 3, 1]
1 3 2 3
After:  [3, 0, 3, 3]

Before: [1, 2, 3, 0]
9 0 2 2
After:  [1, 2, 3, 0]

Before: [2, 2, 2, 0]
11 3 0 1
After:  [2, 2, 2, 0]

Before: [2, 2, 2, 1]
14 0 2 3
After:  [2, 2, 2, 4]

Before: [3, 2, 0, 2]
7 1 0 3
After:  [3, 2, 0, 1]

Before: [0, 2, 1, 3]
9 2 3 2
After:  [0, 2, 3, 3]

Before: [1, 1, 2, 2]
9 0 3 2
After:  [1, 1, 3, 2]

Before: [2, 1, 3, 3]
12 0 3 2
After:  [2, 1, 0, 3]

Before: [0, 2, 2, 0]
14 1 2 3
After:  [0, 2, 2, 4]

Before: [2, 2, 0, 3]
12 0 3 2
After:  [2, 2, 0, 3]

Before: [0, 0, 3, 2]
15 2 3 1
After:  [0, 6, 3, 2]

Before: [0, 3, 0, 1]
4 1 3 0
After:  [6, 3, 0, 1]

Before: [2, 2, 2, 3]
12 0 3 0
After:  [0, 2, 2, 3]

Before: [0, 1, 1, 0]
5 1 0 1
After:  [0, 1, 1, 0]

Before: [0, 3, 1, 3]
2 1 3 3
After:  [0, 3, 1, 1]

Before: [1, 1, 2, 0]
14 2 2 2
After:  [1, 1, 4, 0]

Before: [0, 0, 2, 1]
10 0 0 3
After:  [0, 0, 2, 0]

Before: [3, 3, 3, 1]
3 1 2 0
After:  [1, 3, 3, 1]

Before: [3, 2, 1, 1]
11 1 2 3
After:  [3, 2, 1, 3]

Before: [2, 2, 1, 1]
6 1 2 2
After:  [2, 2, 1, 1]

Before: [0, 0, 1, 0]
10 0 0 2
After:  [0, 0, 0, 0]

Before: [3, 3, 3, 3]
3 1 0 0
After:  [1, 3, 3, 3]

Before: [3, 0, 2, 1]
1 3 2 2
After:  [3, 0, 3, 1]

Before: [1, 1, 2, 3]
13 1 0 2
After:  [1, 1, 1, 3]

Before: [2, 1, 1, 1]
6 2 3 0
After:  [0, 1, 1, 1]

Before: [1, 1, 2, 2]
1 3 1 3
After:  [1, 1, 2, 3]

Before: [1, 2, 3, 3]
15 0 1 0
After:  [2, 2, 3, 3]

Before: [0, 1, 2, 2]
5 1 0 3
After:  [0, 1, 2, 1]

Before: [1, 2, 0, 3]
12 0 3 3
After:  [1, 2, 0, 0]

Before: [1, 0, 3, 1]
6 3 3 1
After:  [1, 0, 3, 1]

Before: [3, 0, 1, 2]
15 0 3 2
After:  [3, 0, 6, 2]

Before: [3, 3, 2, 0]
8 1 2 2
After:  [3, 3, 6, 0]

Before: [0, 1, 1, 2]
1 3 1 1
After:  [0, 3, 1, 2]

Before: [2, 1, 3, 3]
9 0 1 1
After:  [2, 3, 3, 3]

Before: [2, 3, 2, 3]
12 0 3 3
After:  [2, 3, 2, 0]

Before: [2, 1, 0, 3]
9 1 3 2
After:  [2, 1, 3, 3]

Before: [3, 0, 2, 1]
9 1 2 2
After:  [3, 0, 2, 1]

Before: [2, 0, 1, 3]
8 0 3 0
After:  [6, 0, 1, 3]

Before: [1, 0, 2, 1]
0 1 0 1
After:  [1, 1, 2, 1]

Before: [2, 1, 2, 3]
8 2 3 3
After:  [2, 1, 2, 6]

Before: [1, 0, 3, 3]
12 0 3 3
After:  [1, 0, 3, 0]

Before: [0, 3, 3, 0]
4 2 2 2
After:  [0, 3, 5, 0]

Before: [0, 3, 3, 3]
10 0 0 3
After:  [0, 3, 3, 0]

Before: [3, 2, 2, 2]
14 2 2 2
After:  [3, 2, 4, 2]

Before: [0, 3, 1, 3]
8 3 3 3
After:  [0, 3, 1, 9]

Before: [0, 2, 0, 1]
3 1 1 3
After:  [0, 2, 0, 1]

Before: [0, 2, 2, 1]
14 2 2 0
After:  [4, 2, 2, 1]

Before: [1, 3, 0, 3]
2 1 3 1
After:  [1, 1, 0, 3]

Before: [2, 3, 2, 3]
2 1 3 2
After:  [2, 3, 1, 3]

Before: [3, 2, 2, 1]
7 1 0 2
After:  [3, 2, 1, 1]

Before: [1, 3, 1, 3]
12 0 3 0
After:  [0, 3, 1, 3]

Before: [1, 1, 2, 3]
8 1 2 1
After:  [1, 2, 2, 3]

Before: [1, 2, 3, 0]
15 0 1 0
After:  [2, 2, 3, 0]

Before: [1, 0, 0, 2]
0 1 0 2
After:  [1, 0, 1, 2]

Before: [0, 2, 2, 3]
3 1 1 0
After:  [1, 2, 2, 3]

Before: [3, 2, 2, 0]
3 1 1 0
After:  [1, 2, 2, 0]

Before: [1, 3, 2, 1]
1 3 2 0
After:  [3, 3, 2, 1]

Before: [1, 1, 3, 0]
13 1 0 1
After:  [1, 1, 3, 0]

Before: [1, 1, 2, 1]
13 1 0 3
After:  [1, 1, 2, 1]

Before: [1, 0, 1, 1]
0 1 0 0
After:  [1, 0, 1, 1]

Before: [2, 3, 3, 2]
4 2 2 3
After:  [2, 3, 3, 5]

Before: [3, 2, 1, 2]
7 1 0 3
After:  [3, 2, 1, 1]

Before: [1, 0, 3, 2]
0 1 0 0
After:  [1, 0, 3, 2]

Before: [0, 2, 0, 3]
10 0 0 1
After:  [0, 0, 0, 3]

Before: [3, 1, 2, 3]
9 1 3 0
After:  [3, 1, 2, 3]

Before: [0, 0, 3, 1]
1 3 2 0
After:  [3, 0, 3, 1]

Before: [0, 3, 2, 1]
1 3 2 1
After:  [0, 3, 2, 1]

Before: [3, 2, 3, 1]
15 3 1 3
After:  [3, 2, 3, 2]

Before: [3, 2, 1, 1]
15 3 1 3
After:  [3, 2, 1, 2]

Before: [2, 0, 0, 3]
12 0 3 0
After:  [0, 0, 0, 3]

Before: [2, 2, 1, 2]
4 2 3 3
After:  [2, 2, 1, 4]

Before: [0, 1, 2, 3]
8 3 3 2
After:  [0, 1, 9, 3]

Before: [1, 2, 2, 3]
12 0 3 3
After:  [1, 2, 2, 0]

Before: [2, 2, 0, 3]
8 1 2 3
After:  [2, 2, 0, 4]

Before: [1, 0, 0, 3]
0 1 0 2
After:  [1, 0, 1, 3]

Before: [0, 2, 0, 2]
10 0 0 2
After:  [0, 2, 0, 2]

Before: [2, 2, 3, 2]
3 1 0 0
After:  [1, 2, 3, 2]

Before: [2, 0, 0, 3]
12 0 3 2
After:  [2, 0, 0, 3]

Before: [2, 0, 2, 3]
8 2 3 3
After:  [2, 0, 2, 6]

Before: [1, 0, 2, 1]
0 1 0 2
After:  [1, 0, 1, 1]

Before: [1, 2, 2, 3]
14 1 2 3
After:  [1, 2, 2, 4]

Before: [0, 3, 1, 3]
11 0 1 2
After:  [0, 3, 3, 3]

Before: [1, 0, 3, 2]
15 2 3 0
After:  [6, 0, 3, 2]

Before: [0, 1, 1, 1]
5 1 0 3
After:  [0, 1, 1, 1]

Before: [0, 1, 3, 2]
5 1 0 1
After:  [0, 1, 3, 2]

Before: [3, 2, 1, 2]
8 1 2 3
After:  [3, 2, 1, 4]

Before: [2, 1, 2, 3]
12 0 3 3
After:  [2, 1, 2, 0]

Before: [0, 1, 0, 0]
5 1 0 2
After:  [0, 1, 1, 0]

Before: [1, 1, 2, 1]
4 0 3 2
After:  [1, 1, 4, 1]

Before: [1, 0, 3, 1]
4 2 2 1
After:  [1, 5, 3, 1]

Before: [1, 1, 3, 1]
13 1 0 1
After:  [1, 1, 3, 1]

Before: [3, 2, 3, 1]
7 1 0 3
After:  [3, 2, 3, 1]

Before: [1, 1, 0, 2]
13 1 0 1
After:  [1, 1, 0, 2]

Before: [3, 2, 0, 3]
7 1 0 3
After:  [3, 2, 0, 1]

Before: [3, 2, 2, 1]
15 3 1 2
After:  [3, 2, 2, 1]

Before: [0, 3, 0, 2]
15 1 3 1
After:  [0, 6, 0, 2]

Before: [3, 2, 0, 3]
7 1 0 0
After:  [1, 2, 0, 3]

Before: [3, 2, 0, 2]
7 1 0 1
After:  [3, 1, 0, 2]

Before: [0, 2, 2, 3]
9 0 3 1
After:  [0, 3, 2, 3]

Before: [0, 2, 2, 1]
11 0 3 1
After:  [0, 1, 2, 1]

Before: [2, 2, 1, 0]
3 1 1 1
After:  [2, 1, 1, 0]

Before: [2, 3, 3, 2]
15 2 3 1
After:  [2, 6, 3, 2]

Before: [0, 2, 3, 2]
10 0 0 0
After:  [0, 2, 3, 2]

Before: [1, 0, 2, 2]
9 0 3 2
After:  [1, 0, 3, 2]

Before: [2, 1, 1, 3]
8 3 3 0
After:  [9, 1, 1, 3]

Before: [1, 2, 3, 3]
9 0 2 0
After:  [3, 2, 3, 3]

Before: [1, 2, 2, 1]
2 1 2 3
After:  [1, 2, 2, 1]

Before: [1, 2, 0, 2]
15 0 1 1
After:  [1, 2, 0, 2]

Before: [0, 2, 3, 0]
10 0 0 3
After:  [0, 2, 3, 0]

Before: [2, 2, 0, 2]
3 1 1 2
After:  [2, 2, 1, 2]

Before: [3, 2, 2, 2]
14 2 2 0
After:  [4, 2, 2, 2]

Before: [0, 0, 3, 2]
15 2 3 0
After:  [6, 0, 3, 2]

Before: [0, 3, 0, 3]
10 0 0 0
After:  [0, 3, 0, 3]

Before: [0, 1, 1, 2]
6 3 3 3
After:  [0, 1, 1, 0]

Before: [1, 2, 0, 3]
11 0 1 3
After:  [1, 2, 0, 3]

Before: [0, 2, 1, 0]
6 1 2 3
After:  [0, 2, 1, 1]

Before: [1, 0, 0, 0]
0 1 0 1
After:  [1, 1, 0, 0]

Before: [3, 3, 1, 1]
6 3 3 2
After:  [3, 3, 0, 1]

Before: [0, 1, 3, 3]
9 0 1 1
After:  [0, 1, 3, 3]

Before: [1, 0, 1, 1]
0 1 0 1
After:  [1, 1, 1, 1]

Before: [1, 1, 2, 0]
13 1 0 0
After:  [1, 1, 2, 0]

Before: [2, 2, 2, 2]
14 2 2 1
After:  [2, 4, 2, 2]

Before: [0, 1, 1, 3]
10 0 0 2
After:  [0, 1, 0, 3]

Before: [0, 2, 1, 1]
10 0 0 3
After:  [0, 2, 1, 0]

Before: [3, 3, 0, 3]
2 1 3 2
After:  [3, 3, 1, 3]

Before: [0, 1, 3, 3]
10 0 0 0
After:  [0, 1, 3, 3]

Before: [1, 0, 3, 3]
0 1 0 2
After:  [1, 0, 1, 3]

Before: [0, 2, 3, 3]
10 0 0 1
After:  [0, 0, 3, 3]

Before: [2, 3, 1, 2]
11 2 1 3
After:  [2, 3, 1, 3]

Before: [3, 3, 3, 2]
4 0 1 2
After:  [3, 3, 4, 2]

Before: [0, 0, 2, 0]
9 0 2 1
After:  [0, 2, 2, 0]

Before: [1, 0, 1, 3]
0 1 0 2
After:  [1, 0, 1, 3]

Before: [1, 2, 0, 1]
15 3 1 0
After:  [2, 2, 0, 1]

Before: [1, 3, 3, 3]
12 0 3 2
After:  [1, 3, 0, 3]

Before: [3, 3, 0, 3]
2 1 3 3
After:  [3, 3, 0, 1]

Before: [0, 3, 1, 2]
11 0 1 1
After:  [0, 3, 1, 2]

Before: [2, 0, 1, 0]
11 0 2 3
After:  [2, 0, 1, 3]

Before: [2, 2, 2, 3]
2 1 2 0
After:  [1, 2, 2, 3]

Before: [2, 0, 3, 1]
1 3 2 1
After:  [2, 3, 3, 1]

Before: [0, 0, 3, 1]
10 0 0 2
After:  [0, 0, 0, 1]

Before: [3, 2, 0, 0]
7 1 0 1
After:  [3, 1, 0, 0]

Before: [1, 1, 0, 3]
13 1 0 3
After:  [1, 1, 0, 1]

Before: [1, 2, 1, 3]
12 0 3 0
After:  [0, 2, 1, 3]

Before: [1, 0, 2, 2]
14 3 2 0
After:  [4, 0, 2, 2]

Before: [2, 2, 2, 1]
14 2 2 0
After:  [4, 2, 2, 1]

Before: [2, 3, 1, 3]
11 2 0 2
After:  [2, 3, 3, 3]

Before: [0, 1, 3, 3]
5 1 0 2
After:  [0, 1, 1, 3]

Before: [3, 3, 3, 3]
2 1 3 3
After:  [3, 3, 3, 1]

Before: [1, 1, 2, 2]
13 1 0 1
After:  [1, 1, 2, 2]

Before: [0, 1, 2, 2]
5 1 0 1
After:  [0, 1, 2, 2]

Before: [1, 0, 1, 1]
4 2 3 3
After:  [1, 0, 1, 4]

Before: [2, 2, 1, 1]
15 3 1 1
After:  [2, 2, 1, 1]

Before: [1, 3, 0, 1]
11 0 1 3
After:  [1, 3, 0, 3]

Before: [3, 3, 2, 3]
8 2 3 3
After:  [3, 3, 2, 6]

Before: [3, 1, 1, 1]
6 2 3 0
After:  [0, 1, 1, 1]

Before: [0, 3, 0, 2]
6 3 3 1
After:  [0, 0, 0, 2]

Before: [0, 2, 3, 1]
15 3 1 0
After:  [2, 2, 3, 1]

Before: [1, 0, 3, 3]
12 0 3 2
After:  [1, 0, 0, 3]

Before: [0, 1, 3, 1]
1 3 2 3
After:  [0, 1, 3, 3]

Before: [2, 1, 2, 2]
9 1 2 3
After:  [2, 1, 2, 3]

Before: [2, 1, 2, 2]
9 0 1 3
After:  [2, 1, 2, 3]

Before: [1, 0, 1, 2]
0 1 0 2
After:  [1, 0, 1, 2]

Before: [1, 0, 1, 0]
0 1 0 2
After:  [1, 0, 1, 0]

Before: [3, 2, 2, 1]
3 1 1 3
After:  [3, 2, 2, 1]

Before: [0, 3, 2, 3]
2 1 3 3
After:  [0, 3, 2, 1]

Before: [1, 2, 3, 3]
6 1 0 1
After:  [1, 1, 3, 3]

Before: [2, 2, 2, 2]
2 1 2 3
After:  [2, 2, 2, 1]

Before: [2, 2, 2, 1]
3 1 1 2
After:  [2, 2, 1, 1]

Before: [1, 1, 0, 2]
13 1 0 0
After:  [1, 1, 0, 2]

Before: [2, 0, 0, 3]
12 0 3 3
After:  [2, 0, 0, 0]

Before: [1, 1, 1, 1]
6 2 3 0
After:  [0, 1, 1, 1]

Before: [3, 2, 1, 3]
11 2 0 0
After:  [3, 2, 1, 3]

Before: [1, 3, 1, 2]
15 1 3 1
After:  [1, 6, 1, 2]

Before: [0, 2, 1, 0]
3 1 1 0
After:  [1, 2, 1, 0]

Before: [1, 1, 1, 3]
12 0 3 3
After:  [1, 1, 1, 0]

Before: [3, 2, 3, 2]
3 1 1 2
After:  [3, 2, 1, 2]

Before: [1, 0, 2, 0]
9 0 2 1
After:  [1, 3, 2, 0]

Before: [3, 3, 1, 2]
8 3 2 0
After:  [4, 3, 1, 2]

Before: [1, 1, 0, 3]
13 1 0 0
After:  [1, 1, 0, 3]

Before: [1, 1, 3, 0]
13 1 0 0
After:  [1, 1, 3, 0]

Before: [3, 0, 2, 3]
8 3 2 3
After:  [3, 0, 2, 6]

Before: [2, 3, 3, 2]
15 2 3 2
After:  [2, 3, 6, 2]

Before: [1, 1, 0, 2]
13 1 0 3
After:  [1, 1, 0, 1]

Before: [0, 1, 2, 1]
1 3 2 2
After:  [0, 1, 3, 1]

Before: [3, 0, 2, 1]
6 3 3 2
After:  [3, 0, 0, 1]

Before: [3, 2, 1, 0]
7 1 0 3
After:  [3, 2, 1, 1]

Before: [3, 2, 1, 2]
15 0 3 1
After:  [3, 6, 1, 2]

Before: [1, 2, 1, 1]
11 1 2 1
After:  [1, 3, 1, 1]

Before: [3, 0, 2, 0]
14 2 2 1
After:  [3, 4, 2, 0]

Before: [0, 2, 3, 1]
1 3 2 3
After:  [0, 2, 3, 3]

Before: [3, 1, 3, 0]
11 1 0 0
After:  [3, 1, 3, 0]

Before: [2, 3, 1, 1]
6 2 3 1
After:  [2, 0, 1, 1]

Before: [3, 2, 0, 0]
7 1 0 2
After:  [3, 2, 1, 0]

Before: [1, 1, 3, 1]
13 1 0 2
After:  [1, 1, 1, 1]

Before: [0, 3, 1, 3]
2 1 3 2
After:  [0, 3, 1, 3]

Before: [0, 1, 2, 2]
5 1 0 2
After:  [0, 1, 1, 2]

Before: [1, 2, 3, 3]
12 0 3 3
After:  [1, 2, 3, 0]

Before: [0, 1, 0, 1]
6 3 3 2
After:  [0, 1, 0, 1]

Before: [2, 2, 3, 1]
4 1 3 3
After:  [2, 2, 3, 5]

Before: [1, 2, 3, 3]
9 0 3 2
After:  [1, 2, 3, 3]

Before: [1, 0, 0, 0]
0 1 0 3
After:  [1, 0, 0, 1]

Before: [2, 1, 2, 1]
4 2 3 2
After:  [2, 1, 5, 1]

Before: [0, 3, 3, 3]
2 1 3 2
After:  [0, 3, 1, 3]

Before: [1, 0, 0, 1]
0 1 0 1
After:  [1, 1, 0, 1]

Before: [2, 2, 3, 1]
1 3 2 2
After:  [2, 2, 3, 1]

Before: [1, 0, 1, 0]
0 1 0 1
After:  [1, 1, 1, 0]

Before: [2, 0, 0, 0]
11 1 0 0
After:  [2, 0, 0, 0]

Before: [2, 0, 1, 1]
8 0 2 1
After:  [2, 4, 1, 1]

Before: [3, 2, 2, 1]
2 1 2 2
After:  [3, 2, 1, 1]

Before: [3, 2, 3, 2]
7 1 0 1
After:  [3, 1, 3, 2]

Before: [3, 3, 3, 3]
4 3 2 3
After:  [3, 3, 3, 5]

Before: [1, 2, 2, 2]
2 1 2 3
After:  [1, 2, 2, 1]

Before: [2, 3, 0, 3]
2 1 3 2
After:  [2, 3, 1, 3]

Before: [0, 3, 2, 1]
11 0 3 0
After:  [1, 3, 2, 1]

Before: [1, 1, 2, 0]
13 1 0 3
After:  [1, 1, 2, 1]

Before: [3, 2, 1, 3]
7 1 0 0
After:  [1, 2, 1, 3]

Before: [0, 1, 3, 2]
5 1 0 0
After:  [1, 1, 3, 2]

Before: [3, 1, 0, 0]
11 1 0 2
After:  [3, 1, 3, 0]

Before: [2, 3, 2, 1]
1 3 2 2
After:  [2, 3, 3, 1]

Before: [0, 1, 1, 0]
5 1 0 3
After:  [0, 1, 1, 1]

Before: [0, 1, 2, 3]
14 2 2 2
After:  [0, 1, 4, 3]

Before: [1, 0, 3, 2]
0 1 0 2
After:  [1, 0, 1, 2]

Before: [3, 1, 1, 1]
6 3 3 1
After:  [3, 0, 1, 1]

Before: [2, 0, 3, 2]
11 1 2 1
After:  [2, 3, 3, 2]

Before: [0, 2, 2, 1]
14 1 2 0
After:  [4, 2, 2, 1]

Before: [0, 1, 1, 2]
10 0 0 3
After:  [0, 1, 1, 0]

Before: [0, 0, 0, 1]
10 0 0 1
After:  [0, 0, 0, 1]

Before: [0, 1, 0, 1]
9 0 1 3
After:  [0, 1, 0, 1]

Before: [0, 2, 0, 0]
10 0 0 3
After:  [0, 2, 0, 0]

Before: [1, 0, 2, 0]
11 2 0 1
After:  [1, 3, 2, 0]

Before: [1, 2, 0, 0]
11 0 1 1
After:  [1, 3, 0, 0]

Before: [1, 1, 2, 1]
13 1 0 0
After:  [1, 1, 2, 1]

Before: [1, 1, 3, 3]
9 1 2 3
After:  [1, 1, 3, 3]

Before: [0, 0, 3, 2]
10 0 0 1
After:  [0, 0, 3, 2]

Before: [1, 3, 1, 3]
12 0 3 1
After:  [1, 0, 1, 3]

Before: [0, 3, 2, 1]
14 2 2 1
After:  [0, 4, 2, 1]

Before: [3, 2, 3, 1]
7 1 0 0
After:  [1, 2, 3, 1]

Before: [0, 2, 2, 0]
3 1 1 0
After:  [1, 2, 2, 0]

Before: [1, 1, 3, 0]
9 0 2 1
After:  [1, 3, 3, 0]

Before: [2, 0, 2, 1]
6 3 3 0
After:  [0, 0, 2, 1]

Before: [2, 0, 2, 1]
1 3 2 1
After:  [2, 3, 2, 1]

Before: [1, 2, 2, 3]
3 1 1 3
After:  [1, 2, 2, 1]

Before: [2, 2, 2, 2]
2 1 2 2
After:  [2, 2, 1, 2]

Before: [1, 3, 1, 3]
2 1 3 0
After:  [1, 3, 1, 3]

Before: [2, 3, 1, 2]
8 3 2 2
After:  [2, 3, 4, 2]

Before: [1, 1, 2, 3]
13 1 0 3
After:  [1, 1, 2, 1]

Before: [0, 2, 1, 3]
4 3 2 0
After:  [5, 2, 1, 3]

Before: [2, 1, 1, 3]
12 0 3 0
After:  [0, 1, 1, 3]

Before: [0, 0, 3, 1]
4 3 3 0
After:  [4, 0, 3, 1]

Before: [2, 0, 3, 2]
15 2 3 1
After:  [2, 6, 3, 2]

Before: [1, 0, 3, 3]
0 1 0 1
After:  [1, 1, 3, 3]

Before: [0, 2, 0, 1]
10 0 0 2
After:  [0, 2, 0, 1]

Before: [2, 1, 2, 2]
1 3 1 2
After:  [2, 1, 3, 2]

Before: [3, 2, 2, 0]
7 1 0 3
After:  [3, 2, 2, 1]

Before: [2, 3, 2, 3]
12 0 3 1
After:  [2, 0, 2, 3]

Before: [0, 0, 3, 0]
10 0 0 2
After:  [0, 0, 0, 0]

Before: [3, 2, 3, 3]
7 1 0 2
After:  [3, 2, 1, 3]

Before: [2, 2, 3, 2]
3 1 1 2
After:  [2, 2, 1, 2]

Before: [1, 0, 3, 1]
4 2 2 2
After:  [1, 0, 5, 1]

Before: [2, 0, 1, 1]
6 3 3 3
After:  [2, 0, 1, 0]

Before: [2, 0, 3, 2]
15 2 3 2
After:  [2, 0, 6, 2]

Before: [1, 2, 1, 3]
12 0 3 3
After:  [1, 2, 1, 0]

Before: [3, 1, 1, 2]
15 0 3 1
After:  [3, 6, 1, 2]

Before: [1, 0, 0, 3]
4 0 1 1
After:  [1, 2, 0, 3]

Before: [0, 3, 0, 0]
10 0 0 0
After:  [0, 3, 0, 0]

Before: [1, 3, 2, 2]
9 0 2 1
After:  [1, 3, 2, 2]

Before: [0, 2, 1, 0]
10 0 0 3
After:  [0, 2, 1, 0]

Before: [0, 1, 0, 1]
6 3 3 3
After:  [0, 1, 0, 0]

Before: [2, 0, 3, 3]
12 0 3 2
After:  [2, 0, 0, 3]

Before: [2, 0, 2, 2]
9 1 2 2
After:  [2, 0, 2, 2]

Before: [0, 1, 0, 1]
5 1 0 1
After:  [0, 1, 0, 1]

Before: [0, 3, 2, 2]
14 3 2 1
After:  [0, 4, 2, 2]

Before: [1, 1, 3, 3]
13 1 0 0
After:  [1, 1, 3, 3]

Before: [3, 2, 2, 1]
8 0 2 2
After:  [3, 2, 6, 1]

Before: [3, 2, 1, 3]
11 2 0 2
After:  [3, 2, 3, 3]

Before: [3, 3, 1, 3]
3 1 0 2
After:  [3, 3, 1, 3]

Before: [2, 0, 0, 1]
6 3 3 3
After:  [2, 0, 0, 0]

Before: [0, 0, 3, 2]
10 0 0 2
After:  [0, 0, 0, 2]

Before: [1, 1, 3, 1]
6 3 3 1
After:  [1, 0, 3, 1]

Before: [0, 0, 2, 2]
10 0 0 3
After:  [0, 0, 2, 0]

Before: [0, 0, 0, 2]
6 3 3 0
After:  [0, 0, 0, 2]

Before: [1, 0, 2, 1]
14 2 2 2
After:  [1, 0, 4, 1]

Before: [1, 1, 1, 2]
13 1 0 3
After:  [1, 1, 1, 1]

Before: [1, 3, 1, 3]
11 2 1 2
After:  [1, 3, 3, 3]

Before: [2, 1, 1, 2]
1 3 1 1
After:  [2, 3, 1, 2]

Before: [3, 0, 1, 0]
11 1 0 1
After:  [3, 3, 1, 0]

Before: [0, 2, 3, 2]
11 0 2 0
After:  [3, 2, 3, 2]

Before: [1, 0, 1, 3]
12 0 3 2
After:  [1, 0, 0, 3]

Before: [1, 1, 0, 0]
13 1 0 1
After:  [1, 1, 0, 0]

Before: [0, 1, 2, 0]
5 1 0 2
After:  [0, 1, 1, 0]

Before: [3, 1, 0, 2]
15 0 3 2
After:  [3, 1, 6, 2]

Before: [2, 0, 1, 0]
8 0 2 3
After:  [2, 0, 1, 4]

Before: [3, 1, 1, 2]
1 3 1 2
After:  [3, 1, 3, 2]

Before: [1, 2, 0, 1]
15 3 1 3
After:  [1, 2, 0, 2]

Before: [2, 1, 3, 2]
9 1 3 3
After:  [2, 1, 3, 3]

Before: [0, 3, 0, 1]
10 0 0 1
After:  [0, 0, 0, 1]

Before: [3, 0, 3, 1]
1 3 2 2
After:  [3, 0, 3, 1]

Before: [2, 0, 3, 1]
4 0 3 1
After:  [2, 5, 3, 1]

Before: [2, 3, 2, 1]
14 0 2 2
After:  [2, 3, 4, 1]

Before: [3, 2, 2, 3]
3 1 1 1
After:  [3, 1, 2, 3]

Before: [0, 1, 0, 3]
5 1 0 2
After:  [0, 1, 1, 3]

Before: [1, 3, 1, 3]
12 0 3 3
After:  [1, 3, 1, 0]

Before: [1, 1, 1, 3]
13 1 0 0
After:  [1, 1, 1, 3]

Before: [0, 0, 3, 0]
10 0 0 1
After:  [0, 0, 3, 0]

Before: [2, 2, 0, 2]
3 1 1 1
After:  [2, 1, 0, 2]

Before: [0, 0, 0, 0]
10 0 0 3
After:  [0, 0, 0, 0]

Before: [0, 1, 2, 3]
14 2 2 1
After:  [0, 4, 2, 3]

Before: [0, 1, 2, 1]
1 3 2 0
After:  [3, 1, 2, 1]

Before: [1, 2, 0, 1]
15 0 1 3
After:  [1, 2, 0, 2]

Before: [1, 3, 2, 3]
8 1 2 0
After:  [6, 3, 2, 3]

Before: [2, 2, 2, 2]
14 0 2 3
After:  [2, 2, 2, 4]

Before: [3, 0, 2, 2]
14 3 2 2
After:  [3, 0, 4, 2]

Before: [3, 2, 0, 1]
6 3 3 2
After:  [3, 2, 0, 1]

Before: [1, 1, 2, 3]
8 3 2 2
After:  [1, 1, 6, 3]

Before: [0, 1, 0, 3]
10 0 0 1
After:  [0, 0, 0, 3]

Before: [3, 3, 0, 2]
15 1 3 0
After:  [6, 3, 0, 2]

Before: [1, 0, 0, 0]
0 1 0 2
After:  [1, 0, 1, 0]

Before: [1, 3, 3, 3]
2 1 3 0
After:  [1, 3, 3, 3]

Before: [3, 3, 2, 1]
3 1 0 2
After:  [3, 3, 1, 1]

Before: [3, 2, 2, 1]
7 1 0 3
After:  [3, 2, 2, 1]

Before: [3, 2, 2, 0]
9 3 2 3
After:  [3, 2, 2, 2]

Before: [1, 1, 1, 3]
12 0 3 2
After:  [1, 1, 0, 3]

Before: [2, 2, 0, 3]
12 0 3 0
After:  [0, 2, 0, 3]

Before: [3, 1, 2, 1]
1 3 2 0
After:  [3, 1, 2, 1]

Before: [3, 2, 0, 3]
9 2 3 2
After:  [3, 2, 3, 3]

Before: [1, 2, 0, 0]
15 0 1 0
After:  [2, 2, 0, 0]

Before: [1, 2, 1, 2]
6 1 0 3
After:  [1, 2, 1, 1]

Before: [2, 0, 3, 1]
1 3 2 2
After:  [2, 0, 3, 1]

Before: [0, 0, 2, 3]
8 3 3 3
After:  [0, 0, 2, 9]

Before: [3, 2, 3, 0]
11 3 1 1
After:  [3, 2, 3, 0]

Before: [2, 2, 2, 2]
3 1 0 1
After:  [2, 1, 2, 2]

Before: [1, 2, 0, 1]
15 3 1 1
After:  [1, 2, 0, 1]

Before: [3, 2, 2, 0]
14 2 2 3
After:  [3, 2, 2, 4]

Before: [0, 0, 0, 1]
6 3 3 0
After:  [0, 0, 0, 1]

Before: [1, 0, 3, 0]
0 1 0 3
After:  [1, 0, 3, 1]

Before: [3, 2, 3, 2]
7 1 0 2
After:  [3, 2, 1, 2]

Before: [0, 3, 3, 3]
3 1 2 3
After:  [0, 3, 3, 1]

Before: [1, 0, 1, 2]
8 3 2 2
After:  [1, 0, 4, 2]

Before: [0, 2, 1, 3]
10 0 0 2
After:  [0, 2, 0, 3]

Before: [2, 3, 2, 2]
14 3 2 3
After:  [2, 3, 2, 4]

Before: [3, 0, 2, 1]
1 3 2 0
After:  [3, 0, 2, 1]

Before: [0, 0, 2, 1]
6 3 3 1
After:  [0, 0, 2, 1]

Before: [0, 1, 1, 2]
9 0 1 0
After:  [1, 1, 1, 2]

Before: [3, 2, 0, 2]
7 1 0 2
After:  [3, 2, 1, 2]

Before: [2, 2, 1, 2]
4 2 3 0
After:  [4, 2, 1, 2]

Before: [0, 2, 3, 2]
3 1 1 0
After:  [1, 2, 3, 2]

Before: [3, 1, 3, 1]
4 2 3 0
After:  [6, 1, 3, 1]

Before: [2, 3, 0, 1]
6 3 3 0
After:  [0, 3, 0, 1]

Before: [2, 2, 3, 1]
4 2 3 2
After:  [2, 2, 6, 1]

Before: [0, 1, 2, 1]
1 3 2 1
After:  [0, 3, 2, 1]

Before: [2, 2, 2, 3]
2 1 2 3
After:  [2, 2, 2, 1]

Before: [3, 0, 1, 2]
8 3 2 1
After:  [3, 4, 1, 2]

Before: [2, 2, 3, 3]
3 1 1 3
After:  [2, 2, 3, 1]

Before: [3, 2, 2, 2]
7 1 0 3
After:  [3, 2, 2, 1]

Before: [3, 2, 1, 2]
4 0 1 1
After:  [3, 4, 1, 2]

Before: [1, 0, 0, 2]
6 3 3 0
After:  [0, 0, 0, 2]

Before: [2, 3, 2, 1]
1 3 2 1
After:  [2, 3, 2, 1]

Before: [3, 2, 1, 0]
7 1 0 2
After:  [3, 2, 1, 0]

Before: [1, 1, 0, 0]
13 1 0 3
After:  [1, 1, 0, 1]

Before: [3, 2, 2, 2]
2 1 2 0
After:  [1, 2, 2, 2]

Before: [0, 2, 3, 2]
4 3 3 1
After:  [0, 5, 3, 2]

Before: [0, 0, 3, 3]
9 0 3 0
After:  [3, 0, 3, 3]

Before: [2, 2, 3, 0]
8 1 2 3
After:  [2, 2, 3, 4]

Before: [1, 2, 1, 3]
3 1 1 1
After:  [1, 1, 1, 3]

Before: [0, 1, 1, 3]
5 1 0 3
After:  [0, 1, 1, 1]

Before: [1, 3, 2, 1]
14 2 2 0
After:  [4, 3, 2, 1]

Before: [1, 0, 1, 2]
0 1 0 0
After:  [1, 0, 1, 2]

Before: [0, 1, 2, 1]
11 0 3 1
After:  [0, 1, 2, 1]

Before: [3, 3, 3, 2]
15 1 3 0
After:  [6, 3, 3, 2]

Before: [1, 0, 3, 1]
0 1 0 1
After:  [1, 1, 3, 1]

Before: [3, 3, 2, 1]
4 3 1 0
After:  [2, 3, 2, 1]

Before: [2, 2, 1, 3]
3 1 1 1
After:  [2, 1, 1, 3]

Before: [2, 3, 0, 2]
4 3 3 1
After:  [2, 5, 0, 2]

Before: [1, 1, 0, 2]
1 3 1 2
After:  [1, 1, 3, 2]

Before: [3, 2, 2, 2]
7 1 0 2
After:  [3, 2, 1, 2]

Before: [3, 2, 3, 3]
7 1 0 1
After:  [3, 1, 3, 3]

Before: [3, 0, 0, 2]
6 3 3 3
After:  [3, 0, 0, 0]

Before: [2, 3, 1, 3]
2 1 3 2
After:  [2, 3, 1, 3]

Before: [2, 2, 0, 3]
12 0 3 3
After:  [2, 2, 0, 0]

Before: [2, 3, 1, 3]
2 1 3 1
After:  [2, 1, 1, 3]

Before: [0, 3, 3, 3]
2 1 3 0
After:  [1, 3, 3, 3]

Before: [1, 1, 1, 2]
13 1 0 1
After:  [1, 1, 1, 2]

Before: [2, 0, 3, 1]
6 3 3 3
After:  [2, 0, 3, 0]

Before: [1, 2, 1, 1]
11 2 1 2
After:  [1, 2, 3, 1]

Before: [1, 3, 0, 2]
9 0 3 0
After:  [3, 3, 0, 2]

Before: [1, 3, 3, 3]
8 3 3 2
After:  [1, 3, 9, 3]

Before: [3, 3, 3, 2]
3 1 2 2
After:  [3, 3, 1, 2]

Before: [3, 2, 3, 3]
3 1 1 3
After:  [3, 2, 3, 1]

Before: [3, 3, 2, 2]
14 3 2 2
After:  [3, 3, 4, 2]

Before: [0, 2, 3, 0]
3 1 1 1
After:  [0, 1, 3, 0]

Before: [0, 1, 3, 1]
10 0 0 2
After:  [0, 1, 0, 1]

Before: [0, 0, 0, 3]
10 0 0 3
After:  [0, 0, 0, 0]

Before: [2, 0, 2, 3]
14 2 2 0
After:  [4, 0, 2, 3]

Before: [1, 0, 3, 2]
4 3 3 0
After:  [5, 0, 3, 2]

Before: [0, 1, 2, 1]
5 1 0 3
After:  [0, 1, 2, 1]

Before: [0, 3, 2, 3]
2 1 3 1
After:  [0, 1, 2, 3]

Before: [3, 1, 2, 0]
14 2 2 3
After:  [3, 1, 2, 4]

Before: [1, 3, 0, 3]
11 0 1 0
After:  [3, 3, 0, 3]

Before: [3, 2, 2, 3]
7 1 0 2
After:  [3, 2, 1, 3]

Before: [2, 2, 1, 3]
8 0 3 2
After:  [2, 2, 6, 3]

Before: [0, 2, 3, 1]
10 0 0 2
After:  [0, 2, 0, 1]

Before: [1, 0, 2, 1]
1 3 2 0
After:  [3, 0, 2, 1]

Before: [3, 2, 2, 1]
2 1 2 1
After:  [3, 1, 2, 1]

Before: [3, 2, 1, 3]
7 1 0 1
After:  [3, 1, 1, 3]

Before: [1, 1, 2, 1]
1 3 2 1
After:  [1, 3, 2, 1]

Before: [0, 1, 3, 2]
5 1 0 2
After:  [0, 1, 1, 2]

Before: [3, 1, 0, 3]
4 0 2 1
After:  [3, 5, 0, 3]

Before: [1, 0, 3, 2]
0 1 0 1
After:  [1, 1, 3, 2]

Before: [0, 2, 0, 1]
10 0 0 0
After:  [0, 2, 0, 1]

Before: [1, 1, 0, 1]
13 1 0 0
After:  [1, 1, 0, 1]

Before: [1, 0, 3, 3]
4 2 2 1
After:  [1, 5, 3, 3]

Before: [0, 1, 2, 2]
10 0 0 3
After:  [0, 1, 2, 0]

Before: [2, 2, 3, 1]
4 2 2 2
After:  [2, 2, 5, 1]

Before: [1, 3, 1, 3]
2 1 3 1
After:  [1, 1, 1, 3]

Before: [0, 1, 3, 3]
5 1 0 1
After:  [0, 1, 3, 3]

Before: [0, 0, 1, 1]
10 0 0 0
After:  [0, 0, 1, 1]

Before: [2, 0, 2, 3]
12 0 3 2
After:  [2, 0, 0, 3]

Before: [0, 2, 3, 1]
3 1 1 2
After:  [0, 2, 1, 1]

Before: [0, 3, 1, 0]
4 1 2 3
After:  [0, 3, 1, 5]

Before: [1, 2, 1, 3]
6 1 0 3
After:  [1, 2, 1, 1]

Before: [0, 1, 0, 0]
5 1 0 3
After:  [0, 1, 0, 1]

Before: [0, 0, 3, 2]
11 1 2 2
After:  [0, 0, 3, 2]

Before: [1, 1, 3, 3]
12 0 3 3
After:  [1, 1, 3, 0]

Before: [2, 3, 1, 2]
11 2 1 2
After:  [2, 3, 3, 2]

Before: [1, 1, 3, 3]
12 0 3 2
After:  [1, 1, 0, 3]

Before: [1, 2, 2, 2]
2 1 2 2
After:  [1, 2, 1, 2]

Before: [2, 3, 2, 2]
15 1 3 0
After:  [6, 3, 2, 2]

Before: [0, 0, 2, 1]
4 3 1 2
After:  [0, 0, 2, 1]

Before: [3, 3, 1, 3]
2 1 3 3
After:  [3, 3, 1, 1]

Before: [0, 1, 1, 3]
11 0 2 0
After:  [1, 1, 1, 3]

Before: [1, 1, 1, 3]
13 1 0 1
After:  [1, 1, 1, 3]

Before: [1, 3, 2, 1]
14 2 2 2
After:  [1, 3, 4, 1]

Before: [0, 1, 2, 2]
5 1 0 0
After:  [1, 1, 2, 2]

Before: [2, 2, 0, 0]
3 1 0 0
After:  [1, 2, 0, 0]

Before: [0, 2, 3, 1]
10 0 0 3
After:  [0, 2, 3, 0]

Before: [1, 3, 2, 3]
12 0 3 1
After:  [1, 0, 2, 3]

Before: [1, 3, 3, 1]
1 3 2 1
After:  [1, 3, 3, 1]

Before: [0, 2, 0, 3]
4 3 2 2
After:  [0, 2, 5, 3]

Before: [1, 2, 2, 1]
14 2 2 3
After:  [1, 2, 2, 4]

Before: [2, 3, 3, 1]
3 1 2 0
After:  [1, 3, 3, 1]

Before: [3, 2, 3, 2]
7 1 0 3
After:  [3, 2, 3, 1]

Before: [1, 0, 3, 2]
11 1 3 0
After:  [2, 0, 3, 2]

Before: [3, 0, 2, 1]
8 3 2 1
After:  [3, 2, 2, 1]

Before: [2, 1, 3, 2]
15 2 3 3
After:  [2, 1, 3, 6]

Before: [1, 2, 2, 0]
2 1 2 2
After:  [1, 2, 1, 0]

Before: [3, 3, 0, 2]
3 1 0 2
After:  [3, 3, 1, 2]

Before: [2, 2, 1, 1]
6 1 2 0
After:  [1, 2, 1, 1]

Before: [1, 0, 3, 3]
0 1 0 0
After:  [1, 0, 3, 3]

Before: [1, 1, 1, 0]
13 1 0 0
After:  [1, 1, 1, 0]

Before: [2, 2, 2, 1]
1 3 2 2
After:  [2, 2, 3, 1]

Before: [2, 2, 2, 1]
2 1 2 2
After:  [2, 2, 1, 1]

Before: [3, 1, 1, 1]
6 3 3 2
After:  [3, 1, 0, 1]

Before: [3, 1, 3, 3]
9 1 3 1
After:  [3, 3, 3, 3]

Before: [0, 2, 3, 2]
10 0 0 2
After:  [0, 2, 0, 2]

Before: [0, 3, 1, 1]
11 0 2 3
After:  [0, 3, 1, 1]

Before: [1, 0, 3, 1]
1 3 2 2
After:  [1, 0, 3, 1]

Before: [2, 1, 2, 2]
1 3 1 1
After:  [2, 3, 2, 2]

Before: [3, 2, 2, 3]
2 1 2 0
After:  [1, 2, 2, 3]

Before: [1, 0, 0, 2]
0 1 0 0
After:  [1, 0, 0, 2]

Before: [0, 1, 0, 1]
10 0 0 0
After:  [0, 1, 0, 1]

Before: [1, 0, 0, 3]
12 0 3 3
After:  [1, 0, 0, 0]

Before: [1, 1, 0, 3]
13 1 0 1
After:  [1, 1, 0, 3]

Before: [1, 0, 0, 3]
0 1 0 0
After:  [1, 0, 0, 3]

Before: [3, 2, 1, 3]
7 1 0 3
After:  [3, 2, 1, 1]

Before: [0, 3, 3, 3]
2 1 3 3
After:  [0, 3, 3, 1]

Before: [3, 2, 1, 2]
7 1 0 1
After:  [3, 1, 1, 2]

Before: [3, 2, 0, 1]
4 3 3 0
After:  [4, 2, 0, 1]

Before: [3, 2, 1, 0]
6 1 2 2
After:  [3, 2, 1, 0]

Before: [0, 1, 1, 2]
5 1 0 1
After:  [0, 1, 1, 2]

Before: [3, 3, 3, 3]
2 1 3 1
After:  [3, 1, 3, 3]

Before: [1, 3, 3, 2]
15 2 3 2
After:  [1, 3, 6, 2]

Before: [3, 2, 3, 1]
15 3 1 2
After:  [3, 2, 2, 1]

Before: [3, 3, 0, 3]
4 1 1 2
After:  [3, 3, 4, 3]

Before: [0, 0, 2, 3]
9 0 3 1
After:  [0, 3, 2, 3]

Before: [1, 3, 3, 1]
1 3 2 2
After:  [1, 3, 3, 1]

Before: [3, 0, 2, 2]
15 0 3 0
After:  [6, 0, 2, 2]

Before: [0, 1, 3, 3]
9 0 1 3
After:  [0, 1, 3, 1]

Before: [1, 3, 3, 2]
4 1 1 3
After:  [1, 3, 3, 4]

Before: [1, 0, 2, 2]
0 1 0 1
After:  [1, 1, 2, 2]

Before: [1, 2, 2, 1]
15 3 1 0
After:  [2, 2, 2, 1]

Before: [0, 1, 2, 1]
5 1 0 1
After:  [0, 1, 2, 1]

Before: [1, 2, 1, 1]
15 0 1 0
After:  [2, 2, 1, 1]

Before: [1, 1, 0, 3]
13 1 0 2
After:  [1, 1, 1, 3]

Before: [2, 1, 2, 3]
14 0 2 0
After:  [4, 1, 2, 3]

Before: [1, 3, 2, 3]
8 2 3 3
After:  [1, 3, 2, 6]

Before: [0, 1, 1, 1]
9 0 1 1
After:  [0, 1, 1, 1]

Before: [2, 0, 1, 2]
8 3 2 3
After:  [2, 0, 1, 4]

Before: [1, 0, 1, 1]
0 1 0 3
After:  [1, 0, 1, 1]

Before: [2, 3, 1, 1]
6 3 3 3
After:  [2, 3, 1, 0]

Before: [3, 2, 3, 3]
7 1 0 3
After:  [3, 2, 3, 1]

Before: [1, 3, 3, 3]
3 1 2 1
After:  [1, 1, 3, 3]

Before: [0, 1, 1, 3]
5 1 0 2
After:  [0, 1, 1, 3]

Before: [3, 3, 1, 3]
2 1 3 1
After:  [3, 1, 1, 3]

Before: [0, 1, 2, 0]
5 1 0 0
After:  [1, 1, 2, 0]

Before: [2, 1, 2, 0]
9 1 2 0
After:  [3, 1, 2, 0]

Before: [0, 0, 3, 1]
10 0 0 1
After:  [0, 0, 3, 1]

Before: [0, 1, 0, 2]
1 3 1 1
After:  [0, 3, 0, 2]

Before: [3, 0, 1, 0]
11 1 0 2
After:  [3, 0, 3, 0]

Before: [1, 2, 3, 1]
6 1 0 3
After:  [1, 2, 3, 1]

Before: [1, 0, 3, 0]
0 1 0 0
After:  [1, 0, 3, 0]

Before: [0, 2, 1, 0]
11 0 1 3
After:  [0, 2, 1, 2]

Before: [1, 1, 1, 0]
13 1 0 3
After:  [1, 1, 1, 1]

Before: [1, 2, 3, 1]
1 3 2 3
After:  [1, 2, 3, 3]

Before: [0, 1, 3, 0]
5 1 0 2
After:  [0, 1, 1, 0]

Before: [3, 0, 2, 2]
6 3 3 0
After:  [0, 0, 2, 2]

Before: [1, 2, 3, 1]
6 3 3 3
After:  [1, 2, 3, 0]

Before: [3, 2, 0, 1]
7 1 0 2
After:  [3, 2, 1, 1]

Before: [1, 1, 3, 2]
13 1 0 1
After:  [1, 1, 3, 2]

Before: [0, 0, 1, 2]
10 0 0 2
After:  [0, 0, 0, 2]

Before: [3, 2, 2, 2]
6 3 3 0
After:  [0, 2, 2, 2]

Before: [1, 0, 2, 0]
0 1 0 0
After:  [1, 0, 2, 0]

Before: [1, 0, 2, 2]
9 0 2 3
After:  [1, 0, 2, 3]

Before: [2, 0, 3, 3]
12 0 3 1
After:  [2, 0, 3, 3]

Before: [2, 3, 0, 3]
2 1 3 1
After:  [2, 1, 0, 3]

Before: [1, 3, 2, 2]
6 3 3 3
After:  [1, 3, 2, 0]

Before: [3, 1, 1, 1]
4 3 3 0
After:  [4, 1, 1, 1]

Before: [2, 1, 1, 0]
8 0 2 2
After:  [2, 1, 4, 0]

Before: [1, 0, 2, 3]
0 1 0 3
After:  [1, 0, 2, 1]

Before: [1, 2, 2, 1]
4 3 3 1
After:  [1, 4, 2, 1]

Before: [0, 3, 2, 1]
10 0 0 3
After:  [0, 3, 2, 0]

Before: [0, 1, 0, 2]
5 1 0 0
After:  [1, 1, 0, 2]

Before: [0, 1, 1, 1]
5 1 0 0
After:  [1, 1, 1, 1]

Before: [1, 2, 3, 3]
12 0 3 2
After:  [1, 2, 0, 3]

Before: [1, 3, 3, 1]
3 1 2 1
After:  [1, 1, 3, 1]

Before: [1, 0, 3, 0]
0 1 0 2
After:  [1, 0, 1, 0]

Before: [3, 2, 3, 0]
7 1 0 1
After:  [3, 1, 3, 0]

Before: [1, 1, 1, 1]
13 1 0 0
After:  [1, 1, 1, 1]

Before: [0, 0, 0, 3]
9 1 3 1
After:  [0, 3, 0, 3]

Before: [3, 1, 0, 0]
11 2 0 2
After:  [3, 1, 3, 0]

Before: [0, 0, 1, 3]
11 0 2 0
After:  [1, 0, 1, 3]

Before: [3, 3, 3, 2]
4 2 1 1
After:  [3, 4, 3, 2]

Before: [0, 3, 2, 3]
2 1 3 0
After:  [1, 3, 2, 3]

Before: [1, 1, 1, 3]
9 0 3 2
After:  [1, 1, 3, 3]

Before: [2, 2, 1, 1]
15 3 1 3
After:  [2, 2, 1, 2]

Before: [3, 2, 1, 3]
7 1 0 2
After:  [3, 2, 1, 3]

Before: [1, 1, 0, 1]
13 1 0 2
After:  [1, 1, 1, 1]

Before: [3, 2, 2, 3]
3 1 1 0
After:  [1, 2, 2, 3]

Before: [0, 1, 2, 3]
5 1 0 0
After:  [1, 1, 2, 3]

Before: [0, 1, 3, 3]
5 1 0 3
After:  [0, 1, 3, 1]

Before: [0, 3, 1, 2]
6 3 3 0
After:  [0, 3, 1, 2]

Before: [0, 2, 3, 1]
8 1 2 2
After:  [0, 2, 4, 1]

Before: [1, 0, 2, 3]
0 1 0 2
After:  [1, 0, 1, 3]

Before: [2, 1, 1, 3]
9 1 3 1
After:  [2, 3, 1, 3]

Before: [2, 3, 3, 3]
12 0 3 1
After:  [2, 0, 3, 3]

Before: [1, 2, 3, 3]
3 1 1 3
After:  [1, 2, 3, 1]

Before: [1, 2, 0, 2]
15 0 1 3
After:  [1, 2, 0, 2]

Before: [1, 0, 0, 0]
0 1 0 0
After:  [1, 0, 0, 0]

Before: [0, 1, 1, 1]
6 2 3 0
After:  [0, 1, 1, 1]

Before: [3, 2, 2, 0]
7 1 0 1
After:  [3, 1, 2, 0]

Before: [2, 2, 0, 3]
8 0 3 1
After:  [2, 6, 0, 3]

Before: [1, 1, 0, 3]
12 0 3 2
After:  [1, 1, 0, 3]

Before: [2, 3, 2, 3]
12 0 3 0
After:  [0, 3, 2, 3]

Before: [2, 0, 1, 1]
6 3 3 1
After:  [2, 0, 1, 1]

Before: [1, 2, 0, 1]
4 1 3 0
After:  [5, 2, 0, 1]

Before: [0, 3, 2, 3]
10 0 0 3
After:  [0, 3, 2, 0]

Before: [2, 1, 2, 3]
9 1 2 2
After:  [2, 1, 3, 3]

Before: [2, 1, 2, 0]
11 1 0 2
After:  [2, 1, 3, 0]

Before: [0, 2, 2, 2]
2 1 2 0
After:  [1, 2, 2, 2]

Before: [1, 3, 2, 3]
8 0 2 2
After:  [1, 3, 2, 3]

Before: [1, 0, 0, 3]
9 2 3 1
After:  [1, 3, 0, 3]

Before: [1, 0, 2, 2]
0 1 0 2
After:  [1, 0, 1, 2]

Before: [0, 1, 1, 2]
1 3 1 0
After:  [3, 1, 1, 2]

Before: [3, 1, 3, 1]
1 3 2 3
After:  [3, 1, 3, 3]

Before: [0, 1, 0, 3]
4 3 2 2
After:  [0, 1, 5, 3]

Before: [1, 2, 2, 3]
8 3 3 0
After:  [9, 2, 2, 3]

Before: [1, 2, 2, 2]
14 3 2 2
After:  [1, 2, 4, 2]

Before: [0, 3, 0, 3]
2 1 3 2
After:  [0, 3, 1, 3]

Before: [2, 2, 0, 0]
3 1 0 2
After:  [2, 2, 1, 0]

Before: [1, 2, 3, 2]
9 0 2 1
After:  [1, 3, 3, 2]

Before: [0, 3, 2, 2]
8 1 2 0
After:  [6, 3, 2, 2]

Before: [1, 3, 2, 3]
2 1 3 0
After:  [1, 3, 2, 3]

Before: [1, 0, 2, 1]
1 3 2 1
After:  [1, 3, 2, 1]

Before: [0, 2, 2, 3]
10 0 0 1
After:  [0, 0, 2, 3]

Before: [0, 2, 2, 3]
2 1 2 1
After:  [0, 1, 2, 3]

Before: [3, 1, 2, 1]
14 2 2 1
After:  [3, 4, 2, 1]

Before: [1, 0, 0, 1]
4 0 3 3
After:  [1, 0, 0, 4]

Before: [1, 1, 0, 2]
8 3 2 2
After:  [1, 1, 4, 2]

Before: [0, 1, 1, 1]
4 1 3 1
After:  [0, 4, 1, 1]

Before: [1, 3, 1, 2]
15 1 3 3
After:  [1, 3, 1, 6]

Before: [2, 2, 2, 2]
6 3 3 0
After:  [0, 2, 2, 2]

Before: [1, 2, 3, 3]
8 1 3 1
After:  [1, 6, 3, 3]

Before: [3, 2, 2, 1]
14 2 2 1
After:  [3, 4, 2, 1]

Before: [3, 1, 2, 3]
8 1 2 3
After:  [3, 1, 2, 2]

Before: [2, 0, 3, 3]
4 2 1 0
After:  [4, 0, 3, 3]

Before: [0, 1, 1, 3]
5 1 0 0
After:  [1, 1, 1, 3]

Before: [1, 2, 2, 1]
15 3 1 2
After:  [1, 2, 2, 1]

Before: [2, 3, 3, 3]
4 3 1 1
After:  [2, 4, 3, 3]

Before: [1, 3, 2, 1]
14 2 2 1
After:  [1, 4, 2, 1]

Before: [0, 1, 2, 3]
5 1 0 3
After:  [0, 1, 2, 1]

Before: [1, 1, 1, 2]
13 1 0 2
After:  [1, 1, 1, 2]

Before: [3, 2, 2, 0]
7 1 0 0
After:  [1, 2, 2, 0]

Before: [0, 1, 0, 2]
5 1 0 2
After:  [0, 1, 1, 2]

Before: [1, 0, 1, 3]
0 1 0 3
After:  [1, 0, 1, 1]

Before: [0, 3, 0, 3]
2 1 3 3
After:  [0, 3, 0, 1]

Before: [0, 2, 2, 1]
10 0 0 1
After:  [0, 0, 2, 1]

Before: [2, 2, 2, 0]
14 1 2 3
After:  [2, 2, 2, 4]

Before: [3, 1, 3, 2]
15 2 3 3
After:  [3, 1, 3, 6]

Before: [1, 1, 2, 0]
13 1 0 2
After:  [1, 1, 1, 0]

Before: [1, 0, 1, 1]
0 1 0 2
After:  [1, 0, 1, 1]

Before: [3, 2, 1, 2]
7 1 0 2
After:  [3, 2, 1, 2]

Before: [2, 3, 3, 3]
12 0 3 2
After:  [2, 3, 0, 3]

Before: [0, 1, 3, 3]
10 0 0 3
After:  [0, 1, 3, 0]

Before: [3, 1, 2, 2]
1 3 1 2
After:  [3, 1, 3, 2]

Before: [1, 3, 3, 1]
6 3 3 2
After:  [1, 3, 0, 1]

Before: [0, 2, 3, 0]
10 0 0 2
After:  [0, 2, 0, 0]

Before: [0, 2, 1, 3]
10 0 0 3
After:  [0, 2, 1, 0]

Before: [2, 1, 3, 2]
4 1 3 2
After:  [2, 1, 4, 2]

Before: [1, 0, 2, 0]
0 1 0 1
After:  [1, 1, 2, 0]

Before: [1, 3, 0, 3]
4 1 2 3
After:  [1, 3, 0, 5]

Before: [0, 1, 0, 3]
5 1 0 1
After:  [0, 1, 0, 3]

Before: [2, 3, 2, 1]
1 3 2 3
After:  [2, 3, 2, 3]

Before: [3, 2, 1, 1]
7 1 0 0
After:  [1, 2, 1, 1]

Before: [2, 3, 2, 0]
14 0 2 0
After:  [4, 3, 2, 0]

Before: [1, 0, 0, 3]
9 0 3 1
After:  [1, 3, 0, 3]

Before: [0, 3, 2, 1]
14 2 2 3
After:  [0, 3, 2, 4]

Before: [0, 1, 0, 1]
5 1 0 0
After:  [1, 1, 0, 1]

Before: [1, 3, 3, 2]
15 2 3 0
After:  [6, 3, 3, 2]

Before: [1, 1, 3, 0]
4 2 3 1
After:  [1, 6, 3, 0]

Before: [1, 2, 0, 1]
15 0 1 2
After:  [1, 2, 2, 1]

Before: [0, 1, 2, 0]
5 1 0 1
After:  [0, 1, 2, 0]

Before: [1, 2, 3, 1]
15 0 1 3
After:  [1, 2, 3, 2]

Before: [1, 1, 2, 2]
13 1 0 2
After:  [1, 1, 1, 2]

Before: [3, 2, 2, 2]
7 1 0 0
After:  [1, 2, 2, 2]

Before: [1, 1, 3, 1]
1 3 2 3
After:  [1, 1, 3, 3]

Before: [1, 0, 2, 0]
0 1 0 2
After:  [1, 0, 1, 0]

Before: [2, 2, 3, 3]
12 0 3 2
After:  [2, 2, 0, 3]

Before: [1, 0, 2, 3]
0 1 0 0
After:  [1, 0, 2, 3]

Before: [0, 1, 0, 3]
5 1 0 3
After:  [0, 1, 0, 1]

Before: [1, 2, 3, 0]
15 0 1 1
After:  [1, 2, 3, 0]

Before: [1, 2, 2, 0]
14 1 2 2
After:  [1, 2, 4, 0]



1 0 2 3
1 2 1 0
1 2 3 2
9 0 3 3
8 3 2 3
14 3 1 1
5 1 2 0
1 0 1 3
1 2 2 1
12 3 2 3
8 3 1 3
14 3 0 0
5 0 2 2
8 0 0 0
4 0 2 0
1 0 3 3
1 1 0 1
9 0 3 3
8 3 1 3
14 2 3 2
1 2 1 3
1 0 3 1
3 0 3 0
8 0 1 0
14 2 0 2
1 2 0 1
1 2 0 0
1 3 2 3
13 3 0 3
8 3 2 3
14 3 2 2
5 2 0 1
1 1 0 3
1 3 2 2
1 0 3 0
14 3 3 0
8 0 1 0
14 1 0 1
1 0 0 0
1 1 1 2
14 3 3 0
8 0 1 0
14 0 1 1
5 1 3 0
1 2 3 1
1 3 2 3
13 3 1 1
8 1 2 1
8 1 2 1
14 1 0 0
5 0 0 3
1 2 1 1
1 3 0 0
11 1 0 0
8 0 3 0
14 0 3 3
5 3 3 2
1 1 3 3
1 3 2 0
13 0 1 0
8 0 3 0
14 0 2 2
1 0 1 3
1 3 1 1
8 1 0 0
4 0 1 0
14 0 0 3
8 3 1 3
14 3 2 2
5 2 3 1
1 0 0 0
1 0 3 3
1 2 3 2
12 3 2 0
8 0 2 0
8 0 3 0
14 0 1 1
5 1 2 2
1 0 1 1
1 1 3 0
1 1 2 3
4 3 1 3
8 3 2 3
8 3 1 3
14 2 3 2
5 2 3 3
1 0 1 2
1 1 0 1
8 0 2 1
8 1 2 1
14 3 1 3
5 3 3 0
1 1 1 3
1 1 2 1
8 0 0 2
4 2 3 2
8 3 2 3
8 3 1 3
14 0 3 0
5 0 2 1
1 1 2 0
1 1 2 3
14 0 0 3
8 3 3 3
14 3 1 1
5 1 1 0
8 1 0 2
4 2 0 2
1 2 2 3
1 1 3 1
15 1 3 1
8 1 1 1
8 1 3 1
14 1 0 0
5 0 1 1
1 2 3 2
8 0 0 0
4 0 1 0
5 0 2 3
8 3 2 3
8 3 2 3
14 1 3 1
5 1 1 2
1 1 1 3
1 2 3 0
1 3 3 1
13 1 0 3
8 3 2 3
8 3 3 3
14 3 2 2
5 2 1 3
1 0 1 1
1 1 0 0
1 1 3 2
4 0 1 1
8 1 2 1
8 1 2 1
14 1 3 3
1 3 2 2
1 1 3 1
1 2 2 0
11 0 2 2
8 2 2 2
8 2 1 2
14 3 2 3
5 3 3 0
1 3 2 2
1 1 1 3
14 1 3 1
8 1 1 1
14 1 0 0
5 0 2 2
1 2 3 1
8 0 0 0
4 0 2 0
6 0 3 1
8 1 2 1
14 2 1 2
5 2 3 1
1 1 1 0
1 3 1 3
1 1 1 2
10 3 2 2
8 2 3 2
14 2 1 1
1 2 3 3
8 2 0 2
4 2 0 2
1 0 2 0
1 3 2 0
8 0 1 0
14 1 0 1
5 1 1 0
1 2 2 2
1 0 0 3
8 0 0 1
4 1 0 1
12 3 2 3
8 3 1 3
14 3 0 0
5 0 1 1
1 1 1 3
1 3 2 2
1 2 1 0
15 3 0 2
8 2 2 2
14 1 2 1
8 1 0 2
4 2 0 2
1 3 3 0
8 2 0 3
4 3 3 3
2 2 0 3
8 3 1 3
14 1 3 1
5 1 3 0
8 3 0 1
4 1 3 1
1 0 3 3
1 3 2 1
8 1 2 1
8 1 2 1
14 0 1 0
5 0 0 3
8 3 0 0
4 0 3 0
1 3 2 2
1 2 0 1
11 1 2 1
8 1 3 1
14 3 1 3
5 3 0 1
1 2 0 0
1 1 1 3
1 0 1 2
6 0 3 3
8 3 2 3
14 1 3 1
5 1 0 3
1 2 3 1
1 3 3 0
1 3 0 2
11 1 0 0
8 0 3 0
14 3 0 3
5 3 3 0
1 3 1 1
1 0 3 3
0 3 2 1
8 1 2 1
14 0 1 0
5 0 2 1
8 3 0 0
4 0 1 0
8 1 0 3
4 3 2 3
1 2 0 3
8 3 1 3
14 1 3 1
1 3 0 3
1 0 2 2
1 2 1 0
1 2 3 0
8 0 3 0
14 0 1 1
5 1 3 2
1 1 2 0
1 1 3 3
1 3 3 1
4 0 1 3
8 3 1 3
14 2 3 2
8 0 0 0
4 0 2 0
8 3 0 1
4 1 2 1
1 1 3 3
15 3 0 1
8 1 2 1
8 1 2 1
14 2 1 2
5 2 0 1
1 2 2 3
1 3 2 2
11 0 2 0
8 0 1 0
8 0 1 0
14 0 1 1
5 1 0 3
1 2 1 1
1 2 3 0
2 0 2 1
8 1 2 1
14 3 1 3
5 3 1 2
1 0 1 3
1 3 1 1
9 0 3 3
8 3 2 3
8 3 1 3
14 3 2 2
5 2 1 0
1 2 1 2
1 2 0 3
1 1 1 1
15 1 3 3
8 3 2 3
14 0 3 0
5 0 2 3
1 0 2 2
1 3 1 0
1 2 0 1
8 1 1 1
8 1 1 1
14 1 3 3
5 3 2 1
1 0 2 3
1 3 0 2
1 1 1 0
0 3 2 3
8 3 3 3
14 1 3 1
5 1 0 3
1 3 0 0
1 0 1 1
10 0 2 2
8 2 3 2
14 3 2 3
5 3 1 2
1 2 3 0
8 0 0 3
4 3 1 3
1 3 1 1
4 3 1 1
8 1 3 1
14 2 1 2
5 2 2 0
1 3 0 2
1 0 1 3
1 2 0 1
0 3 2 3
8 3 2 3
14 0 3 0
5 0 0 1
1 2 2 0
1 0 1 3
0 3 2 2
8 2 1 2
14 2 1 1
1 1 1 3
1 3 1 0
1 0 0 2
2 2 0 2
8 2 1 2
14 1 2 1
1 0 0 3
1 2 2 2
7 2 0 0
8 0 1 0
14 0 1 1
1 2 2 3
1 3 1 0
1 0 3 2
0 2 3 2
8 2 1 2
14 1 2 1
1 1 0 0
1 0 1 2
0 2 3 0
8 0 2 0
14 1 0 1
5 1 3 2
8 0 0 0
4 0 2 0
1 1 2 1
1 1 0 3
15 1 0 3
8 3 3 3
8 3 2 3
14 3 2 2
5 2 2 3
1 0 2 1
1 1 2 2
1 1 0 0
4 0 1 0
8 0 2 0
14 0 3 3
5 3 1 1
1 1 1 0
1 1 2 3
14 3 3 2
8 2 2 2
14 2 1 1
1 2 3 2
14 3 0 3
8 3 1 3
14 3 1 1
5 1 2 3
8 2 0 2
4 2 1 2
1 3 1 1
1 0 0 0
10 1 2 2
8 2 3 2
8 2 2 2
14 3 2 3
5 3 0 2
8 3 0 3
4 3 1 3
1 2 1 0
1 0 0 1
14 3 3 0
8 0 1 0
14 2 0 2
5 2 2 1
1 2 0 2
1 1 2 0
1 2 2 3
15 0 3 2
8 2 2 2
8 2 3 2
14 1 2 1
1 0 3 2
1 3 2 0
2 2 0 3
8 3 3 3
14 3 1 1
5 1 2 2
1 1 2 1
8 2 0 0
4 0 1 0
1 1 3 3
1 3 1 1
8 1 3 1
14 1 2 2
5 2 1 1
8 2 0 3
4 3 0 3
8 1 0 0
4 0 0 0
8 0 0 2
4 2 3 2
1 2 3 2
8 2 3 2
14 2 1 1
1 2 2 0
1 2 3 2
12 3 2 2
8 2 1 2
14 2 1 1
1 1 1 0
1 0 3 2
8 2 0 3
4 3 2 3
8 0 2 3
8 3 2 3
14 1 3 1
5 1 2 3
1 3 1 2
1 2 3 1
11 1 2 0
8 0 2 0
8 0 2 0
14 0 3 3
5 3 3 0
1 1 2 3
1 3 0 1
1 1 1 2
14 3 3 2
8 2 3 2
14 2 0 0
5 0 1 3
1 3 0 0
1 0 0 2
1 0 3 1
2 2 0 2
8 2 2 2
14 3 2 3
5 3 1 0
1 1 1 3
1 1 0 2
8 0 0 1
4 1 2 1
14 3 3 3
8 3 3 3
14 0 3 0
5 0 1 2
1 3 0 0
8 1 0 1
4 1 0 1
8 1 0 3
4 3 2 3
1 1 3 0
8 0 1 0
8 0 2 0
14 0 2 2
1 2 3 0
1 1 1 3
6 0 3 1
8 1 3 1
14 1 2 2
5 2 3 1
8 0 0 2
4 2 2 2
6 0 3 3
8 3 1 3
14 1 3 1
1 1 0 0
1 2 0 3
5 0 2 0
8 0 3 0
8 0 2 0
14 1 0 1
5 1 0 3
1 2 1 1
1 3 0 0
11 2 0 2
8 2 1 2
14 2 3 3
5 3 2 1
1 2 3 0
1 1 0 3
1 0 0 2
15 3 0 0
8 0 2 0
8 0 1 0
14 1 0 1
5 1 0 3
1 3 2 2
1 2 3 1
8 1 0 0
4 0 3 0
10 0 2 2
8 2 3 2
8 2 2 2
14 3 2 3
5 3 2 0
1 0 2 3
1 0 2 1
8 1 0 2
4 2 3 2
0 3 2 3
8 3 1 3
14 3 0 0
8 0 0 1
4 1 3 1
1 2 0 2
1 0 0 3
12 3 2 1
8 1 2 1
14 0 1 0
1 3 1 2
8 2 0 1
4 1 2 1
11 1 2 2
8 2 2 2
14 0 2 0
5 0 3 2
1 2 2 3
1 2 1 0
3 0 3 1
8 1 1 1
14 1 2 2
5 2 1 3
1 0 3 2
1 1 2 0
8 2 0 1
4 1 3 1
4 0 1 2
8 2 2 2
8 2 1 2
14 2 3 3
5 3 0 2
1 2 2 0
1 2 0 3
1 0 1 1
1 1 3 0
8 0 3 0
14 2 0 2
5 2 2 3
1 2 3 1
1 3 3 2
1 2 0 0
11 0 2 1
8 1 3 1
8 1 1 1
14 3 1 3
5 3 1 0
1 2 3 1
1 1 2 3
1 2 3 1
8 1 2 1
14 1 0 0
8 1 0 1
4 1 1 1
1 2 0 3
15 1 3 1
8 1 3 1
14 0 1 0
5 0 3 1
8 1 0 3
4 3 1 3
1 0 0 0
8 3 2 2
8 2 2 2
14 1 2 1
8 3 0 2
4 2 0 2
1 2 0 3
1 2 2 0
3 0 3 3
8 3 2 3
8 3 1 3
14 3 1 1
1 1 1 0
1 2 1 2
8 3 0 3
4 3 3 3
5 0 2 3
8 3 3 3
14 1 3 1
5 1 1 0
1 0 3 3
1 3 1 1
12 3 2 2
8 2 1 2
14 2 0 0
5 0 3 2
1 2 1 0
1 2 2 3
8 3 0 1
4 1 1 1
3 0 3 0
8 0 2 0
14 2 0 2
5 2 3 1
1 1 2 2
8 3 0 3
4 3 3 3
8 0 0 0
4 0 1 0
10 3 2 0
8 0 1 0
8 0 3 0
14 0 1 1
5 1 0 2
1 2 1 1
1 1 1 0
8 0 0 3
4 3 2 3
15 0 3 3
8 3 3 3
14 3 2 2
5 2 0 3
1 1 1 1
1 0 1 2
8 3 0 0
4 0 3 0
2 2 0 2
8 2 2 2
8 2 3 2
14 3 2 3
1 2 2 2
11 2 0 2
8 2 2 2
14 3 2 3
5 3 1 1
1 2 3 0
1 3 0 2
1 1 2 3
2 0 2 3
8 3 2 3
14 1 3 1
5 1 1 3
1 1 1 1
1 0 2 0
8 1 2 2
8 2 1 2
14 3 2 3
5 3 1 2
1 2 2 0
1 2 2 3
3 0 3 1
8 1 2 1
14 1 2 2
5 2 1 0
1 0 2 2
8 3 0 1
4 1 1 1
0 2 3 3
8 3 1 3
14 0 3 0
5 0 1 2
1 1 1 3
1 2 3 0
8 2 0 1
4 1 3 1
7 0 1 0
8 0 1 0
14 0 2 2
1 3 2 0
8 2 0 1
4 1 2 1
11 1 0 0
8 0 3 0
14 2 0 2
5 2 2 3
1 0 0 2
8 3 0 0
4 0 3 0
13 0 1 2
8 2 2 2
14 3 2 3
5 3 2 2
1 0 1 3
8 3 0 0
4 0 1 0
1 0 3 1
4 0 1 0
8 0 3 0
14 2 0 2
5 2 1 3
1 2 0 2
1 3 0 0
8 3 0 1
4 1 2 1
7 2 0 2
8 2 3 2
14 2 3 3
5 3 0 1
8 1 0 0
4 0 1 0
1 2 2 2
8 2 0 3
4 3 0 3
12 3 2 2
8 2 2 2
8 2 3 2
14 1 2 1
5 1 0 2
8 1 0 1
4 1 3 1
4 0 1 3
8 3 2 3
14 2 3 2
5 2 2 3
1 2 2 1
8 2 0 0
4 0 0 0
1 3 0 2
1 2 0 2
8 2 2 2
14 2 3 3
1 1 2 0
8 3 0 2
4 2 2 2
5 0 2 1
8 1 1 1
8 1 2 1
14 1 3 3
5 3 1 1
1 3 3 0
1 0 0 3
12 3 2 0
8 0 1 0
14 0 1 1
8 0 0 3
4 3 2 3
1 2 1 0
1 3 0 2
3 0 3 0
8 0 3 0
14 1 0 1
5 1 1 0
1 0 1 2
1 3 0 3
1 1 3 1
8 1 2 3
8 3 2 3
8 3 1 3
14 0 3 0
5 0 2 1
1 3 3 2
1 1 2 3
1 3 3 0
10 0 2 0
8 0 2 0
14 1 0 1
5 1 3 2
1 1 0 1
1 0 0 3
8 3 0 0
4 0 0 0
1 3 1 3
8 3 1 3
14 2 3 2
8 2 0 3
4 3 0 3
1 2 0 0
15 1 0 1
8 1 2 1
14 2 1 2
5 2 3 1
1 1 1 3
1 2 0 2
1 3 1 0
7 2 0 3
8 3 2 3
8 3 3 3
14 1 3 1
5 1 3 0
1 1 0 1
1 2 2 3
1 0 1 2
8 1 2 2
8 2 2 2
14 2 0 0
1 3 3 1
1 3 0 2
1 1 2 3
4 3 1 1
8 1 3 1
8 1 1 1
14 1 0 0
5 0 0 3
1 2 0 1
1 1 0 0
8 0 2 0
8 0 1 0
14 0 3 3
5 3 2 2
1 3 0 1
1 3 0 3
1 1 1 0
4 0 1 0
8 0 1 0
14 2 0 2
1 1 0 1
1 0 0 3
1 2 2 0
15 1 0 0
8 0 2 0
14 2 0 2
5 2 1 3
1 3 0 2
1 2 2 1
1 2 1 0
11 0 2 0
8 0 1 0
14 3 0 3
5 3 1 0
1 2 1 2
1 2 2 3
1 3 2 1
7 2 1 2
8 2 3 2
8 2 1 2
14 2 0 0
5 0 3 3
8 1 0 1
4 1 0 1
1 1 1 0
1 2 1 2
5 0 2 2
8 2 2 2
8 2 1 2
14 2 3 3
5 3 1 0
1 1 2 3
1 0 1 2
1 3 0 1
8 3 2 3
8 3 3 3
14 3 0 0
5 0 2 2
1 1 3 0
1 1 0 3
1 2 2 1
14 3 0 0
8 0 3 0
14 2 0 2
1 3 3 1
1 1 2 0
1 0 0 3
4 0 1 0
8 0 3 0
14 0 2 2
5 2 0 0
1 0 1 2
8 0 0 3
4 3 1 3
8 3 2 1
8 1 1 1
14 1 0 0
1 0 0 1
1 2 0 3
1 2 0 2
9 2 3 2
8 2 3 2
14 2 0 0
5 0 0 1
1 3 0 0
1 3 0 2
1 0 3 3
0 3 2 0
8 0 2 0
8 0 2 0
14 0 1 1
5 1 3 2
1 1 0 0
8 1 0 1
4 1 1 1
1 3 3 3
14 1 0 0
8 0 3 0
14 2 0 2
5 2 0 1
1 2 1 2
1 3 0 0
1 2 1 3
7 2 0 0
8 0 2 0
14 0 1 1
5 1 0 2
1 1 0 3
1 3 3 1
1 2 1 0
15 3 0 0
8 0 3 0
8 0 3 0
14 0 2 2
5 2 1 3
1 3 2 2
1 1 3 1
1 0 0 0
8 1 2 1
8 1 2 1
14 1 3 3
8 3 0 0
4 0 2 0
1 2 1 2
8 3 0 1
4 1 3 1
7 0 1 2
8 2 3 2
14 3 2 3
5 3 2 0
8 0 0 3
4 3 0 3
1 2 0 2
8 3 0 1
4 1 1 1
12 3 2 2
8 2 2 2
14 2 0 0
1 0 1 1
1 1 0 3
1 2 0 2
4 3 1 2
8 2 1 2
8 2 2 2
14 2 0 0
5 0 0 3
8 3 0 2
4 2 3 2
1 2 1 0
2 0 2 0
8 0 3 0
14 0 3 3
1 2 2 0
2 0 2 2
8 2 2 2
8 2 1 2
14 3 2 3
5 3 0 1
1 2 0 3
1 2 2 2
9 0 3 3
8 3 1 3
14 3 1 1
5 1 3 3
1 3 3 1
1 1 3 2
10 1 2 2
8 2 2 2
8 2 1 2
14 3 2 3
5 3 0 0";
        }
    }

    class Sample
    {
        public Dictionary<int, int> BeforeState { get; set; }
        public List<int> Instruction { get; set; }
        public Dictionary<int, int> AfterState { get; set; }
    }
}
