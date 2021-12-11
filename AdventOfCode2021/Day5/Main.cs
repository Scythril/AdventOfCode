using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day5
{
    internal class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = await ReadInput();
            var mapSize = 1000;
            var map = GenerateMap(mapSize, input, false);
            //PrintMap(map);
            return map.SelectMany(x => x).Count(x => x > 1).ToString();
        }

        public new async Task<string> Part2()
        {
            var input = await ReadInput();
            var mapSize = 1000;
            var map = GenerateMap(mapSize, input, true);
            //PrintMap(map);
            return map.SelectMany(x => x).Count(x => x > 1).ToString();
        }

        private List<List<int>> GenerateMap(int mapSize, string[] input, bool includeDiagonal)
        {
            var map = new List<List<int>>();
            for (var i = 0; i < mapSize; i++)
            {
                var row = new List<int>();
                for (var j = 0; j < mapSize; j++)
                {
                    row.Add(0);
                }

                map.Add(row);
            }

            foreach (var vent in input)
            {
                var parts = vent.Split(" -> ");
                var start = parts[0].Split(',').Select(x => int.Parse(x)).ToList();
                var end = parts[1].Split(',').Select(x => int.Parse(x)).ToList();
                // horizontal
                if (start[1] == end[1])
                {
                    var first = start[0] < end[0] ? start[0] : end[0];
                    var last = start[0] < end[0] ? end[0] : start[0];
                    for (var i = first; i <= last; i++)
                        map[start[1]][i]++;
                }
                // vertical
                else if (start[0] == end[0])
                {
                    var first = start[1] < end[1] ? start[1] : end[1];
                    var last = start[1] < end[1] ? end[1] : start[1];
                    for (var i = first; i <= last; i++)
                    {
                        map[i][start[0]]++;
                    }
                }
                // diagonal
                else if (includeDiagonal)
                {
                    var points = Math.Abs(start[0] - end[0]) + 1;
                    var curX = start[0];
                    var curY = start[1];
                    for (var i = 0; i < points; i++)
                    {
                        curX = (start[0] < end[0]) ? start[0] + i : start[0] - i;
                        curY = (start[1] < end[1]) ? start[1] + i : start[1] - i;
                        map[curY][curX]++;
                    }
                }
            }

            return map;
        }

        private void PrintMap(List<List<int>> map)
        {
            foreach (var row in map)
            {
                foreach (var point in row)
                {
                    if (point == 0)
                        Console.Write('.');
                    else
                        Console.Write(point.ToString());
                }

                Console.WriteLine();
            }
        }

        private string[] GetSample()
        {
            var sample = @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";
            return sample.Split(Environment.NewLine);
        }
    }
}
