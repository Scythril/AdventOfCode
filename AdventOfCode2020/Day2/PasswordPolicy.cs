using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Day2
{
    class PasswordPolicy
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public char Value { get; set; }
        public string Password { get; set; }

        public PasswordPolicy(string line)
        {
            var regex = Regex.Match(line, @"^(\d+)-(\d+)\s(\w):\s(.*)$");
            Min = int.Parse(regex.Groups[1].Value);
            Max = int.Parse(regex.Groups[2].Value);
            Value = char.Parse(regex.Groups[3].Value);
            Password = regex.Groups[4].Value;
        }

        public bool IsValid()
        {
            var charCount = Password.ToCharArray().Count(c => c == Value);
            return charCount >= Min && charCount <= Max;
        }

        public bool IsValid2() => Password[Min - 1] == Value ^ Password[Max - 1] == Value;
    }
}
