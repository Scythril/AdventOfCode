using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day9
{
    class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = (await ReadInput()).Select(l => long.Parse(l)).ToList();
            return FindInvalidNumber(input, 25).ToString();
        }

        public new async Task<string> Part2()
        {
            var input = GetSample().Select(l => long.Parse(l)).ToList();
            var invalidNumber = FindInvalidNumber(input, 5);
            for (var i = 0; i < input.Count; i++)
            {

            }

            return null;
        }

        private long FindInvalidNumber(List<long> input, int preambleLength)
        {
            for (var i = preambleLength; i < input.Count; i++)
            {
                var found = false;
                var prevNums = input.GetRange(i - preambleLength, preambleLength);
                foreach (var first in prevNums)
                {
                    foreach (var second in prevNums)
                    {
                        if (first == second)
                            continue;

                        if (first + second == input[i])
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                        break;
                }

                if (!found)
                    return input[i];
            }

            throw new Exception("Could not find the invalid number.");
        }

        private string[] GetSample()
        {
            return @"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576".Split(Environment.NewLine);
        }
    }
}
