namespace AdventOfCode2024.Day1
{
    internal class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var firstList = new List<int>();
            var secondList = new List<int>();
            var input = await ReadInput();
            foreach (var line in input)
            {
                var lineSplit = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                firstList.Add(int.Parse(lineSplit[0]));
                secondList.Add(int.Parse(lineSplit[1]));
            }

            firstList.Sort();
            secondList.Sort();
            var count = 0;
            for (var i = 0; i < firstList.Count; i++)
            {
                count += Math.Abs(firstList[i] - secondList[i]);
            }
            
            return count.ToString();
        }

        public new async Task<string> Part2()
        {
            //var input = GetSample();
            var input = await ReadInput();
            
            var firstList = new List<int>();
            var secondList = new List<int>();
            foreach (var line in input)
            {
                var lineSplit = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                firstList.Add(int.Parse(lineSplit[0]));
                secondList.Add(int.Parse(lineSplit[1]));
            }

            secondList.Sort();
            var score = firstList.Sum(num => secondList.Count(x => x == num) * num);
            
            return score.ToString();
        }

        private string[] GetSample()
        {
            var sample = @"3   4
4   3
2   5
1   3
3   9
3   3";
            return sample.Split(Environment.NewLine);
        }  
    }
}