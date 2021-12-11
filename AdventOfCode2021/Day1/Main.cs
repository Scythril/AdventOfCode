namespace AdventOfCode2021.Day1
{
    internal class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = (await ReadInput()).Select(x => int.Parse(x));
            var increases = 0;
            var lastDepth = int.MaxValue;
            foreach (var depth in input)
            {
                if (depth > lastDepth)
                    increases++;
                lastDepth = depth;
            }

            return increases.ToString();
        }

        public new async Task<string> Part2()
        {
            var input = (await ReadInput()).Select(x => int.Parse(x)).ToArray();
            //var input = GetSample().Select(x => int.Parse(x)).ToArray();
            var increases = 0;
            var lastSum = int.MaxValue;
            for (var i = 0; i < input.Length - 2; i++)
            {
                var sum = input[i] + input[i + 1] + input[i + 2];
                if (sum > lastSum)
                    increases++;
                lastSum = sum;
            }

            return increases.ToString();
        }

        private string[] GetSample()
        {
            var sample = @"199
200
208
210
200
207
240
269
260
263";
            return sample.Split(Environment.NewLine);
        }  
    }
}
