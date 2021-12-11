using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day3
{
    internal class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = await ReadInput();
            var gamma = "";
            var epsilon = "";
            var onesCount = new int[input[0].Length];
            foreach (var val in input)
            {
                for (var i = 0; i < val.Length; i++)
                {
                    if (val[i] == '1')
                        onesCount[i]++;
                }
            }

            foreach (var i in onesCount)
            {
                if (i > input.Length / 2)
                {
                    gamma += "1";
                    epsilon += "0";
                }
                else
                {
                    gamma += "0";
                    epsilon += "1";
                }
            }

            var gammaRate = Convert.ToInt64(gamma, 2);
            var epsilonRate = Convert.ToInt64(epsilon, 2);
            return (gammaRate * epsilonRate).ToString();
        }

        public new async Task<string> Part2()
        {
            var input = await ReadInput();
            var o2Gen = GetO2GenRating(input.ToList(), 0)[0];
            var co2Scrub = GetCO2ScrubRating(input.ToList(), 0)[0];
            var o2GenRating = Convert.ToInt64(o2Gen, 2);
            var co2ScrubRating = Convert.ToInt64(co2Scrub, 2);
            return (o2GenRating * co2ScrubRating).ToString();
        }

        private List<string> GetO2GenRating(List<string> values, int position)
        {
            if (values.Count == 1)
                return values;

            var ones = new List<string>();
            var zeros = new List<string>();
            foreach (var val in values)
            {
                if (val[position] == '1')
                    ones.Add(val);
                else
                    zeros.Add(val);
            }

            var newList = (ones.Count >= zeros.Count) ? ones : zeros;
            return GetO2GenRating(newList, position + 1);
        }

        private List<string> GetCO2ScrubRating(List<string> values, int position)
        {
            if (values.Count == 1)
                return values;

            var ones = new List<string>();
            var zeros = new List<string>();
            foreach (var val in values)
            {
                if (val[position] == '1')
                    ones.Add(val);
                else
                    zeros.Add(val);
            }

            var newList = (zeros.Count <= ones.Count) ? zeros : ones;
            return GetCO2ScrubRating(newList, position + 1);
        }

        private string[] GetSample()
        {
            var sample = @"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010";
            return sample.Split(Environment.NewLine);
        }
    }
}
