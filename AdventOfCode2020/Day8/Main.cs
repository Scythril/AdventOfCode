using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day8
{
    class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = await ReadInput();
            var handler = new InstructionHandler(input);
            handler.Run();
            return handler.Accumulator.ToString();
        }

        public new async Task<string> Part2()
        {
            var input = await ReadInput();
            var linesToSwitch = new List<int>();
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i].StartsWith("jmp") || input[i].StartsWith("nop"))
                    linesToSwitch.Add(i);
            }

            InstructionHandler handler = null;

            foreach (var line in linesToSwitch)
            {
                var newInput = (string[])input.Clone();
                if (newInput[line].StartsWith("jmp"))
                    newInput[line] = newInput[line].Replace("jmp", "nop");
                else
                    newInput[line] = newInput[line].Replace("nop", "jmp");

                handler = new InstructionHandler(newInput);
                handler.Run();
                if (handler.RanToCompletion)
                    break;
            }

            if (!handler.RanToCompletion)
                throw new Exception("None of the switches worked!");

            return handler?.Accumulator.ToString();
        }

        private string[] GetSample()
        {
            return @"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6".Split(Environment.NewLine);
        }
    }
}
