using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day3
{
    class Main : BaseClass
    {
        const char TREE = '#';

        public new async Task<string> Part1()
        {
            var input = await ReadInput();
            return RunSlope(input).ToString();
        }

        public new async Task<string> Part2()
        {
            var input = await ReadInput();
            var slopes = new List<Point>
            {
                new Point(1, 1),
                new Point(3, 1),
                new Point(5, 1),
                new Point(7, 1),
                new Point(1, 2)
            };

            var treeMultiplier = 1L;
            foreach (var slope in slopes)
                treeMultiplier *= RunSlope(input, slope.X, slope.Y);

            return treeMultiplier.ToString();
        }

        private int RunSlope(string[] input, int x = 3, int y = 1)
        {
            var rightWall = input[0].Length;
            var treesHit = 0;
            var currentPoint = new Point(0, 0);
            while (currentPoint.Y < input.Length)
            {
                Console.Write($"Point {currentPoint.Y}, {currentPoint.X}: ");
                if (input[currentPoint.Y][currentPoint.X] == TREE)
                {
                    Console.WriteLine("HIT!");
                    treesHit++;
                }
                else
                    Console.WriteLine("MISS!");

                currentPoint.X = (currentPoint.X + x) % rightWall;
                currentPoint.Y += y;
            }

            Console.WriteLine($"{treesHit} trees hit!");
            return treesHit;
        }

        private string[] GetSample()
        {
            return @"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#".Split(Environment.NewLine);
        }
    }
}
