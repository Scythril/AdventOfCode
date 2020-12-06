using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day6
{
    class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = await ReadInputAsString();
            var groups = input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries);
            var yeses = 0;
            foreach (var group in groups)
            {
                var groupAnswers = new HashSet<char>();
                var people = group.Split(Environment.NewLine);
                foreach (var person in people)
                {
                    foreach (var answer in person)
                        groupAnswers.Add(answer);
                }

                yeses += groupAnswers.Count;
            }

            return yeses.ToString();
        }

        public new async Task<string> Part2()
        {
            var input = await ReadInputAsString();
            var groups = input.Split($"{Environment.NewLine}{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries);
            var yeses = 0;
            foreach (var group in groups)
            {
                var groupAnswers = new HashSet<char>();
                var people = group.Split(Environment.NewLine);
                LogMessage($"Group: {group}");
                for (var c = 'a'; c <= 'z'; c++)
                {
                    if (people.All(p => p.Contains(c)))
                        groupAnswers.Add(c);
                }

                LogMessage($"Group answers: {string.Join(' ', groupAnswers)}");
                yeses += groupAnswers.Count;
            }

            return yeses.ToString();
        }

        private string GetSample()
        {
            return @"abc

a
b
c

ab
ac

a
a
a
a

b";
        }
    }
}
