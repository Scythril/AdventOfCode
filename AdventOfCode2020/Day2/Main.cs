using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day2
{
    class Main : BaseClass
    {
        public async Task<string> Part1()
        {
            var input = await ReadInput();
            var validPasswords = 0;
            foreach (var line in input)
            {
                var policy = new PasswordPolicy(line);
                if (policy.IsValid())
                    validPasswords++;
            }

            return validPasswords.ToString();
        }

        public async Task<string> Part2()
        {
            var input = await ReadInput();
            var validPasswords = 0;
            foreach (var line in input)
            {
                var policy = new PasswordPolicy(line);
                if (policy.IsValid2())
                    validPasswords++;
            }

            return validPasswords.ToString();
        }

        private string[] GetSample()
        {
            var sample = @"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc";
            return sample.Split(Environment.NewLine);
        }
    }
}
