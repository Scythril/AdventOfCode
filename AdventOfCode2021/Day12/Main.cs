namespace AdventOfCode2021.Day12
{
    internal class Main : BaseClass
    {
        private readonly Dictionary<string, Cave> _caves = new();
        private readonly List<List<Cave>> _allPaths = new();

        public new async Task<string> Part1()
        {
            _caves.Clear();
            _allPaths.Clear();
            var input = await ReadInput();
            GetCaves(input);
            FindPaths(new Stack<Cave>(), _caves["start"]);

            return _allPaths.Count.ToString();
        }

        public new async Task<string> Part2()
        {
            _caves.Clear();
            _allPaths.Clear();
            var input = await ReadInput();
            GetCaves(input);
            FindPaths(new Stack<Cave>(), _caves["start"], true);

            return _allPaths.Count.ToString();
        }

        private void GetCaves(string[] input)
        {
            foreach (var line in input)
            {
                var parts = line.Split('-');
                if (!_caves.ContainsKey(parts[0]))
                    _caves.Add(parts[0], new Cave(parts[0]));

                if (!_caves.ContainsKey(parts[1]))
                    _caves.Add(parts[1], new Cave(parts[1]));

                _caves[parts[0]].Connections.Add(parts[1]);
                _caves[parts[1]].Connections.Add(parts[0]);
            }
        }

        private void FindPaths(Stack<Cave> path, Cave current, bool canVisitTwice = false)
        {
            if (current.IsSmallCave && path.Contains(current))
            {
                if (!canVisitTwice || current.Name == "start")
                {
                    return;
                }
                else
                {
                    canVisitTwice = false;
                }
            }

            path.Push(current);
            if (current.Name == "end")
            {
                _allPaths.Add(path.Reverse().ToList());
                path.Pop();
                return;
            }

            foreach (var connection in current.Connections)
                FindPaths(path, _caves[connection], canVisitTwice);

            path.Pop();
        }

        private string[] GetSample()
        {
            var sample = @"start-A
start-b
A-c
A-b
b-d
A-end
b-end";
            return sample.Split(Environment.NewLine);
        }

        private string[] GetSample2()
        {
            var sample = @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc";
            return sample.Split(Environment.NewLine);
        }

        private string[] GetSample3()
        {
            var sample = @"fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW";
            return sample.Split(Environment.NewLine);
        }
    }
}
