using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2017
{
    public class Day10
    {
        private IList<int> _list { get; set; }

        public int Part1(String input, int size)
        {
            _list = Enumerable.Range(0, size).ToList();
            var lengths = input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Int32.Parse(x)).ToList();
            var curPos = 0;
            for (var skipSize = 0; skipSize < lengths.Count; skipSize++)
            {
                var len = lengths[skipSize];
                ReverseSublist(curPos, len);
                curPos = (curPos + len + skipSize) % _list.Count;
            }

            return _list[0] * _list[1];
        }

        public String Part2(String input)
        {
            _list = Enumerable.Range(0, 256).ToList();
            var lengths = input.Select(x => (int) x).Concat(new List<int> { 17, 31, 73, 47, 23 }).ToList();
            var curPos = 0;
            var skipSize = 0;
            for (int i = 0; i < 64; i++)
            {
                for (int j = 0; j < lengths.Count; j++)
                {
                    var len = lengths[j];
                    ReverseSublist(curPos, len);
                    curPos = (curPos + len + skipSize) % _list.Count;
                    skipSize++;
                }
            }

            var denseHash = new List<int>();
            for (int i = 0; i < _list.Count / 16; i++)
            {
                var start = i * 16;
                for (int j = 0; j < 16; j++)
                {
                    if (j == 0)
                        denseHash.Add(_list[start]);
                    else
                        denseHash[i] ^= _list[start + j];
                }
            }
            return String.Join("", denseHash.Select(x => x.ToString("x2")));
        }

        private void ReverseSublist(int currentPosition, int length)
        {
            var end = (currentPosition + length - 1) % _list.Count;
            for (int i = 0; i < length / 2; i++)
            {
                var next = (currentPosition + i) % _list.Count;
                var prev = end - i;
                if (prev < 0)
                    prev += _list.Count;
                var temp = _list[next];
                _list[next] = _list[prev];
                _list[prev] = temp;
            }
        }

        public static void Run()
        {
            var day10 = new Day10();
            Console.WriteLine("\n###############\n###############\nDay 10\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(day10.Part1("3,4,1,5", 5));
            Console.WriteLine(day10.Part1(Input, 256));

            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(day10.Part2(""));
            Console.WriteLine(day10.Part2("AoC 2017"));
            Console.WriteLine(day10.Part2("1,2,3"));
            Console.WriteLine(day10.Part2("1,2,4"));
            Console.WriteLine(day10.Part2(Input));
        }

        private static String Input
        {
            get
            {
                return @"183,0,31,146,254,240,223,150,2,206,161,1,255,232,199,88";
            }
        }
    }
}
