using System;
using System.Collections.Generic;
using System.Drawing;

namespace AdventOfCode2017
{
    public class Day3
    {
        private Dictionary<Point, int> values = new Dictionary<Point, int>();

        public int Part1(int start)
        {
            if (start == 1)
                return 0;
            var ring = 0; // The center is considered ring 0
            do
            {
                ring++;
            }
            while (Math.Pow(2 * ring + 1, 2) < start);
            var length = 2 * ring + 1;
            var max = Math.Pow(length, 2);
            var min = Math.Pow(2 * (ring - 1) + 1, 2);
            var side = 1;
            while (start - ((length - 1) * side) > min)
            {
                side++;
            }
            side--;
            var distFromNextCorner = start - ((length - 1) * side) - min;
            var offset = (int) Math.Abs((length - distFromNextCorner) - ((length / 2) + 1));
            return ring + offset;
        }

        public int Part2(int input)
        {
            var xVal = 0;
            var yVal = 0;
            var val = 1;
            values.Add(new Point(xVal, yVal), val);
            var currentDirection = Direction.DOWN;            
            while (val < input)
            {
                // Check direction of next point
                switch (currentDirection)
                {
                    case Direction.DOWN:
                        if (!values.ContainsKey(new Point(xVal + 1, yVal)))
                        {
                            currentDirection = Direction.RIGHT;
                            xVal++;
                        }
                        else
                        {
                            yVal--;
                        }
                        break;
                    case Direction.RIGHT:
                        if (!values.ContainsKey(new Point(xVal, yVal + 1)))
                        {
                            currentDirection = Direction.UP;
                            yVal++;
                        }
                        else
                        {
                            xVal++;
                        }
                        break;
                    case Direction.UP:
                        if (!values.ContainsKey(new Point(xVal - 1, yVal)))
                        {
                            currentDirection = Direction.LEFT;
                            xVal--;
                        }
                        else
                        {
                            yVal++;
                        }
                        break;
                    case Direction.LEFT:
                        if (!values.ContainsKey(new Point(xVal, yVal - 1)))
                        {
                            currentDirection = Direction.DOWN;
                            yVal--;
                        }
                        else
                        {
                            xVal--;
                        }
                        break;
                }
                val = CalcValue(new Point(xVal, yVal));
            }
            return val;
        }

        private int CalcValue(Point point)
        {
            var total = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (values.ContainsKey(new Point(point.X + x, point.Y + y)))
                        total += values[new Point(point.X + x, point.Y + y)];
                }
            }
            values.Add(point, total);
            return total;
        }

        public static void Run()
        {
            var input = 265149;
            var day3 = new Day3();
            Console.WriteLine("\n###############\n###############\nDay 3\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(day3.Part1(1));
            Console.WriteLine(day3.Part1(12));
            Console.WriteLine(day3.Part1(23));
            Console.WriteLine(day3.Part1(1024));
            Console.WriteLine(day3.Part1(input));

            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(day3.Part2(input));
        }

        protected enum Direction
        {
            UP,
            LEFT,
            DOWN,
            RIGHT
        }
    }
}
