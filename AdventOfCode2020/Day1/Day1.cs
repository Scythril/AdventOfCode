using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day1
{
    class Day1 : BaseClass
    {
        public async Task<string> Part1()
        {
            var input = (await ReadInput()).Select(x => int.Parse(x)).ToList();
            for (var pointer = 0; pointer < input.Count; pointer++)
            {
                for (var counter = pointer + 1; counter < input.Count; counter++)
                {
                    if (input[pointer] + input[counter] == 2020)
                    {
                        return (input[pointer] * input[counter]).ToString();
                    }
                }
            }

            return "No answer.";
        }

        public async Task<string> Part2()
        {
            var input = (await ReadInput()).Select(x => long.Parse(x)).ToList();
            for (var i = 0; i < input.Count; i++)
            {
                for (var j = i + 1; j < input.Count; j++)
                {
                    for (var k = i + 1; k < input.Count; k++)
                    {
                        if (input[i] + input[j] + input[k] == 2020)
                        {
                            return (input[i] * input[j] * input[k]).ToString();
                        }
                    }
                }
            }

            return "No answer.";
        }
    }
}
