namespace AdventOfCode2021.Day10
{
    internal class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = await ReadInput();
            var bracketPairs = new Dictionary<char, char> { { ')', '(' }, { ']', '[' }, { '}', '{' }, { '>', '<' } };
            var bracketPoints = new Dictionary<char, int> { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
            var points = 0;
            foreach (var line in input)
            {
                var stack = new Stack<char>();
                foreach (var c in line)
                {
                    if (bracketPairs.ContainsValue(c))
                    {
                        stack.Push(c);
                        continue;
                    }

                    if (stack.Pop() != bracketPairs[c])
                    {
                        points += bracketPoints[c];
                        break;
                    }
                }
            }

            return points.ToString();
        }

        public new async Task<string> Part2()
        {
            var input = await ReadInput();
            var bracketPairs = new Dictionary<char, char> { { ')', '(' }, { ']', '[' }, { '}', '{' }, { '>', '<' } };
            var bracketPoints = new Dictionary<char, int> { { '(', 1 }, { '[', 2 }, { '{', 3 }, { '<', 4 } };
            var allPoints = new List<long>();
            foreach (var line in input)
            {
                var linePoints = 0L;
                var stack = new Stack<char>();
                var invalid = false;
                foreach (var c in line)
                {
                    if (bracketPairs.ContainsValue(c))
                    {
                        stack.Push(c);
                        continue;
                    }

                    if (stack.Pop() != bracketPairs[c])
                    {
                        invalid = true;
                        break;
                    }
                }

                if (invalid)
                    continue;

                while (stack.Count > 0)
                {
                    linePoints *= 5;
                    linePoints += bracketPoints[stack.Pop()];
                }

                allPoints.Add(linePoints);
            }

            allPoints.Sort();
            return allPoints[allPoints.Count / 2].ToString();
        }

        private string[] GetSample()
        {
            var sample = @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";
            return sample.Split(Environment.NewLine);
        }
    }
}
