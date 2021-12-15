using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day9
{
    internal class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            //var input = GetSample().Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList()).ToList();
            var input = (await ReadInput()).Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList()).ToList();
            var sum = 0;
            for (var y = 0; y < input.Count; y++)
            {
                for (var x = 0; x < input[y].Count; x++)
                {
                    // left
                    if (x > 0 && input[y][x] >= input[y][x - 1])
                        continue;

                    // right
                    if (x < input[y].Count - 1 && input[y][x] >= input[y][x + 1])
                        continue;

                    // top
                    if (y > 0 && input[y][x] >= input[y - 1][x])
                        continue;

                    // bottom
                    if (y < input.Count - 1 && input[y][x] >= input[y + 1][x])
                        continue;

                    sum += input[y][x] + 1;
                }
            }

            return sum.ToString();
        }

        public new async Task<string> Part2()
        {
            //var input = GetSample().Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList()).ToList();
            var input = (await ReadInput()).Select(x => x.ToCharArray().Select(y => int.Parse(y.ToString())).ToList()).ToList();
            var lowPoints = new List<Point>();
            for (var y = 0; y < input.Count; y++)
            {
                for (var x = 0; x < input[y].Count; x++)
                {
                    // left
                    if (x > 0 && input[y][x] >= input[y][x - 1])
                        continue;

                    // right
                    if (x < input[y].Count - 1 && input[y][x] >= input[y][x + 1])
                        continue;

                    // top
                    if (y > 0 && input[y][x] >= input[y - 1][x])
                        continue;

                    // bottom
                    if (y < input.Count - 1 && input[y][x] >= input[y + 1][x])
                        continue;

                    lowPoints.Add(new Point(x, y));
                }
            }

            var basinSizes = new List<int>();
            foreach (var point in lowPoints)
            {
                basinSizes.Add(GetBasinSize(input, new List<Point>(), point));
            }

            var largestBasins = basinSizes.OrderByDescending(x => x).Take(3).ToList();
            var product = largestBasins[0] * largestBasins[1] * largestBasins[2];
            return product.ToString();
        }

        private int GetBasinSize(List<List<int>> input, List<Point> counted, Point currentPoint)
        {
            if (counted.Contains(currentPoint)
                || currentPoint.Y < 0
                || currentPoint.Y >= input.Count
                || currentPoint.X < 0
                || currentPoint.X >= input[currentPoint.Y].Count
                || input[currentPoint.Y][currentPoint.X] == 9)
                return 0;

            counted.Add(currentPoint);
            // left
            GetBasinSize(input, counted, new Point(currentPoint.X - 1, currentPoint.Y));

            // right
            GetBasinSize(input, counted, new Point(currentPoint.X + 1, currentPoint.Y));

            // top
            GetBasinSize(input, counted, new Point(currentPoint.X, currentPoint.Y - 1));

            // bottom
            GetBasinSize(input, counted, new Point(currentPoint.X, currentPoint.Y + 1));

            return counted.Count;
        }

        private string[] GetSample()
        {
            var sample = @"2199943210
3987894921
9856789892
8767896789
9899965678";
            return sample.Split(Environment.NewLine);
        }
    }
}
