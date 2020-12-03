using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2019
{
    class Day12 : BaseDay
    {
        public Day12()
        {
            Sample = @"<x=-8, y=-10, z=0>
<x=5, y=5, z=10>
<x=2, y=-7, z=3>
<x=9, y=-8, z=-3>";

            Input = @"<x=-4, y=-9, z=-3>
<x=-13, y=-11, z=0>
<x=-17, y=-7, z=15>
<x=-16, y=4, z=2>";
        }

        public new string Part1()
        {
            var stepCount = 1000;
            var moons = Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(i => new Moon(i)).ToList();
            for (var i = 0; i < stepCount; i++)
            {
                foreach (var moon in moons)
                {
                    foreach (var counterMoon in moons.Where(m => m != moon))
                    {
                        var comparison = moon.Position.X.CompareTo(counterMoon.Position.X);
                        if (comparison > 0)
                            moon.Velocity.X--;
                        else if (comparison < 0)
                            moon.Velocity.X++;

                        comparison = moon.Position.Y.CompareTo(counterMoon.Position.Y);
                        if (comparison > 0)
                            moon.Velocity.Y--;
                        else if (comparison < 0)
                            moon.Velocity.Y++;

                        comparison = moon.Position.Z.CompareTo(counterMoon.Position.Z);
                        if (comparison > 0)
                            moon.Velocity.Z--;
                        else if (comparison < 0)
                            moon.Velocity.Z++;
                    }
                }

                foreach (var moon in moons)
                {
                    moon.Position.X += moon.Velocity.X;
                    moon.Position.Y += moon.Velocity.Y;
                    moon.Position.Z += moon.Velocity.Z;
                }
            }

            return moons.Sum(m => m.TotalEnergy).ToString();
        }

        public new string Part2()
        {
            var stepCount = int.MaxValue;
            var moons = Input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(i => new Moon(i)).ToList();
            var initialState = moons.Select(m => new Point3D(m.Position)).ToList();
            var cycles = new Point3D(0, 0, 0);
            var counter = 0;
            while (counter < stepCount)
            {
                foreach (var moon in moons)
                {
                    foreach (var counterMoon in moons.Where(m => m != moon))
                    {
                        var comparison = moon.Position.X.CompareTo(counterMoon.Position.X);
                        if (comparison > 0)
                            moon.Velocity.X--;
                        else if (comparison < 0)
                            moon.Velocity.X++;

                        comparison = moon.Position.Y.CompareTo(counterMoon.Position.Y);
                        if (comparison > 0)
                            moon.Velocity.Y--;
                        else if (comparison < 0)
                            moon.Velocity.Y++;

                        comparison = moon.Position.Z.CompareTo(counterMoon.Position.Z);
                        if (comparison > 0)
                            moon.Velocity.Z--;
                        else if (comparison < 0)
                            moon.Velocity.Z++;
                    }
                }

                foreach (var moon in moons)
                {
                    moon.Position.X += moon.Velocity.X;
                    moon.Position.Y += moon.Velocity.Y;
                    moon.Position.Z += moon.Velocity.Z;
                }

                counter++;
                if (cycles.X == 0 && moons.All(m => m.IsInitialX))
                    cycles.X = counter;

                if (cycles.Y == 0 && moons.All(m => m.IsInitialY))
                    cycles.Y = counter;

                if (cycles.Z == 0 && moons.All(m => m.IsInitialZ))
                    cycles.Z = counter;

                if (cycles.X != 0 && cycles.Y != 0 && cycles.Z != 0)
                    break;
            }

            return LCM(new List<long> { cycles.X, cycles.Y, cycles.Z }).ToString();
        }

        private static long LCM(IEnumerable<long> numbers)
        {
            return numbers.Aggregate(LCM);
        }

        private static long LCM(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }

        private static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }

    class Moon
    {
        public Point3D Position { get; set; }
        public Point3D Velocity { get; set; }
        public Point3D InitialPosition { get; private set; }

        public Moon(string input)
        {
            var matchGroups = Regex.Match(input, @"<x=([-\d]+),\s*y=([-\d]+),\s*z=([-\d]+)>").Groups;
            Position = new Point3D(int.Parse(matchGroups[1].Value), int.Parse(matchGroups[2].Value), int.Parse(matchGroups[3].Value));
            Velocity = new Point3D(0, 0, 0);
            InitialPosition = new Point3D(Position);
        }

        public int PotentialEnergy => Math.Abs(Position.X) + Math.Abs(Position.Y) + Math.Abs(Position.Z);
        public int KineticEnergy => Math.Abs(Velocity.X) + Math.Abs(Velocity.Y) + Math.Abs(Velocity.Z);
        public int TotalEnergy => PotentialEnergy * KineticEnergy;
        public bool IsInitialX => Position.X == InitialPosition.X && Velocity.X == 0;
        public bool IsInitialY => Position.Y == InitialPosition.Y && Velocity.Y == 0;
        public bool IsInitialZ => Position.Z == InitialPosition.Z && Velocity.Z == 0;

        public override string ToString() => $"pos=<x={Position.X.ToString().PadLeft(3)}, y={Position.Y.ToString().PadLeft(3)}, z={Position.Z.ToString().PadLeft(3)}>, vel=<x={Velocity.X.ToString().PadLeft(3)}, y={Velocity.Y.ToString().PadLeft(3)}, z={Velocity.Z.ToString().PadLeft(3)}>";
    }

    class Point3D
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Point3D(Point3D point)
        {
            X = point.X;
            Y = point.Y;
            Z = point.Z;
        }

        public override string ToString() => $"<x={X.ToString().PadLeft(3)}, y={Y.ToString().PadLeft(3)}, z={Z.ToString().PadLeft(3)}>";

        public override bool Equals(object obj) => obj is Point3D point ? X == point.X && Y == point.Y && Z == point.Z : false;

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }
    }
}
