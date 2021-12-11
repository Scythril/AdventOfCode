using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day7
{
    internal class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            //var input = GetSample().Split(',').Select(x => int.Parse(x)).ToList();
            var input = (await ReadInputAsString()).Split(',').Select(x => int.Parse(x)).ToList();
            var positions = Enumerable.Range(input.Min(), input.Max()).ToDictionary(x => x, x => input.Sum(pos => Math.Abs(x - pos)));
            var minKvp = positions.MinBy(x => x.Value);
            return minKvp.Value.ToString();
        }

        public new async Task<string> Part2()
        {
            //var input = GetSample().Split(',').Select(x => int.Parse(x)).ToList();
            var input = (await ReadInputAsString()).Split(',').Select(x => int.Parse(x)).ToList();
            var positions = Enumerable.Range(input.Min(), input.Max()).ToDictionary(x => x, x => input.Sum(pos => { var fuelCost = Math.Abs(x - pos); return ((fuelCost * fuelCost) + fuelCost) / 2; }));
            var minKvp = positions.MinBy(x => x.Value);
            return minKvp.Value.ToString();
        }

        private string GetSample()
        {
            return @"16,1,2,0,4,2,7,1,2,14";
        }
    }
}
