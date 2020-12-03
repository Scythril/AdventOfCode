using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2019
{
    class Day10 : BaseDay
    {
        public Day10()
        {
            Sample = @".#..##.###...#######
##.############..##.
.#.######.########.#
.###.#######.####.#.
#####.##.#.##.###.##
..#####..#.#########
####################
#.####....###.#.#.##
##.#################
#####.##.###..####..
..######..##.#######
####.##.####...##..#
.#####..#.######.###
##...#.##########...
#.##########.#######
.####.#.###.###.#.##
....##.##.###..#####
.#.#.###########.###
#.#.#.#####.####.###
###.##.####.##.#..##";

            Input = @".#..#..#..#...#..#...###....##.#....
.#.........#.#....#...........####.#
#..##.##.#....#...#.#....#..........
......###..#.#...............#.....#
......#......#....#..##....##.......
....................#..............#
..#....##...#.....#..#..........#..#
..#.#.....#..#..#..#.#....#.###.##.#
.........##.#..#.......#.........#..
.##..#..##....#.#...#.#.####.....#..
.##....#.#....#.......#......##....#
..#...#.#...##......#####..#......#.
##..#...#.....#...###..#..........#.
......##..#.##..#.....#.......##..#.
#..##..#..#.....#.#.####........#.#.
#......#..........###...#..#....##..
.......#...#....#.##.#..##......#...
.............##.......#.#.#..#...##.
..#..##...#...............#..#......
##....#...#.#....#..#.....##..##....
.#...##...........#..#..............
.............#....###...#.##....#.#.
#..#.#..#...#....#.....#............
....#.###....##....##...............
....#..........#..#..#.......#.#....
#..#....##.....#............#..#....
...##.............#...#.....#..###..
...#.......#........###.##..#..##.##
.#.##.#...##..#.#........#.....#....
#......#....#......#....###.#.....#.
......#.##......#...#.#.##.##...#...
..#...#.#........#....#...........#.
......#.##..#..#.....#......##..#...
..##.........#......#..##.#.#.......
.#....#..#....###..#....##..........
..............#....##...#.####...##.";
        }

        public new string Part1()
        {
            var asteroids = new List<Asteroid>();
            var lines = Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '.')
                        continue;

                    asteroids.Add(new Asteroid(x, y));
                }
            }

            foreach (var mainAst in asteroids)
            {
                var directions = new HashSet<double>();
                foreach (var compAst in asteroids)
                {
                    if (mainAst == compAst)
                        continue;

                    var dir = GetAngle(mainAst, compAst);
                    directions.Add(dir);
                }

                mainAst.VisibleAsteroids = directions.Count;
            }

            var best = asteroids.OrderByDescending(a => a.VisibleAsteroids).First();
            //return $"{best.Location.X},{best.Location.Y}";
            return best.VisibleAsteroids.ToString();
        }

        public new string Part2()
        {
            var asteroids = new List<Asteroid>();
            var lines = Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            for (var y = 0; y < lines.Length; y++)
            {
                for (var x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '.')
                        continue;

                    asteroids.Add(new Asteroid(x, y));
                }
            }

            foreach (var mainAst in asteroids)
            {
                var directions = new HashSet<double>();
                foreach (var compAst in asteroids)
                {
                    if (mainAst == compAst)
                        continue;

                    var dir = GetAngle(mainAst, compAst);
                    directions.Add(dir);
                }

                mainAst.VisibleAsteroids = directions.Count;
            }

            var best = asteroids.OrderByDescending(a => a.VisibleAsteroids).First();
            //best = asteroids.First(a => a.Location.X == 8 && a.Location.Y == 3);
            var astMap = new SortedDictionary<double, List<Asteroid>>();
            foreach (var asteroid in asteroids)
            {
                if (asteroid == best)
                    continue;

                var dir = GetAngle(best, asteroid);
                if (astMap.ContainsKey(dir))
                    astMap[dir].Add(asteroid);
                else
                    astMap[dir] = new List<Asteroid> { asteroid };
            }

            var possibleDirs = astMap.Keys.OrderBy(k => k).ToList();
            var dirIdx = 0;
            Asteroid targetDestroyed = null;
            for (var count = 1; count <= 200; count++)
            {
                targetDestroyed = astMap[possibleDirs[dirIdx]].OrderBy(a => GetDistance(best, a)).First();
                astMap[possibleDirs[dirIdx]].Remove(targetDestroyed);
                if (astMap[possibleDirs[dirIdx]].Count < 1)
                {
                    astMap.Remove(possibleDirs[dirIdx]);
                    possibleDirs.RemoveAt(dirIdx);
                }
                else
                    dirIdx++;
                
                if (dirIdx == possibleDirs.Count() - 1)
                    dirIdx = 0;
            }

            return $"{targetDestroyed.Location.X * 100 + targetDestroyed.Location.Y}";
        }

        private static double GetAngle(Asteroid asteroid1, Asteroid asteroid2)
        {
            double deltaX = asteroid2.Location.X - asteroid1.Location.X;
            double deltaY = asteroid1.Location.Y - asteroid2.Location.Y;
            var rads = Math.Atan2(deltaX, deltaY);
            var result = (180 / Math.PI) * rads;
            if (result < 0)
                return 360d + result;
            else if (result >= 360d)
                return result - 360d;

            return result;
        }

        private static int GetDistance(Asteroid asteroid1, Asteroid asteroid2)
        {
            return Math.Abs(asteroid1.Location.X - asteroid2.Location.X) + Math.Abs(asteroid1.Location.Y - asteroid2.Location.Y);
        }
    }

    class Asteroid
    {
        public Point Location { get; private set; }
        public int VisibleAsteroids { get; set; }

        public Asteroid(int x, int y)
        {
            Location = new Point(x, y);
        }
    }
}
