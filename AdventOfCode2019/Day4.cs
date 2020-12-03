using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    class Day4 : BaseDay
    {
        public Day4()
        {
            Input = @"134564-585159";
        }

        public new string Part1()
        {
            var range = Input.Split('-').Select(x => int.Parse(x)).ToList();
            var min = range[0];
            var max = range[1];
            var matches = new List<int>();

            for (var i = min; i <= max; i++)
            {
                var numString = i.ToString();

                var adjacents = false;
                var increasing = true;
                for (var j = 1; increasing && j < numString.Length; j++)
                {
                    var prevChar = numString[j - 1];
                    var currChar = numString[j];

                    if (!adjacents && (currChar == prevChar))
                        adjacents = true;

                    if (currChar < prevChar)
                        increasing = false;
                }

                if (adjacents && increasing)
                    matches.Add(i);
            }

            return matches.Count.ToString();
        }

        public new string Part2()
        {
            var range = Input.Split('-').Select(x => int.Parse(x)).ToList();
            var min = range[0];
            var max = range[1];
            var matches = new List<int>();

            for (var i = min; i <= max; i++)
            {
                var numString = i.ToString();

                var adjacents = false;
                var increasing = true;
                for (var j = 1; increasing && j < numString.Length; j++)
                {
                    var prevChar = numString[j - 1];
                    var currChar = numString[j];

                    if (!adjacents && (currChar == prevChar))
                    {
                        var left = new char();
                        var right = new char();
                        if (j > 1)
                            left = numString[j - 2];
                        if (j < numString.Length - 1)
                            right = numString[j + 1];

                        adjacents = left != currChar && right != currChar;
                    }

                    if (currChar < prevChar)
                        increasing = false;
                }

                if (adjacents && increasing)
                    matches.Add(i);
            }

            return matches.Count.ToString();
        }
    }
}
