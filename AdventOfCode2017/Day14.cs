using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2017
{
    public class Day14
    {
        private Dictionary<int, BitArray> _grid;
        private Dictionary<int, HashSet<Point>> _regions;

        public int Part1(String input)
        {
            var used = 0;
            var day10 = new Day10();
            var grid = new Dictionary<int, BitArray>();
            for (int row = 0; row < 128; row++)
            {
                var hashInput = String.Format("{0}-{1}", input, row);
                var hashOutput = day10.Part2(hashInput);
                for (int i = 0; i < hashOutput.Length; i++)
                {
                    var b = Byte.Parse(hashOutput[i].ToString(), System.Globalization.NumberStyles.HexNumber);
                    for (int j = 0; j < 4; j++)
                    {
                        if ((b & (1 << (3 - j))) != 0)
                            used++;
                    }
                }
            }
            return used;
        }

        public int Part2(String input)
        {
            var day10 = new Day10();
            _grid = new Dictionary<int, BitArray>();
            _regions = new Dictionary<int, HashSet<Point>>();
            for (int row = 0; row < 128; row++)
            {
                var hashInput = String.Format("{0}-{1}", input, row);
                var hashOutput = day10.Part2(hashInput);
                var bitArray = new BitArray(hashOutput.Length * 4);
                for (int i = 0; i < hashOutput.Length; i++)
                {
                    var b = Byte.Parse(hashOutput[i].ToString(), System.Globalization.NumberStyles.HexNumber);
                    for (int j = 0; j < 4; j++)
                        bitArray.Set(i * 4 + j, (b & (1 << (3 - j))) != 0);
                }
                _grid.Add(row, bitArray);
            }
            for (int row = 0; row < 128; row++)
            {
                for (int col = 0; col < 128; col++)
                    FindRegionMembers(col, row, _regions.Count);
            }
            return _regions.Count;
        }

        private void FindRegionMembers(int col, int row, int regionNumber)
        {
            // Square is out of bounds, free, or already part of another region
            if (col < 0 || col > 127 || row < 0 || row > 127 || !_grid[row].Get(col) || _regions.Any(x => x.Value.Contains(new Point(col, row))))
                return;

            if (!_regions.ContainsKey(regionNumber))
                _regions.Add(regionNumber, new HashSet<Point> { new Point(col, row) });
            else
                _regions[regionNumber].Add(new Point(col, row));

            FindRegionMembers(col + 1, row, regionNumber);
            FindRegionMembers(col - 1, row, regionNumber);
            FindRegionMembers(col, row + 1, regionNumber);
            FindRegionMembers(col, row - 1, regionNumber);
        }

        public static void Run()
        {
            var day14 = new Day14();
            Console.WriteLine("\n###############\n###############\nDay 14\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(day14.Part1(@"flqrgnkx"));
            Console.WriteLine(day14.Part1(Input));

            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(day14.Part2(@"flqrgnkx"));
            Console.WriteLine(day14.Part2(Input));
        }

        private static String Input
        {
            get
            {
                return @"hwlqcszp";
            }
        }
    }
}
