using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AdventOfCode2019
{
    class Day11 : BaseDay
    {
        private const char BLACK = '.';
        private const char WHITE = '#';

        public Day11()
        {
            Sample = @"";

            Input = @"3,8,1005,8,328,1106,0,11,0,0,0,104,1,104,0,3,8,102,-1,8,10,101,1,10,10,4,10,108,1,8,10,4,10,101,0,8,28,1006,0,13,3,8,102,-1,8,10,101,1,10,10,4,10,1008,8,1,10,4,10,1002,8,1,54,1,1103,9,10,1006,0,97,2,1003,0,10,1,105,6,10,3,8,102,-1,8,10,1001,10,1,10,4,10,1008,8,1,10,4,10,1001,8,0,91,3,8,102,-1,8,10,101,1,10,10,4,10,1008,8,0,10,4,10,102,1,8,113,2,109,5,10,1006,0,96,1,2,5,10,3,8,1002,8,-1,10,101,1,10,10,4,10,1008,8,0,10,4,10,102,1,8,146,2,103,2,10,1006,0,69,2,9,8,10,1006,0,25,3,8,102,-1,8,10,1001,10,1,10,4,10,1008,8,0,10,4,10,101,0,8,182,3,8,1002,8,-1,10,101,1,10,10,4,10,108,1,8,10,4,10,1001,8,0,203,2,5,9,10,1006,0,0,2,6,2,10,3,8,102,-1,8,10,101,1,10,10,4,10,108,1,8,10,4,10,1002,8,1,236,2,4,0,10,3,8,1002,8,-1,10,1001,10,1,10,4,10,1008,8,0,10,4,10,1002,8,1,263,2,105,9,10,1,103,15,10,1,4,4,10,2,109,7,10,3,8,1002,8,-1,10,101,1,10,10,4,10,1008,8,0,10,4,10,1001,8,0,301,1006,0,63,2,105,6,10,101,1,9,9,1007,9,1018,10,1005,10,15,99,109,650,104,0,104,1,21102,387508441116,1,1,21102,1,345,0,1106,0,449,21102,1,387353256852,1,21102,1,356,0,1105,1,449,3,10,104,0,104,1,3,10,104,0,104,0,3,10,104,0,104,1,3,10,104,0,104,1,3,10,104,0,104,0,3,10,104,0,104,1,21101,179410308315,0,1,21102,1,403,0,1106,0,449,21101,206199495827,0,1,21102,414,1,0,1105,1,449,3,10,104,0,104,0,3,10,104,0,104,0,21102,718086758760,1,1,21102,1,437,0,1105,1,449,21101,838429573908,0,1,21102,448,1,0,1106,0,449,99,109,2,21202,-1,1,1,21102,1,40,2,21102,480,1,3,21101,470,0,0,1105,1,513,109,-2,2105,1,0,0,1,0,0,1,109,2,3,10,204,-1,1001,475,476,491,4,0,1001,475,1,475,108,4,475,10,1006,10,507,1102,0,1,475,109,-2,2106,0,0,0,109,4,2101,0,-1,512,1207,-3,0,10,1006,10,530,21101,0,0,-3,21202,-3,1,1,21201,-2,0,2,21102,1,1,3,21102,549,1,0,1105,1,554,109,-4,2106,0,0,109,5,1207,-3,1,10,1006,10,577,2207,-4,-2,10,1006,10,577,22102,1,-4,-4,1106,0,645,22102,1,-4,1,21201,-3,-1,2,21202,-2,2,3,21101,596,0,0,1106,0,554,22101,0,1,-4,21102,1,1,-1,2207,-4,-2,10,1006,10,615,21101,0,0,-1,22202,-2,-1,-2,2107,0,-3,10,1006,10,637,21201,-1,0,1,21101,637,0,0,106,0,512,21202,-2,-1,-2,22201,-4,-2,-4,109,-5,2106,0,0";
        }

        public new string Part1()
        {
            var panels = new Dictionary<Point, char>();
            var computer = new IntCodeComputer(Input, 0, false);
            var robot = new Robot(computer);
            while (robot.Computer.State != State.Halt)
            {
                robot.Computer.Input = panels.ContainsKey(robot.Location) && panels[robot.Location] == WHITE ? 1 : 0;
                var color = robot.Computer.RunToOutput();
                if (!color.HasValue)
                    continue;

                panels[robot.Location] = color.Value == 0L ? BLACK : WHITE;

                var direction = robot.Computer.RunToOutput();
                if (!direction.HasValue)
                    continue;

                robot.Turn(direction.Value == 0 ? Direction.Left : Direction.Right);
            }

            return panels.Count.ToString();
        }

        public new string Part2()
        {
            var panels = new Dictionary<Point, char>();
            var computer = new IntCodeComputer(Input, 0, false);
            var robot = new Robot(computer);
            while (robot.Computer.State != State.Halt)
            {
                robot.Computer.Input = panels.ContainsKey(robot.Location) && panels[robot.Location] == WHITE ? 1 : 0;
                if (panels.Count == 0)
                    robot.Computer.Input = WHITE;

                var color = robot.Computer.RunToOutput();
                if (!color.HasValue)
                    continue;

                panels[robot.Location] = color.Value == 0L ? BLACK : WHITE;

                var direction = robot.Computer.RunToOutput();
                if (!direction.HasValue)
                    continue;

                robot.Turn(direction.Value == 0 ? Direction.Left : Direction.Right);
            }

            var buffer = 2;
            var minX = panels.Min(p => p.Key.X) - buffer;
            var maxX = panels.Max(p => p.Key.X) + buffer;
            var minY = panels.Min(p => p.Key.Y) - buffer;
            var maxY = panels.Max(p => p.Key.Y) + buffer;

            var output = "";
            for (var y = maxY; y >= minY; y--)
            {
                for (var x = minX; x <= maxX; x++)
                    output += panels.TryGetValue(new Point(x, y), out var color) ? color : BLACK;

                output += '\n';
            }

            return output;
        }
    }

    class Robot

    {
        public IntCodeComputer Computer { get; private set; }
        public Direction Direction { get; set; }
        public Point Location;

        public Robot(IntCodeComputer computer)
        {
            Computer = computer;
            Direction = Direction.Up;
            Location = new Point(0, 0);
        }

        public void Turn(Direction direction)
        {
            if (direction == Direction.Left)
            {
                Direction = Direction == Direction.Right ? Direction.Up : Direction + 1;
            }
            else if (direction == Direction.Right)
            {
                Direction = Direction == Direction.Up ? Direction.Right : Direction - 1;
            }
            else
                throw new Exception("Robot can't turn up or down.");

            switch (Direction)
            {
                case Direction.Up:
                    Location.Y++;
                    break;
                case Direction.Left:
                    Location.X--;
                    break;
                case Direction.Down:
                    Location.Y--;
                    break;
                case Direction.Right:
                    Location.X++;
                    break;
            }
        }
    }

    enum Direction
    {
        Up = 0,
        Left = 1,
        Down = 2,
        Right = 3
    }
}
