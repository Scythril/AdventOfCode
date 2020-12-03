using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    class Day7 : BaseDay
    {
        public Day7()
        {
            Sample = @"3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10";

            Input = @"3,8,1001,8,10,8,105,1,0,0,21,30,55,80,101,118,199,280,361,442,99999,3,9,101,4,9,9,4,9,99,3,9,101,4,9,9,1002,9,4,9,101,4,9,9,1002,9,5,9,1001,9,2,9,4,9,99,3,9,101,5,9,9,1002,9,2,9,101,3,9,9,102,4,9,9,1001,9,2,9,4,9,99,3,9,102,2,9,9,101,5,9,9,102,3,9,9,101,3,9,9,4,9,99,3,9,1001,9,2,9,102,4,9,9,1001,9,3,9,4,9,99,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,101,2,9,9,4,9,3,9,101,2,9,9,4,9,99,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,1,9,4,9,3,9,102,2,9,9,4,9,99,3,9,1001,9,1,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,1002,9,2,9,4,9,3,9,102,2,9,9,4,9,99,3,9,1001,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,101,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,101,2,9,9,4,9,99,3,9,1001,9,2,9,4,9,3,9,101,2,9,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1002,9,2,9,4,9,3,9,101,1,9,9,4,9,3,9,1001,9,2,9,4,9,3,9,1001,9,2,9,4,9,3,9,102,2,9,9,4,9,3,9,102,2,9,9,4,9,99";
        }

        public new string Part1()
        {
            var numAmplifiers = 5;
            var maxOutput = long.MinValue;
            foreach (var phaseSettings in GetPermutations(Enumerable.Range(0, numAmplifiers)))
            {
                var input = 0L;
                foreach (var phaseSetting in phaseSettings)
                {
                    var computer = new IntCodeComputer(Input, phaseSetting);
                    computer.RunInstruction();
                    computer.Input = input;
                    input = computer.RunToOutput().Value;
                }

                if (input > maxOutput)
                    maxOutput = input;
            }

            return maxOutput.ToString();
        }

        public new string Part2()
        {
            var numAmplifiers = 5;
            var maxOutput = long.MinValue;
            foreach (var phaseSettings in GetPermutations(Enumerable.Range(5, numAmplifiers)))
            {
                var computers = new List<IntCodeComputer>(numAmplifiers);
                foreach (var phaseSetting in phaseSettings)
                {
                    var computer = new IntCodeComputer(Input, phaseSetting);
                    computer.RunInstruction();
                    computers.Add(computer);
                }

                var input = 0L;
                var compIdx = 0;
                while (computers.Any(c => c.State != State.Halt))
                {
                    computers[compIdx].Input = input;
                    var output = computers[compIdx].RunToOutput();
                    if (output.HasValue)
                        input = output.Value;

                    if (compIdx == computers.Count() - 1)
                        compIdx = 0;
                    else
                        compIdx++;
                }

                if (input > maxOutput)
                    maxOutput = input;
            }

            return maxOutput.ToString();
        }

        private IEnumerable<IEnumerable<int>> GetPermutations(IEnumerable<int> source)
        {
            if (source.Count() == 1)
                return new List<IEnumerable<int>> { source };

            var permutations = from c in source
                               from p in GetPermutations(source.Where(x => x != c))
                               select new List<int> { c }.Concat(p);

            return permutations;
        }
    }
}
