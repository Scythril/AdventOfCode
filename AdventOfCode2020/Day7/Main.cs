using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day7
{
    class Main : BaseClass
    {
        private const string MyBag = "shiny gold";

        public new async Task<string> Part1()
        {
            var input = await ReadInput();
            var bagsWithContainers = new Dictionary<string, HashSet<string>>();
            foreach (var line in input)
            {
                var mainRegex = Regex.Match(line, @"^(.+) bags contain (.+)\.$");
                var color = mainRegex.Groups[1].Value;
                if (mainRegex.Groups[2].Value == "no other bags")
                    continue;

                var innerBags = mainRegex.Groups[2].Value.Split(',').Select(x => x.Trim(new[] { ' ', '.' }));
                foreach (var innerBag in innerBags)
                {
                    var innerRegex = Regex.Match(innerBag, @"(\d+) (.+) bags?");
                    if (bagsWithContainers.ContainsKey(innerRegex.Groups[2].Value))
                        bagsWithContainers[innerRegex.Groups[2].Value].Add(color);
                    else
                        bagsWithContainers[innerRegex.Groups[2].Value] = new HashSet<string> { color };
                }
            }

            var goodBags = GetContainers(bagsWithContainers, MyBag);
            return goodBags.Distinct().Count().ToString();
        }

        public new async Task<string> Part2()
        {
            var input = await ReadInput();
            var bags = new Dictionary<string, Dictionary<string, int>>();
            foreach (var line in input)
            {
                var mainRegex = Regex.Match(line, @"^(.+) bags contain (.+)\.$");
                var color = mainRegex.Groups[1].Value;
                bags.Add(color, new Dictionary<string, int>());
                if (mainRegex.Groups[2].Value == "no other bags")
                    continue;

                var innerBags = mainRegex.Groups[2].Value.Split(',').Select(x => x.Trim(new[] { ' ', '.' }));
                foreach (var innerBag in innerBags)
                {
                    var innerRegex = Regex.Match(innerBag, @"(\d+) (.+) bags?");
                    bags[color].Add(innerRegex.Groups[2].Value, int.Parse(innerRegex.Groups[1].Value));
                }
            }

            return GetBagCount(bags, MyBag).ToString();
        }

        private List<string> GetContainers(Dictionary<string, HashSet<string>> bags, string bag)
        {
            if (!bags.ContainsKey(bag))
                return new List<string>();

            return bags[bag].SelectMany(b => GetContainers(bags, b)).Concat(bags[bag]).ToList();
        }

        private int GetBagCount(Dictionary<string, Dictionary<string, int>> bags, string bag)
        {
            if (bags[bag].Count == 0)
                return 0;

            var directBags = bags[bag].Sum(b => b.Value);
            var bagsInside = bags[bag].Sum(kvp => kvp.Value * GetBagCount(bags, kvp.Key));
            return directBags + bagsInside;
        }

        private string[] GetSample()
        {
            return @"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.".Split(Environment.NewLine);
        }

        private string[] GetSample2()
        {
            return @"shiny gold bags contain 2 dark red bags.
dark red bags contain 2 dark orange bags.
dark orange bags contain 2 dark yellow bags.
dark yellow bags contain 2 dark green bags.
dark green bags contain 2 dark blue bags.
dark blue bags contain 2 dark violet bags.
dark violet bags contain no other bags.".Split(Environment.NewLine);
        }
    }
}
