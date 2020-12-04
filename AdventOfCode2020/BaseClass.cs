using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    abstract class BaseClass
    {
        public string DayName => GetType().Namespace.Split('.').Last();
        public bool Debug = true;

        public async Task<string> Part1()
        {
            return string.Empty;
        }

        public async Task<string> Part2()
        {
            return string.Empty;
        }

        public async Task<string[]> ReadInput()
        {
            try
            {
                return await File.ReadAllLinesAsync(Path.Combine(DayName, "input.txt"));
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public async Task<string> ReadInputAsString()
        {
            try
            {
                return await File.ReadAllTextAsync(Path.Combine(DayName, "input.txt"));
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public void LogMessage(string message)
        {
            if (Debug)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }

        public void LogError(string message)
        {
            if (Debug)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}
