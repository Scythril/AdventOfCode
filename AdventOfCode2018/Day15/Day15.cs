using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2018.Day15
{
    class Day15
    {
        private const bool WaitBetweenTurns = false;
        private const bool WriteHP = true;
        private const bool WriteGrids = true;

        public long Part1(string[] input)
        {
            Console.WriteLine();
            var units = GetUnits(input);
            var startingCursorTop = Console.CursorTop;
            var startingCursorLeft = Console.CursorLeft;
            PrintGrid(units, startingCursorTop, startingCursorLeft, 0);
            var round = 1;
            var combatOver = false;

            while (!combatOver)
            {
                for (var y = 0; y < units.Count && !combatOver; y++)
                {
                    for (var x = 0; x < units[y].Count; x++)
                    {
                        if (units[y][x].Moves < 1 || units[y][x].AttackPower < 1)
                            continue;

                        try
                        {
                            var unit = ProcessMoves(units, units[y][x], round);
                            if (unit == null)
                                continue;

                            ProcessAttacks(units, unit);
                        }
                        catch (CombatOverException coe)
                        {
                            combatOver = true;
                            break;
                        }
                    }
                }

                PrintGrid(units, startingCursorTop, startingCursorLeft, round);
                if (!combatOver)
                    round++;
            }

            var hp = units.SelectMany(x => x).Sum(x => x.HitPoints);
            Console.WriteLine($"Rounds: {round - 1}\nHP: {hp}");
            return (round - 1) * hp;
        }

        public int Part2(string[] input)
        {
            var elfAttack = 4;
            for (; elfAttack < 50; elfAttack++)
            {
                Console.WriteLine($"Processing with elf AP {elfAttack}");
                var units = GetUnits(input, elfAttack);
                var elfCount = units.SelectMany(u => u).Count(u => u is Elf);
                var startingCursorTop = Console.CursorTop;
                var startingCursorLeft = Console.CursorLeft;
                PrintGrid(units, startingCursorTop, startingCursorLeft, 0);
                var round = 1;
                var combatOver = false;
                var elfDied = false;

                while (!combatOver)
                {
                    for (var y = 0; y < units.Count && !combatOver; y++)
                    {
                        for (var x = 0; x < units[y].Count; x++)
                        {
                            if (units[y][x].Moves < 1 || units[y][x].AttackPower < 1)
                                continue;

                            try
                            {
                                var unit = ProcessMoves(units, units[y][x], round);
                                if (unit == null)
                                    continue;

                                if (ProcessAttacks(units, unit))
                                {
                                    if (units.SelectMany(u => u).Count(u => u is Elf) < elfCount)
                                    {
                                        elfDied = true;
                                        throw new CombatOverException();
                                    }
                                }
                            }
                            catch (CombatOverException coe)
                            {
                                combatOver = true;
                                break;
                            }
                        }
                    }

                    PrintGrid(units, startingCursorTop, startingCursorLeft, round);
                    if (!combatOver)
                        round++;
                }

                if (!elfDied)
                {
                    var hp = units.SelectMany(x => x).Sum(x => x.HitPoints);
                    Console.WriteLine($"Rounds: {round - 1}\nHP: {hp}");
                    return (round - 1) * hp;
                }
            }

            return -1;
        }

        private void PrintGrid(List<List<Unit>> units, int startingCursorTop, int startingCursorLeft, int round)
        {
            if (!WriteGrids)
            {
                Console.SetCursorPosition(startingCursorLeft, startingCursorTop);
                Console.WriteLine($"Round: {round}");
                return;
            }

            Console.SetCursorPosition(startingCursorLeft, startingCursorTop);
            var outputBuilder = new StringBuilder();
            for (var y = 0; y < units.Count; y++)
            {
                for (var x = 0; x < units[y].Count; x++)
                {
                    outputBuilder.Append(units[y][x].Character);
                }

                outputBuilder.AppendLine();
            }

            outputBuilder.AppendLine($"Round {round.ToString()}");
            Console.WriteLine(outputBuilder.ToString());

            if (WriteHP)
            {
                for (var y = 0; y < units.Count; y++)
                {
                    var hps = new List<string>();
                    for (var x = 0; x < units[y].Count; x++)
                    {
                        if (units[y][x].HitPoints > 0)
                        {
                            hps.Add($"{units[y][x].Character}({units[y][x].HitPoints})");
                        }
                    }

                    Console.SetCursorPosition(units.Count + 3, startingCursorTop + y);
                    Console.Write(string.Join(", ", hps).PadRight(50));
                }
                Console.SetCursorPosition(units.Count + 3, startingCursorTop + units.Count);
                Console.WriteLine($"Total HP: {units.SelectMany(x => x).Sum(x => x.HitPoints)}".PadRight(50));
            }

            if (WaitBetweenTurns)
                Console.ReadLine();
        }

        private bool ProcessAttacks(List<List<Unit>> units, Unit unit)
        {
            var enemiesInRange = unit.GetAdjacentEnemies(units);
            if (enemiesInRange.Any())
            {
                var minHp = enemiesInRange.Min(e => e.HitPoints);

                var enemy = enemiesInRange.FirstOrDefault(p => p.HitPoints == minHp && p.X == unit.X && p.Y == unit.Y - 1);
                if (enemy != null)
                    return MakeUnitAttackEnemy(units, unit, enemy);

                enemy = enemiesInRange.FirstOrDefault(p => p.HitPoints == minHp && p.X == unit.X - 1 && p.Y == unit.Y);
                if (enemy != null)
                    return MakeUnitAttackEnemy(units, unit, enemy);

                enemy = enemiesInRange.FirstOrDefault(p => p.HitPoints == minHp && p.X == unit.X + 1 && p.Y == unit.Y);
                if (enemy != null)
                    return MakeUnitAttackEnemy(units, unit, enemy);

                enemy = enemiesInRange.FirstOrDefault(p => p.HitPoints == minHp && p.X == unit.X && p.Y == unit.Y + 1);
                if (enemy != null)
                    return MakeUnitAttackEnemy(units, unit, enemy);
            }

            return false;
        }

        private bool MakeUnitAttackEnemy(List<List<Unit>> units, Unit attacker, Unit target)
        {
            target.HitPoints -= attacker.AttackPower;
            if (target.HitPoints <= 0)
            {
                units[target.Y][target.X] = new OpenSpace(target.X, target.Y);
                return true;
            }

            return false;
        }

        private Unit ProcessMoves(List<List<Unit>> units, Unit unit, int round)
        {
            if (unit.LastProcessedRound == round)
                return null;

            units[unit.Y][unit.X].LastProcessedRound = round;
            if (unit.GetAdjacentEnemies(units).Any())
                return unit;

            var targets = GetTargets(units, unit);
            if (!targets.Any())
                return unit;

            var paths = new List<Point>();
            var allSpaces = units.SelectMany(ul => ul.Where(u => u is OpenSpace)).Select(p => new Point(p.X, p.Y, false, int.MaxValue));
            foreach (var target in targets)
            {
                if (paths.Any() && GetManhattanDistance(unit, target) > paths.Min(p => p.Distance) + 1)
                    continue;

                var spaces = new List<Point>(allSpaces);
                var startingPoint = new Point(unit.X, unit.Y, false, int.MaxValue);
                spaces.Add(startingPoint);
                var destinationPoint = spaces.First(p => p.X == target.X && p.Y == target.Y);
                var path = FindShortestPath(spaces, startingPoint, destinationPoint);
                if (path != null)
                    paths.Add(path);
            }

            if (!paths.Any())
                return unit;

            var minDistance = paths.Min(p => p.Distance);
            var shortestPaths = paths.Where(p => p.Distance == minDistance).ToList();
            var to = shortestPaths.FirstOrDefault(p => p.X == unit.X && p.Y == unit.Y - 1);
            if (to != null)
                return SwapUnits(units, unit, units[to.Y][to.X]);

            to = shortestPaths.FirstOrDefault(p => p.X == unit.X - 1 && p.Y == unit.Y);
            if (to != null)
                return SwapUnits(units, unit, units[to.Y][to.X]);

            to = shortestPaths.FirstOrDefault(p => p.X == unit.X + 1 && p.Y == unit.Y);
            if (to != null)
                return SwapUnits(units, unit, units[to.Y][to.X]);

            to = shortestPaths.FirstOrDefault(p => p.X == unit.X && p.Y == unit.Y + 1);
            if (to != null)
                return SwapUnits(units, unit, units[to.Y][to.X]);

            return unit;
        }

        private int GetManhattanDistance(Unit from, Unit to)
        {
            return Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
        }

        private Unit SwapUnits(List<List<Unit>> units, Unit from, Unit to)
        {
            units[from.Y][from.X] = to;
            units[to.Y][to.X] = from;

            var toX = to.X;
            var toY = to.Y;
            to.X = from.X;
            to.Y = from.Y;
            from.X = toX;
            from.Y = toY;

            return from;
        }

        private Point FindShortestPath(List<Point> points, Point start, Point destination)
        {
            var unvisited = new HashSet<Point>(points);
            var current = destination;
            current.Distance = 0;
            do
            {
                CalculateDistances(unvisited, current);

                current.Visited = true;
                unvisited.Remove(current);

                if (start.Visited)
                    break;

                var minDistance = unvisited.Min(x => x.Distance);
                current = unvisited.First(p => p.Distance == minDistance);
            }
            while (!start.Visited && unvisited.Any(p => p.Distance < int.MaxValue));

            // Couldn't find a path between start and destination
            if (!start.Visited)
                return null;

            var to = start.Parents.FirstOrDefault(p => p.X == start.X && p.Y == start.Y - 1);
            if (to != null)
                return to;

            to = start.Parents.FirstOrDefault(p => p.X == start.X - 1 && p.Y == start.Y);
            if (to != null)
                return to;

            to = start.Parents.FirstOrDefault(p => p.X == start.X + 1 && p.Y == start.Y);
            if (to != null)
                return to;

            to = start.Parents.FirstOrDefault(p => p.X == start.X && p.Y == start.Y + 1);
            if (to != null)
                return to;

            return null;
        }

        private void CalculateDistances(HashSet<Point> unvisited, Point current)
        {
            var nextDistance = current.Distance + 1;
            var point = unvisited.FirstOrDefault(p => !p.Visited && p.X == current.X && p.Y == current.Y - 1);
            if (point != null)
            {
                if (nextDistance == point.Distance)
                {
                    point.Parents.Add(current);
                }
                else if (nextDistance < point.Distance)
                {
                    point.Distance = nextDistance;
                    point.Parents.Clear();
                    point.Parents.Add(current);
                }
            }

            point = unvisited.FirstOrDefault(p => !p.Visited && p.X == current.X - 1 && p.Y == current.Y);
            if (point != null)
            {
                if (nextDistance == point.Distance)
                {
                    point.Parents.Add(current);
                }
                else if (nextDistance < point.Distance)
                {
                    point.Distance = nextDistance;
                    point.Parents.Clear();
                    point.Parents.Add(current);
                }
            }

            point = unvisited.FirstOrDefault(p => !p.Visited && p.X == current.X + 1 && p.Y == current.Y);
            if (point != null)
            {
                if (nextDistance == point.Distance)
                {
                    point.Parents.Add(current);
                }
                else if (nextDistance < point.Distance)
                {
                    point.Distance = nextDistance;
                    point.Parents.Clear();
                    point.Parents.Add(current);
                }
            }

            point = unvisited.FirstOrDefault(p => !p.Visited && p.X == current.X && p.Y == current.Y + 1);
            if (point != null)
            {
                if (nextDistance == point.Distance)
                {
                    point.Parents.Add(current);
                }
                else if (nextDistance < point.Distance)
                {
                    point.Distance = nextDistance;
                    point.Parents.Clear();
                    point.Parents.Add(current);
                }
            }
        }

        private List<Unit> GetTargets(List<List<Unit>> units, Unit currentUnit)
        {
            var targets = new List<Unit>();
            var enemies = units.SelectMany(x => x).Where(x => x.GetType().Equals(currentUnit.Enemy));
            if (!enemies.Any())
                throw new CombatOverException();

            return enemies.SelectMany(x => x.GetAdjacentSpaces(units)).OrderBy(x => GetManhattanDistance(currentUnit, x)).ToList();
        }

        private List<List<Unit>> GetUnits(string[] input, int elfAttack = 3)
        {
            var units = new List<List<Unit>>();
            for (var y = 0; y < input.Length; y++)
            {
                var lineUnits = new List<Unit>();
                for (var x = 0; x < input[y].Length; x++)
                {
                    var point = input[y][x];
                    switch (point)
                    {
                        case '#':
                            lineUnits.Add(new Wall(x, y));
                            break;
                        case '.':
                            lineUnits.Add(new OpenSpace(x, y));
                            break;
                        case 'G':
                            lineUnits.Add(new Goblin(x, y));
                            break;
                        case 'E':
                            lineUnits.Add(new Elf(x, y, elfAttack));
                            break;
                    }
                }

                units.Add(lineUnits);
            }

            return units;
        }

        public static void Run()
        {
            var input = GetInput().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var examples = new List<string>
            {
                @"
#######
#.G...#
#...EG#
#.#.#G#
#..G#E#
#.....#
#######",
                @"
#######
#G..#E#
#E#E.E#
#G.##.#
#...#E#
#...E.#
#######",
                @"
#######
#E..EG#
#.#G.E#
#E.##E#
#G..#.#
#..E#.#
#######",
                @"
#######
#E.G#.#
#.#G..#
#G.#.G#
#G..#.#
#...E.#
#######",
                @"
#######
#.E...#
#.#..G#
#.###.#
#E#G#G#
#...#G#
#######",
                @"
#########
#G......#
#.E.#...#
#..##..G#
#...##..#
#...#...#
#.G...G.#
#.....G.#
#########"
            };

            var Day15 = new Day15();
            Console.WriteLine("\n###############\n###############\nDay15\n###############\n###############\n");
            //Console.WriteLine("\n###############\nPart 1\n###############\n");
            //Console.WriteLine(Day15.Part1(examples[1].Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)));
            /*foreach (var example in examples)
            {
                Console.WriteLine(Day15.Part1(example.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)));
            }*/
            //Console.WriteLine(Day15.Part1(input));
            
            Console.WriteLine("\n###############\nPart 2\n###############\n");
            /*foreach (var example in examples)
            {
                Console.WriteLine(Day15.Part2(example.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)));
            }*/
            Console.WriteLine(Day15.Part2(input));
        }

        private static string GetInput()
        {
            return @"
################################
#########################.G.####
#########################....###
##################.G.........###
##################.##.......####
#################...#.........##
################..............##
######..########...G...#.#....##
#####....######.G.GG..G..##.####
#######.#####G............#.####
#####.........G..G......#...####
#####..G......G..........G....##
######GG......#####........E.###
#######......#######..........##
######...G.G#########........###
######......#########.....E..###
#####.......#########........###
#####....G..#########........###
######.##.#.#########......#####
#######......#######.......#####
#######.......#####....E...#####
##.G..#.##............##.....###
#.....#........###..#.#.....####
#.........E.E...#####.#.#....###
######......#.....###...#.#.E###
#####........##...###..####..###
####...G#.##....E####E.####...##
####.#########....###E.####....#
###...#######.....###E.####....#
####..#######.##.##########...##
####..######################.###
################################";
        }
    }

    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool Visited { get; set; }
        public int Distance { get; set; }
        public List<Point> Parents { get; private set; }

        public Point(int x, int y, bool visited, int distance)
        {
            X = x;
            Y = y;
            Visited = visited;
            Distance = distance;
            Parents = new List<Point>();
        }
    }

    abstract class Unit
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int HitPoints { get; set; }
        public int Moves { get; private set; }
        public int AttackPower { get; private set; }
        public int RoundLastProcessed { get; set; }
        public Type Enemy { get; set; }
        public char Character { get; set; }
        public int LastProcessedRound { get; set; }

        public Unit(int x, int y, Type enemy, int hitPoints = 0, int moves = 0, int attackPower = 0)
        {
            X = x;
            Y = y;
            Enemy = enemy;
            HitPoints = hitPoints;            
            Moves = moves;
            AttackPower = attackPower;            
        }

        public List<Unit> GetAdjacentSpaces(List<List<Unit>> units)
        {
            var adjUnits = new List<Unit>();
            if (Y > 0 && units[Y - 1][X] is OpenSpace)
                adjUnits.Add(units[Y - 1][X]);
            if (Y < units.Count - 1 && units[Y + 1][X] is OpenSpace)
                adjUnits.Add(units[Y + 1][X]);
            if (X > 0 && units[Y][X - 1] is OpenSpace)
                adjUnits.Add(units[Y][X - 1]);
            if (X < units.Count - 1 && units[Y][X + 1] is OpenSpace)
                adjUnits.Add(units[Y][X + 1]);

            return adjUnits;
        }

        public List<Unit> GetAdjacentEnemies(List<List<Unit>> units)
        {
            var adjUnits = new List<Unit>();
            if (Y > 0 && units[Y - 1][X].GetType().Equals(Enemy))
                adjUnits.Add(units[Y - 1][X]);
            if (Y < units.Count - 1 && units[Y + 1][X].GetType().Equals(Enemy))
                adjUnits.Add(units[Y + 1][X]);
            if (X > 0 && units[Y][X - 1].GetType().Equals(Enemy))
                adjUnits.Add(units[Y][X - 1]);
            if (X < units.Count - 1 && units[Y][X + 1].GetType().Equals(Enemy))
                adjUnits.Add(units[Y][X + 1]);

            return adjUnits;
        }
    }

    class Elf : Unit
    {
        public Elf(int x, int y, int attackPower) : base(x, y, typeof(Goblin), 200, 1, attackPower)
        {
            Character = 'E';
        }
    }

    class Goblin : Unit
    {
        public Goblin(int x, int y) : base(x, y, typeof(Elf), 200, 1, 3)
        {
            Character = 'G';
        }
    }

    class Wall : Unit
    {
        public Wall(int x, int y) : base(x, y, typeof(Wall))
        {
            Character = '#';
        }
    }

    class OpenSpace : Unit
    {
        public OpenSpace(int x, int y) : base(x, y, typeof(OpenSpace))
        {
            Character = '.';
        }
    }

    class CombatOverException : Exception
    {
    }
}
