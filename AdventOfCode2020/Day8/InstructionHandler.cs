using System;
using System.Collections.Generic;

namespace AdventOfCode2020.Day8
{
    class InstructionHandler
    {
        private string[] Instructions { get; set; }
        public long Accumulator { get; private set; }
        private int InstPtr { get; set; }
        private List<int> PrevInst { get; set; }

        public InstructionHandler(string[] instructions)
        {
            Instructions = instructions;
            Accumulator = 0;
            InstPtr = 0;
            PrevInst = new List<int>();
        }

        public void Run()
        {
            while (InstPtr < Instructions.Length)
            {
                if (PrevInst.Contains(InstPtr))
                    break;

                PrevInst.Add(InstPtr);
                var parts = Instructions[InstPtr].Split(' ');
                switch (parts[0])
                {
                    case "acc":
                        Accumulate(int.Parse(parts[1]));
                        InstPtr++;
                        break;
                    case "jmp":
                        Jump(int.Parse(parts[1]));
                        break;
                    case "nop":
                        InstPtr++;
                        break;
                    default:
                        throw new ArgumentException($"Unknown instruction found on line {InstPtr}: {parts[0]}");
                }
            }
        }

        public bool RanToCompletion => InstPtr >= Instructions.Length;

        private void Accumulate(int val) => Accumulator += val;

        private void Jump(int val) => InstPtr += val;
    }
}
