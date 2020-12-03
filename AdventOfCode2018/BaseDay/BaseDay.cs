using System;

namespace AdventOfCode2018.BaseDay
{
    class BaseDay
    {
        public string Part1(string[] input)
        {
            throw new NotImplementedException();
        }

        public string Part2(string[] input)
        {
            throw new NotImplementedException();
        }

        public static void Run()
        {
            var input = GetInput().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var exampleInput = @"".Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var BaseDay = new BaseDay();
            Console.WriteLine("\n###############\n###############\nBaseDay\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(BaseDay.Part1(exampleInput));
            //Console.WriteLine(BaseDay.Part1(input));
            //
            //Console.WriteLine("\n###############\nPart 2\n###############\n");
            //Console.WriteLine(BaseDay.Part2(exampleInput));
            //Console.WriteLine(BaseDay.Part2(input));
        }

        private static string GetInput()
        {
            return @"";
        }
    }
}
