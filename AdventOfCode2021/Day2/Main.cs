using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day2
{
    internal class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = await ReadInput();
            var position = 0;
            var depth = 0;
            foreach (var instruction in input)
            {
                var parts = instruction.Split(' ');
                switch (parts[0])
                {
                    case "forward":
                        position += int.Parse(parts[1]);
                        break;
                    case "down":
                        depth += int.Parse(parts[1]);
                        break;
                    case "up":
                        depth -= int.Parse(parts[1]);
                        break;
                }
            }

            return (position * depth).ToString();
        }

        public new async Task<string> Part2()
        {
            var input = await ReadInput();
            var position = 0;
            var depth = 0;
            var aim = 0;
            foreach (var instruction in input)
            {
                var parts = instruction.Split(' ');
                var value = int.Parse(parts[1]);
                switch (parts[0])
                {
                    case "forward":
                        position += value;
                        depth += value * aim;
                        break;
                    case "down":
                        aim += value;
                        break;
                    case "up":
                        aim -= value;
                        break;
                }
            }

            return (position * depth).ToString();
        }

        private string[] GetSample()
        {
            var sample = @"forward 5
down 5
forward 8
up 3
down 8
forward 2";
            return sample.Split(Environment.NewLine);
        }
    }
}
