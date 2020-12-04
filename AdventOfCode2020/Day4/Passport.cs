using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2020.Day4
{
    class Passport
    {
        public string BirthYear { get; set; }
        public string IssueYear { get; set; }
        public string ExpirationYear { get; set; }
        public string Height { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public string PassportId { get; set; }
        public string CountryId { get; set; }

        private readonly string[] ValidEyeColors = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

        public Passport(string[] pairs)
        {
            foreach (var field in pairs)
            {
                var parts = field.Split(':');
                var value = parts[1].Trim();
                switch (parts[0])
                {
                    case "byr":
                        BirthYear = value;
                        break;
                    case "iyr":
                        IssueYear = value;
                        break;
                    case "eyr":
                        ExpirationYear = value;
                        break;
                    case "hgt":
                        Height = value;
                        break;
                    case "hcl":
                        HairColor = value;
                        break;
                    case "ecl":
                        EyeColor = value;
                        break;
                    case "pid":
                        PassportId = value;
                        break;
                    case "cid":
                        CountryId = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public bool IsValid(out List<string> errors)
        {
            errors = new List<string>();

            if (!IsValidBirthYear)
                errors.Add($"Invalid birth year: {BirthYear}");

            if (!IsValidIssueYear)
                errors.Add($"Invalid issue year: {IssueYear}");

            if (!IsValidExpirationYear)
                errors.Add($"Invalid expiration year: {ExpirationYear}");

            if (!IsValidHeight)
                errors.Add($"Invalid height: {Height}");

            if (!IsValidHairColor)
                errors.Add($"Invalid hair color: {HairColor}");

            if (!IsValidEyeColor)
                errors.Add($"Invalid eye color: {EyeColor}");

            if (!IsValidPassportId)
                errors.Add($"Invalid passport id: {PassportId}");

            return !errors.Any();
        }

        private bool IsValidYear(string value, int min, int max)
        {
            if (value == null)
                return false;

            if (int.TryParse(value, out var year))
                return year >= min && year <= max;

            return false;
        }

        private bool IsValidBirthYear => IsValidYear(BirthYear, 1920, 2002);

        private bool IsValidIssueYear => IsValidYear(IssueYear, 2010, 2020);

        private bool IsValidExpirationYear => IsValidYear(ExpirationYear, 2020, 2030);

        private bool IsValidHeight
        {
            get
            {
                if (Height == null)
                    return false;

                if (Height.EndsWith("cm"))
                {
                    if (int.TryParse(Height.Substring(0, Height.Length - 2), out var hgt))
                        return hgt >= 150 && hgt <= 193;
                }
                else if (Height.EndsWith("in"))
                {
                    if (int.TryParse(Height.Substring(0, Height.Length - 2), out var hgt))
                        return hgt >= 59 && hgt <= 76;
                }

                return false;
            }
        }

        private bool IsValidHairColor => HairColor != null && Regex.IsMatch(HairColor, @"^#[0-9a-f]{6}$");

        private bool IsValidEyeColor => EyeColor != null && ValidEyeColors.Contains(EyeColor);

        private bool IsValidPassportId => PassportId != null && Regex.IsMatch(PassportId, @"^[0-9]{9}$");

        public override string ToString()
        {
            return $"Country ID: {CountryId}\nPassport ID: {PassportId}\nBirth Year: {BirthYear}\nIssue Year: {IssueYear}\nExpiration Year: {ExpirationYear}\nHeight: {Height}\nHair Color: {HairColor}\nEye Color: {EyeColor}";
        }
    }
}
