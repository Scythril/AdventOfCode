namespace AdventOfCode2021.Day12
{
    internal class Cave
    {
        public string Name { get; set; }
        public List<string> Connections { get; set; }

        public Cave(string name)
        {
            Name = name;
            Connections = new List<string>();
        }

        public bool IsSmallCave => char.IsLower(Name[0]);
    }
}
