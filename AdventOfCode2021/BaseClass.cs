namespace AdventOfCode2021
{
    abstract class BaseClass
    {
        public string DayName => GetType().Namespace?.Split('.').Last() ?? String.Empty;
        public bool Debug = true;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> Part1()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return string.Empty;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<string> Part2()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
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

            return Array.Empty<string>();
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

            return String.Empty;
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
