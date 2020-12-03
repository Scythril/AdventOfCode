using System;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode2017
{
    public class Day22
    {
        private Dictionary<Point, State> _points;

        public int Part1(string map, int bursts)
        {
            SetPoints(map);
            var curPos = new Point(0, 0);
            var curDir = Direction.UP;
            var infectionCount = 0;
            for (int i = 0; i < bursts; i++)
            {
                curDir = _points[curPos] == State.INFECTED ? TurnRight(curDir) : TurnLeft(curDir);
                if (_points[curPos] == State.CLEAN)
                {
                    infectionCount++;
                    _points[curPos] = State.INFECTED;
                }
                else
                {
                    _points[curPos] = State.CLEAN;
                }
                switch (curDir)
                {
                    case Direction.UP:
                        curPos.Y++;
                        break;
                    case Direction.LEFT:
                        curPos.X--;
                        break;
                    case Direction.DOWN:
                        curPos.Y--;
                        break;
                    case Direction.RIGHT:
                        curPos.X++;
                        break;
                }
                if (!_points.ContainsKey(curPos))
                    _points.Add(curPos, State.CLEAN);
            }
            return infectionCount;
        }

        public int Part2(string map, int bursts)
        {
            SetPoints(map);
            var curPos = new Point(0, 0);
            var curDir = Direction.UP;
            var infectionCount = 0;
            for (int i = 0; i < bursts; i++)
            {
                switch (_points[curPos])
                {
                    case State.CLEAN:
                        curDir = TurnLeft(curDir);
                        _points[curPos] = State.WEAKENED;
                        break;
                    case State.WEAKENED:
                        infectionCount++;
                        _points[curPos] = State.INFECTED;
                        break;
                    case State.INFECTED:
                        curDir = TurnRight(curDir);
                        _points[curPos] = State.FLAGGED;
                        break;
                    case State.FLAGGED:
                        curDir = TurnAround(curDir);
                        _points[curPos] = State.CLEAN;
                        break;
                }
                switch (curDir)
                {
                    case Direction.UP:
                        curPos.Y++;
                        break;
                    case Direction.LEFT:
                        curPos.X--;
                        break;
                    case Direction.DOWN:
                        curPos.Y--;
                        break;
                    case Direction.RIGHT:
                        curPos.X++;
                        break;
                }
                if (!_points.ContainsKey(curPos))
                    _points.Add(curPos, State.CLEAN);
            }
            return infectionCount;
        }

        private void SetPoints(string map)
        {
            var lines = map.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var offset = lines[0].Length / 2;
            _points = new Dictionary<Point, State>();
            for (var row = offset; row >= -1 * offset; row--)
            {
                for (var col = -1 * offset; col <= offset; col++)
                {
                    var infected = lines[offset - row][col + offset] == '#';
                    _points.Add(new Point(col, row), infected ? State.INFECTED : State.CLEAN);
                }
            }
        }

        private Direction TurnLeft(Direction direction)
        {
            if ((int)direction + 1 > 3)
                return Direction.UP;
            else
                return (Direction)((int)direction + 1);
        }

        private Direction TurnRight(Direction direction)
        {
            if ((int)direction - 1 < 0)
                return Direction.RIGHT;
            else
                return (Direction)((int)direction - 1);
        }

        private Direction TurnAround(Direction direction)
        {
            if ((int)direction > 1)
                return (Direction)((int)direction - 2);
            else
                return (Direction)((int)direction + 2);
        }

        public static void Run()
        {
            var day22 = new Day22();
            Console.WriteLine("\n###############\n###############\nDay 22\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            var test = @"..#
#..
...";
            Console.WriteLine(day22.Part1(test, 7));
            Console.WriteLine(day22.Part1(test, 70));
            Console.WriteLine(day22.Part1(test, 10000));
            Console.WriteLine(day22.Part1(Input, 10000));

            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(day22.Part2(test, 100));
            Console.WriteLine(day22.Part2(test, 10000000));
            Console.WriteLine(day22.Part2(Input, 10000000));
        }

        private static string Input
        {
            get
            {
                return @"...#.##.#.#.#.#..##.###.#
......##.....#####..#.#.#
#..####.######.#.#.##...#
...##..####........#.#.#.
.#.#####..#.....#######..
.#...#.#.##.#.#.....#....
.#.#.#.#.#####.#.#..#...#
###..##.###.#.....#...#.#
#####..#.....###.....####
#.##............###.#.###
#...###.....#.#.##.#..#.#
.#.###.##..#####.....####
.#...#..#..###.##..#....#
##.##...###....##.###.##.
#.##.###.#.#........#.#..
##......#..###.#######.##
.#####.##..#..#....##.##.
###..#...#..#.##..#.....#
##..#.###.###.#...##...#.
##..##..##.###..#.##..#..
...#.#.###..#....##.##.#.
##.##..####..##.##.##.##.
#...####.######.#...##...
.###..##.##..##.####....#
#.##....#.#.#..#.###..##.";
            }
        }

        public enum Direction
        {
            UP = 0,
            LEFT = 1,
            DOWN = 2,
            RIGHT = 3
        }

        public enum State
        {
            CLEAN,
            WEAKENED,
            INFECTED,
            FLAGGED
        }
    }
}
