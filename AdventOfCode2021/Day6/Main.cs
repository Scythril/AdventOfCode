using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day6
{
    internal class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = (await ReadInputAsString()).Split(',').Select(x => int.Parse(x)).ToList();
            var days = 80;
            var fish = SimulateFish(input, days);
            return fish.Sum(x => x.Value).ToString();
        }

        public new async Task<string> Part2()
        {
            //var input = GetSample().Split(',').Select(x => int.Parse(x)).ToList();
            var input = (await ReadInputAsString()).Split(',').Select(x => int.Parse(x)).ToList();
            var days = 256;
            var fish = SimulateFish(input, days);
            return fish.Sum(x => x.Value).ToString();
        }

        private Dictionary<int, long> SimulateFish(List<int> input, int days)
        {
            var maxTimer = 8;
            var fish = new Dictionary<int, long>(maxTimer + 1);
            for (var i = 0; i <= maxTimer; i++)
                fish.Add(i, input.Count(x => x == i));

            for (var day = 0; day < days; day++)
            {
                //LogMessage($"Day {day}");
                var newFish = fish[0];
                for (var i = 0; i < maxTimer; i++)
                    fish[i] = fish[i + 1];

                fish[8] = newFish;
                fish[6] += newFish;
            }

            return fish;
        }

        private string GetSample()
        {
            return @"3,4,3,1,2";
        }
    }
}
