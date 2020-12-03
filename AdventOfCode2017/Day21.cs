using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017
{
    public class Day21
    {
        private Dictionary<string, string> _rules;
        private readonly string startingImage = ".#./..#/###";

        public long Part1(string input, int iterations)
        {
            _rules = new Dictionary<string, string>();
            SetRules(input);
            var image = Enhance(startingImage, iterations);
            return image.Count(x => x == '#');
        }

        private void SetRules(string input)
        {
            var rawRules = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var rule in rawRules)
            {
                var pieces = rule.Split(" => ", StringSplitOptions.RemoveEmptyEntries);
                var from = pieces[0];
                var to = pieces[1];
                for (var i = 0; i < 4; i++)
                {
                    _rules.TryAdd(from, to);
                    _rules.TryAdd(Flip(from), to);
                    from = Rotate(from);
                    _rules.TryAdd(from, to);
                }
            }
        }

        private string Flip(string input)
        {
            var newString = new List<string>();
            var list = input.Split('/');
            foreach (var row in list)
                newString.Add(new String(row.Reverse().ToArray()));
            return String.Join('/', newString);
        }

        private string Rotate(string input)
        {
            var oldString = input.Split('/');
            var newString = new List<string>();
            var length = oldString.Length;
            for (var row = 0; row < length; row++)
            {
                newString.Add("");
                for (var col = 0; col < length; col++)
                {
                    newString[row] += oldString[length - 1 - col][row];
                }
            }
            return String.Join('/', newString);
        }        

        private string Enhance(string image, int iterations)
        {
            var imageList = image.Split('/').ToList();
            for (var i = 0; i < iterations; i++)
            {
                imageList = SplitSquares(imageList);
                for (var j = 0; j < imageList.Count; j++)
                    imageList[j] = _rules[imageList[j]];
            }

            var newImage = "";
            var len = Convert.ToInt32(Math.Sqrt(imageList.Count));
            for (var row = 0; row < len; row++)
            {
                var imageRow = new List<string>();
                var squareSize = imageList[0].Split('/').Length;
                for (var col = 0; col < len; col++)
                {
                    var idx = row * len + col;
                    imageRow.AddRange(imageList[idx].Split('/'));
                }
                var lines = new List<string>();
                for (var i = 0; i < squareSize; i++)
                {
                    var line = "";
                    for (var j = 0; j < imageRow.Count; j++)
                    {
                        if (j % squareSize != i)
                            continue;
                        line += imageRow[j];
                    }
                    lines.Add(line);
                }
                newImage += String.Join('/', lines);
            }
            return newImage;
        }

        private List<string> SplitSquares(List<string> image)
        {
            var squares = new List<string>();
            var squareSize = (image.Count % 2 == 0) ? 2 : 3;
            var squareCount = Math.Pow(image.Count / squareSize, 2);
            var x = 0;
            var y = 0;
            for (var i = 0; i < squareCount; i++)
            {
                squares.Add("");
                while (y < image.Count)
                {
                    squares[i] += image[y][x];
                    if (++x % squareSize == 0)
                    {
                        squares[i] += "/";
                        x -= squareSize;
                        y++;
                        if (y % squareSize == 0)
                        {
                            y -= squareSize;
                            x += squareSize;
                            if (x >= image.Count)
                            {
                                x = 0;
                                y += squareSize;
                            }
                            continue;
                        }
                    }
                }
                squares[i] = squares[i].Trim('/');
            }

            return squares;
        }

        public static void Run()
        {
            var day21 = new Day21();
            Console.WriteLine("\n###############\n###############\nDay 21\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(day21.Part1(@"../.# => ##./#../...
.#./..#/### => #..#/..../..../#..#", 2));
            //Console.WriteLine(day21.Part1(Input));
        }

        private static string Input
        {
            get
            {
                return @"../.. => .#./.#./###
#./.. => .#./.#./##.
##/.. => #../.##/.#.
.#/#. => #.#/#../..#
##/#. => ###/##./#.#
##/## => .../.../.#.
.../.../... => #.#./.###/..##/#.##
#../.../... => #.#./#.##/#..#/#..#
.#./.../... => #.#./###./#.#./..#.
##./.../... => ##../###./##.#/...#
#.#/.../... => ..../..#./####/..#.
###/.../... => ##../..../#.../#...
.#./#../... => .#../..##/#..#/.#.#
##./#../... => ####/.###/.###/.###
..#/#../... => .#.#/.##./...#/##..
#.#/#../... => #.##/#.##/#.##/.#..
.##/#../... => .#../...#/..#./.##.
###/#../... => .##./.###/#..#/##.#
.../.#./... => .#../.#.#/.#../#.##
#../.#./... => ...#/##../####/##..
.#./.#./... => ###./#..#/..#./...#
##./.#./... => #.##/..#./#.#./..#.
#.#/.#./... => .#.#/...#/..../#.##
###/.#./... => ..##/##.#/#.##/###.
.#./##./... => .##./####/##../####
##./##./... => .###/..../####/#...
..#/##./... => ..../##.#/.###/.##.
#.#/##./... => #.#./###./..../###.
.##/##./... => ###./.###/.#../##.#
###/##./... => #.##/#.#./..../##.#
.../#.#/... => ###./#.##/.###/#.##
#../#.#/... => ##.#/..../..../.#.#
.#./#.#/... => .#.#/..##/.#../.##.
##./#.#/... => .##./..#./...#/#...
#.#/#.#/... => ..../###./..#./.#.#
###/#.#/... => ..##/.##./###./#.##
.../###/... => .#../####/.##./..#.
#../###/... => ..##/#.#./...#/##..
.#./###/... => ..#./####/##../#.##
##./###/... => .##./##.#/####/.#.#
#.#/###/... => .###/#.##/####/.##.
###/###/... => #.../#.../##../.##.
..#/.../#.. => ..##/#.#./#.../#.#.
#.#/.../#.. => ###./##.#/..#./##.#
.##/.../#.. => ..#./..../##../.#.#
###/.../#.. => ####/.#.#/.#.#/####
.##/#../#.. => ####/####/...#/.#.#
###/#../#.. => ..##/..#./.##./##..
..#/.#./#.. => ####/...#/####/#..#
#.#/.#./#.. => ..#./.###/#.#./##.#
.##/.#./#.. => .###/.#.#/#..#/..#.
###/.#./#.. => ..../##../.#.#/.#..
.##/##./#.. => ###./####/..../#...
###/##./#.. => ####/#..#/##.#/##.#
#../..#/#.. => ####/##.#/..../.###
.#./..#/#.. => ..../.#../..#./..#.
##./..#/#.. => .#.#/...#/#.##/..#.
#.#/..#/#.. => #.#./#.##/#..#/####
.##/..#/#.. => ..#./##../####/.#..
###/..#/#.. => #.../##.#/###./.#.#
#../#.#/#.. => ..../.#.#/..#./#.#.
.#./#.#/#.. => #.##/...#/.##./.#..
##./#.#/#.. => .###/##.#/##.#/####
..#/#.#/#.. => ..../..../...#/##.#
#.#/#.#/#.. => ##.#/.#../###./..#.
.##/#.#/#.. => ###./..../...#/.##.
###/#.#/#.. => .###/#..#/.##./.###
#../.##/#.. => ##../.#.#/.##./.##.
.#./.##/#.. => .###/.###/..##/.#..
##./.##/#.. => ..##/###./...#/#...
#.#/.##/#.. => ..#./###./...#/##..
.##/.##/#.. => ####/###./#.#./##..
###/.##/#.. => ..##/.##./#.../..##
#../###/#.. => ####/.#../.###/.#.#
.#./###/#.. => .##./##.#/..##/##..
##./###/#.. => ..##/##.#/##../.#.#
..#/###/#.. => ##../..../.#.#/#..#
#.#/###/#.. => ..#./###./####/..##
.##/###/#.. => ##../##../..##/.##.
###/###/#.. => ###./...#/#..#/..#.
.#./#.#/.#. => ..../.###/.###/#...
##./#.#/.#. => .###/..#./..../#...
#.#/#.#/.#. => #..#/.##./#.##/..#.
###/#.#/.#. => ####/##../####/....
.#./###/.#. => ..../.###/..../###.
##./###/.#. => ###./.#../#.#./.#..
#.#/###/.#. => ..../..##/..##/....
###/###/.#. => ###./...#/#.../..#.
#.#/..#/##. => ###./.##./.#../....
###/..#/##. => ####/...#/##../#..#
.##/#.#/##. => ..../...#/##.#/#.##
###/#.#/##. => .#.#/.###/..../#...
#.#/.##/##. => .#.#/#.#./...#/#...
###/.##/##. => .##./...#/#.../..#.
.##/###/##. => .#.#/.##./.##./##..
###/###/##. => #.#./##../##../...#
#.#/.../#.# => #.#./##.#/##.#/####
###/.../#.# => .#../.#.#/.##./#.##
###/#../#.# => ###./##../..##/##..
#.#/.#./#.# => ####/#.#./###./.##.
###/.#./#.# => ..#./.##./..../#...
###/##./#.# => #..#/##.#/.##./.#..
#.#/#.#/#.# => .#../###./##.#/.#..
###/#.#/#.# => .#../#.##/##.#/..#.
#.#/###/#.# => ##.#/.###/..##/.#..
###/###/#.# => .#../.###/..#./#...
###/#.#/### => ###./####/.###/#.##
###/###/### => #..#/.#../#.../...#";
            }
        }
    }
}
