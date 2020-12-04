﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day4
{
    class Main : BaseClass
    {
        private readonly string[] RequiredFields = new[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };

        public new async Task<string> Part1()
        {
            var input = await ReadInputAsString();
            var passports = input.Split($"{Environment.NewLine}{Environment.NewLine}");
            var valid = 0;
            foreach (var passport in passports)
            {
                var pairs = passport.Split(new[] { Environment.NewLine, " " }, StringSplitOptions.RemoveEmptyEntries);
                var fields = pairs.Select(p => p.Split(':')[0]);
                if (RequiredFields.All(f => fields.Contains(f)))
                    valid++;
            }

            return valid.ToString();
        }

        public new async Task<string> Part2()
        {
            var input = await ReadInputAsString();
            //var input = GetValidPassports();
            var passports = input.Split($"{Environment.NewLine}{Environment.NewLine}");
            var valid = 0;
            foreach (var passString in passports)
            {
                var pairs = passString.Split(new[] { Environment.NewLine, " " }, StringSplitOptions.RemoveEmptyEntries);
                var fields = pairs.Select(p => p.Split(':')[0]);
                if (RequiredFields.All(f => fields.Contains(f)))
                {
                    var passport = new Passport(pairs);
                    LogMessage(passport.ToString());
                    if (passport.IsValid(out var errors))
                    {
                        LogMessage("Passport is valid!");
                        valid++;
                    }
                    else
                        LogError($"Passport has required fields, but invalid values:\n{string.Join('\n', errors)}");
                }
                else
                    LogError("Passport is missing required fields and is therefore NOT valid!");

                LogMessage("######################################################");
            }

            return valid.ToString();
        }

        

        private string GetSample()
        {
            return @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in";
        }

        private string GetInvalidPassports()
        {
            return @"eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007";
        }

        private string GetValidPassports()
        {
            return @"pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719";
        }
    }
}
