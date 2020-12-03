using System;
using System.IO;
using System.Threading.Tasks;

namespace AdventOfCode2020
{
    abstract class BaseClass
    {
        public async Task<string[]> ReadInput()
        {
            try
            {
                return await File.ReadAllLinesAsync(Path.Combine(GetType().Name, "input.txt"));
            }
            catch (IOException e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
