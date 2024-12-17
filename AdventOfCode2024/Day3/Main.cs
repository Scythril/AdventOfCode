using System.Text.RegularExpressions;

namespace AdventOfCode2024.Day3;

internal class Main : BaseClass
{
    public new async Task<string> Part1()
    {
        //var input = GetSample();
        var input = string.Join("", await ReadInput());
        var matches = Regex.Matches(input, @"mul\((\d{1,3}),(\d{1,3})\)");
        return matches.Sum(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)).ToString();
    }

    public new async Task<string> Part2()
    {
        //var input = GetSample();
        var input = string.Join("", await ReadInput());
        var matches = Regex.Matches(input, @"((?'m'mul)\((?'first'\d{1,3}),(?'second'\d{1,3})\))|(?'cond'do(?:n't)?)\(\)");
        var enabled = true;
        var sum = 0;
        foreach (Match match in matches)
        {
            if (match.Groups.ContainsKey("cond") && match.Groups["cond"].Length > 0)
            {
                enabled = match.Groups["cond"].Value.Equals("do");
                continue;
            }
            
            if (match.Groups.ContainsKey("m") && match.Groups["m"].Length > 0)
            {
                if (!enabled)
                {
                    continue;
                }
                
                sum += int.Parse(match.Groups["first"].Value) * int.Parse(match.Groups["second"].Value);
            }
        }

        return sum.ToString();
    }
    
    private string GetSample()
    {
        var sample = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";
        return sample;
    }
}