using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day8
{
    internal class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = (await ReadInput()).Select(x => x.Split('|')[1]);
            var uniqueCounts = new int[] { 2, 4, 3, 7 }; // 1, 4, 7, 8
            var uniques = 0;
            foreach (var line in input)
            {
                var digits = line.Split(' ');
                uniques += digits.Count(x => uniqueCounts.Contains(x.Length));
            }

            return uniques.ToString();
        }

        public new async Task<string> Part2()
        {
            var input = await ReadInput();
            var sum = 0;
            foreach (var line in input)
            {
                var parts = line.Split(" | ");
                var numberPatterns = parts[0].Split(' ').Select(x => String.Concat(x.OrderBy(y => y)));
                var mapping = new DigitMapping(numberPatterns.First(x => x.Length == 7).ToList());

                // 1 has 2 possibilities for top right and bottom right
                mapping.PossibleTopRight.AddRange(numberPatterns.First(x => x.Length == 2));
                mapping.PossibleBottomRight = mapping.PossibleTopRight;

                // 7 has only the top character in addition to 1
                var pattern = numberPatterns.First(x => x.Length == 3);
                mapping.Top = pattern.Where(x => !mapping.PossibleTopRight.Contains(x)).First();

                // 4 has the middle and top left in addition to 1
                pattern = numberPatterns.First(x => x.Length == 4);
                mapping.PossibleMiddle = pattern.Where(x => !mapping.PossibleTopRight.Contains(x)).ToList();
                mapping.PossibleTopLeft = mapping.PossibleMiddle;

                // top has 8 numbers
                // top left has 6 numbers
                // bottom left has 4 numbers
                // middle has 7 numbers
                // top right has 8 numbers
                // bottom right has 9 numbers
                // bottom has 7 numbers
                foreach (var c in mapping.AllChars.ToArray())
                {
                    var numOfPatterns = numberPatterns.Count(x => x.Contains(c));
                    switch (numOfPatterns)
                    {
                        case 4:
                            mapping.BottomLeft = c;
                            break;
                        case 6:
                            mapping.TopLeft = c;
                            break;
                        case 8:
                            mapping.TopRight = c;
                            break;
                        case 9:
                            mapping.BottomRight = c;
                            break;
                    }
                }

                // middle will be the last character left from 4 (whatever top left wasn't)
                mapping.PossibleMiddle.Remove(mapping.TopLeft);
                mapping.Middle = mapping.PossibleMiddle.First();

                // bottom is the remaining character
                mapping.Bottom = mapping.AllChars.First();

                //LogMessage(mapping.ToString());

                var digit = new Digit(mapping);
                var sb = new StringBuilder();
                foreach (var sequence in parts[1].Split(' '))
                {
                    sb.Append(digit.Value(sequence));
                }
                var number = sb.ToString();
                LogMessage(number);
                sum += int.Parse(number);
            }

            return sum.ToString();
        }

        private string[] GetSample()
        {
            var sample = @"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce";
            return sample.Split(Environment.NewLine);
        }

        private string[] GetShorterSample()
        {
            return new string[] { "acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf" };
        }
    }
}
