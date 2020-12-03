using System;
using System.Collections.Generic;

namespace AdventOfCode2017
{
    public class Day23
    {
        public long Part1(string input)
        {
            var registers = new Dictionary<string, long>();
            var instructions = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            long mulInvoked = 0;
            for (long i = 0; i < instructions.Length && i >= 0; i++)
            {
                var inst = instructions[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (!Int64.TryParse(inst[1], out long result) && !registers.ContainsKey(inst[1]))
                    registers.Add(inst[1], 0);
                switch (inst[0])
                {
                    case "set":
                        registers[inst[1]] = Day18.GetParameter(inst[2], registers);
                        break;
                    case "sub":
                        registers[inst[1]] -= Day18.GetParameter(inst[2], registers);
                        break;
                    case "mul":
                        registers[inst[1]] *= Day18.GetParameter(inst[2], registers);
                        mulInvoked++;
                        break;
                    case "jnz":
                        if (Day18.GetParameter(inst[1], registers) == 0)
                            break;
                        i += Day18.GetParameter(inst[2], registers) - 1;
                        break;
                    default:
                        throw new ArgumentException(String.Format("Unknown instruction {0}", instructions[i]));
                }
            }
            return mulInvoked;
        }

        public static void Run()
        {
            var day23 = new Day23();
            Console.WriteLine("\n###############\n###############\nDay 23\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(day23.Part1(Input));
        }

        private static string Input
        {
            get
            {
                return @"set b 79
set c b
jnz a 2
jnz 1 5
mul b 100
sub b -100000
set c b
sub c -17000
set f 1
set d 2
set e 2
set g d
mul g e
sub g b
jnz g 2
set f 0
sub e -1
set g e
sub g b
jnz g -8
sub d -1
set g d
sub g b
jnz g -13
jnz f 2
sub h -1
set g b
sub g c
jnz g 2
jnz 1 3
sub b -17
jnz 1 -23";
            }
        }
    }
}
