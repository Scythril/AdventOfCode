using System.Drawing;

namespace AdventOfCode2021.Day11
{
    internal class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = (await ReadInput()).Select(x => x.Select(x => int.Parse(x.ToString())).ToList()).ToList();
            var steps = 100;
            var totalFlashes = 0;
            for (var i = 0; i < steps; i++)
            {
                var flashed = new List<Point>();
                var needsFlash = new Queue<Point>();

                // Increase energy level of each octopus by 1
                for (var y = 0; y < input.Count; y++)
                {
                    for (var x = 0; x < input[y].Count; x++)
                    {
                        input[y][x]++;
                        if (input[y][x] == 10)
                            needsFlash.Enqueue(new Point(x, y));
                    }
                }

                // If any energy level is >10, it flashes and increases energy level of adjacent octopi
                while (needsFlash.Count > 0)
                {
                    var octopus = needsFlash.Dequeue();
                    flashed.Add(octopus);
                    var newFlashes = Flash(input, octopus);
                    foreach (var f in newFlashes)
                    {
                        if (!needsFlash.Contains(f))
                            needsFlash.Enqueue(f);
                    }
                }

                // Any octopi that flashed should be set to 0
                foreach (var point in flashed)
                    input[point.Y][point.X] = 0;

                totalFlashes += flashed.Count;
            }

            return totalFlashes.ToString();
        }

        public new async Task<string> Part2()
        {
            var input = (await ReadInput()).Select(x => x.Select(x => int.Parse(x.ToString())).ToList()).ToList();
            var stepCount = 0;

            while (true)
            {
                stepCount++;
                var flashed = new List<Point>();
                var needsFlash = new Queue<Point>();

                // Increase energy level of each octopus by 1
                for (var y = 0; y < input.Count; y++)
                {
                    for (var x = 0; x < input[y].Count; x++)
                    {
                        input[y][x]++;
                        if (input[y][x] == 10)
                            needsFlash.Enqueue(new Point(x, y));
                    }
                }

                // If any energy level is >10, it flashes and increases energy level of adjacent octopi
                while (needsFlash.Count > 0)
                {
                    var octopus = needsFlash.Dequeue();
                    flashed.Add(octopus);
                    var newFlashes = Flash(input, octopus);
                    foreach (var f in newFlashes)
                    {
                        if (!needsFlash.Contains(f))
                            needsFlash.Enqueue(f);
                    }
                }

                // Any octopi that flashed should be set to 0
                foreach (var point in flashed)
                    input[point.Y][point.X] = 0;

                if (flashed.Count == input.Count * input[0].Count)
                    break;
            }

            return stepCount.ToString();
        }

        private List<Point> Flash(List<List<int>> input, Point point)
        {
            var nextFlashes = new List<Point>();
            for (var y = point.Y - 1; y <= point.Y + 1; y++)
            {
                for (var x = point.X - 1; x <= point.X + 1; x++)
                {
                    if (y < 0 || y >= input.Count || x < 0 || x >= input[y].Count || (y == point.Y && x == point.X))
                        continue;

                    input[y][x]++;
                    if (input[y][x] == 10)
                        nextFlashes.Add(new Point(x, y));
                }
            }

            return nextFlashes;
        }

        private string[] GetSample()
        {
            var sample = @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";
            return sample.Split(Environment.NewLine);
        }
    }
}
