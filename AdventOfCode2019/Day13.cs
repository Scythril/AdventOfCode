﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace AdventOfCode2019
{
    class Day13 : BaseDay
    {
        private readonly bool Draw = true;

        public Day13()
        {
            Sample = @"1,2,3,6,5,4";

            Input = @"1,380,379,385,1008,2559,522801,381,1005,381,12,99,109,2560,1102,1,0,383,1101,0,0,382,20101,0,382,1,21001,383,0,2,21102,37,1,0,1106,0,578,4,382,4,383,204,1,1001,382,1,382,1007,382,40,381,1005,381,22,1001,383,1,383,1007,383,24,381,1005,381,18,1006,385,69,99,104,-1,104,0,4,386,3,384,1007,384,0,381,1005,381,94,107,0,384,381,1005,381,108,1106,0,161,107,1,392,381,1006,381,161,1101,0,-1,384,1105,1,119,1007,392,38,381,1006,381,161,1102,1,1,384,20101,0,392,1,21102,22,1,2,21101,0,0,3,21102,1,138,0,1106,0,549,1,392,384,392,20102,1,392,1,21102,1,22,2,21101,0,3,3,21101,0,161,0,1106,0,549,1101,0,0,384,20001,388,390,1,20101,0,389,2,21101,0,180,0,1106,0,578,1206,1,213,1208,1,2,381,1006,381,205,20001,388,390,1,20102,1,389,2,21101,0,205,0,1105,1,393,1002,390,-1,390,1101,1,0,384,20102,1,388,1,20001,389,391,2,21101,228,0,0,1105,1,578,1206,1,261,1208,1,2,381,1006,381,253,21002,388,1,1,20001,389,391,2,21102,1,253,0,1105,1,393,1002,391,-1,391,1102,1,1,384,1005,384,161,20001,388,390,1,20001,389,391,2,21101,0,279,0,1106,0,578,1206,1,316,1208,1,2,381,1006,381,304,20001,388,390,1,20001,389,391,2,21101,0,304,0,1105,1,393,1002,390,-1,390,1002,391,-1,391,1102,1,1,384,1005,384,161,20102,1,388,1,21001,389,0,2,21101,0,0,3,21102,1,338,0,1105,1,549,1,388,390,388,1,389,391,389,20101,0,388,1,21002,389,1,2,21101,4,0,3,21101,0,365,0,1106,0,549,1007,389,23,381,1005,381,75,104,-1,104,0,104,0,99,0,1,0,0,0,0,0,0,335,18,19,1,1,20,109,3,21201,-2,0,1,21202,-1,1,2,21102,0,1,3,21102,1,414,0,1106,0,549,22101,0,-2,1,21202,-1,1,2,21102,429,1,0,1106,0,601,1202,1,1,435,1,386,0,386,104,-1,104,0,4,386,1001,387,-1,387,1005,387,451,99,109,-3,2105,1,0,109,8,22202,-7,-6,-3,22201,-3,-5,-3,21202,-4,64,-2,2207,-3,-2,381,1005,381,492,21202,-2,-1,-1,22201,-3,-1,-3,2207,-3,-2,381,1006,381,481,21202,-4,8,-2,2207,-3,-2,381,1005,381,518,21202,-2,-1,-1,22201,-3,-1,-3,2207,-3,-2,381,1006,381,507,2207,-3,-4,381,1005,381,540,21202,-4,-1,-1,22201,-3,-1,-3,2207,-3,-4,381,1006,381,529,22102,1,-3,-7,109,-8,2105,1,0,109,4,1202,-2,40,566,201,-3,566,566,101,639,566,566,2101,0,-1,0,204,-3,204,-2,204,-1,109,-4,2106,0,0,109,3,1202,-1,40,594,201,-2,594,594,101,639,594,594,20102,1,0,-2,109,-3,2106,0,0,109,3,22102,24,-2,1,22201,1,-1,1,21101,487,0,2,21101,0,357,3,21102,960,1,4,21101,630,0,0,1105,1,456,21201,1,1599,-2,109,-3,2106,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,2,0,0,2,2,2,2,0,2,0,0,2,0,0,2,2,2,2,0,0,0,0,0,2,0,2,0,2,2,2,0,2,0,0,0,1,1,0,2,2,2,0,2,2,0,0,2,0,2,2,2,2,2,2,0,0,2,2,0,0,0,2,2,0,0,2,2,0,2,2,2,2,0,2,0,1,1,0,0,2,2,0,2,2,2,2,0,2,2,2,2,0,2,0,2,2,0,0,2,2,2,0,2,2,2,2,2,2,0,0,2,2,0,0,0,1,1,0,2,2,2,2,0,2,2,2,2,0,2,2,2,2,0,2,2,0,0,0,2,0,0,0,0,0,2,0,0,2,2,0,2,0,0,2,0,1,1,0,2,2,2,2,2,0,2,0,2,0,2,0,2,2,2,2,0,2,0,2,2,2,0,2,2,2,2,2,2,0,2,2,2,0,2,0,0,1,1,0,0,2,0,2,2,2,2,0,2,0,0,2,0,2,2,0,0,0,0,0,2,2,2,0,0,2,0,2,2,0,2,2,0,0,0,0,0,1,1,0,0,2,0,0,2,2,2,2,2,2,2,2,2,2,2,0,2,0,0,2,2,2,2,2,0,2,2,2,0,0,2,0,0,0,2,0,0,1,1,0,0,2,2,0,2,0,2,2,2,0,0,0,0,0,2,2,2,2,2,2,2,0,0,2,0,0,2,0,2,2,2,2,2,0,2,2,0,1,1,0,0,2,2,2,2,0,2,0,0,0,2,2,2,0,2,0,2,0,0,0,0,0,0,2,2,0,2,2,0,0,2,2,2,2,2,2,0,1,1,0,0,0,2,0,2,0,0,2,0,2,0,2,0,0,2,0,2,2,0,0,2,0,0,0,0,0,0,0,2,2,0,2,2,2,2,0,0,1,1,0,0,2,2,2,0,2,0,2,0,2,2,2,2,2,2,2,2,2,2,0,2,0,0,0,2,0,2,0,2,0,2,2,2,2,2,2,0,1,1,0,2,0,2,0,2,2,0,0,2,2,2,2,2,2,2,0,2,2,0,0,0,2,2,0,2,2,0,0,2,0,2,2,2,2,0,0,0,1,1,0,0,2,0,0,2,2,0,0,0,0,0,0,0,2,0,0,0,0,0,2,0,2,2,2,0,0,2,0,2,2,0,2,2,2,2,2,0,1,1,0,2,0,0,2,0,0,2,2,2,0,0,0,2,2,2,2,2,2,2,0,2,2,2,2,2,2,0,0,2,2,0,2,0,0,2,2,0,1,1,0,2,2,0,0,2,2,2,2,0,2,2,2,2,0,0,0,0,2,0,2,2,2,0,2,0,0,0,2,0,2,0,0,0,0,2,0,0,1,1,0,2,2,0,2,2,2,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,0,0,0,0,2,0,2,2,2,2,0,2,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,19,43,18,35,93,6,48,32,25,41,42,15,84,68,59,10,84,24,11,98,75,16,82,37,44,81,39,8,15,91,76,16,59,25,66,58,84,86,59,56,79,78,59,26,51,57,84,11,7,2,50,84,84,73,41,79,7,57,44,96,64,17,22,83,83,20,8,18,34,78,75,31,8,14,17,68,74,60,70,26,84,96,58,42,60,70,50,38,66,51,93,79,40,54,11,26,82,76,39,63,51,9,86,32,9,4,88,73,74,15,75,4,6,2,79,83,46,85,95,21,9,48,43,89,15,81,44,86,95,40,95,20,76,9,42,81,18,85,55,71,24,57,27,27,98,27,54,66,48,55,65,19,3,98,70,76,21,75,74,46,74,46,15,52,58,9,66,94,35,93,6,25,63,33,77,23,40,97,69,20,57,71,46,70,50,80,31,66,71,37,80,94,40,59,54,64,3,93,41,44,35,69,89,18,78,17,85,45,25,27,89,17,27,62,90,50,77,52,3,58,57,27,20,62,24,38,93,55,43,63,85,55,79,21,10,71,36,51,39,53,90,12,83,79,53,35,1,54,14,79,44,26,29,26,77,14,1,19,18,71,68,59,92,89,74,2,93,60,25,60,39,5,38,91,23,56,77,97,21,52,96,20,7,83,66,62,69,46,33,46,49,41,43,36,86,35,54,36,77,59,65,71,7,98,12,56,29,91,50,76,80,53,55,40,91,89,81,87,74,53,59,43,68,80,87,68,2,96,37,16,85,66,49,44,48,4,23,27,22,87,60,44,96,42,36,79,37,59,97,86,81,37,47,23,10,55,36,43,46,51,86,43,15,13,74,56,26,97,23,51,12,57,97,53,31,28,44,46,27,75,8,18,13,22,89,62,71,62,96,66,60,25,7,50,79,61,84,14,67,98,86,62,16,49,30,18,87,3,58,3,22,73,93,10,35,3,68,67,88,50,92,9,67,66,40,14,93,72,52,71,32,22,52,30,36,83,52,84,72,7,91,77,78,2,25,70,18,1,65,45,26,57,41,44,71,5,63,68,31,77,87,67,42,1,87,72,59,47,31,25,34,63,40,23,47,31,66,54,31,26,16,32,46,22,93,48,86,16,34,85,66,88,33,10,83,63,81,47,35,4,47,64,15,67,88,58,4,58,68,26,88,35,24,50,7,63,1,89,74,26,96,58,54,85,42,18,90,40,53,52,33,92,52,33,42,88,35,48,69,83,89,12,90,78,76,30,63,15,9,4,46,85,44,85,16,39,63,79,29,21,72,24,19,87,68,24,80,22,62,52,29,27,58,45,14,51,80,3,22,67,67,55,41,63,32,12,72,32,66,55,7,93,50,20,62,38,94,81,88,29,30,72,78,91,72,8,62,7,12,47,23,52,58,60,29,9,75,84,11,76,58,69,42,76,8,89,19,98,45,84,95,15,96,67,38,7,18,30,33,9,62,85,19,82,60,34,77,19,77,31,77,69,55,60,51,31,25,32,86,85,51,51,69,2,97,81,80,14,27,89,10,7,57,36,49,17,91,58,9,34,50,13,44,60,23,4,82,14,11,21,43,31,87,24,9,10,61,64,13,58,28,47,34,40,43,11,41,82,39,17,16,56,11,39,62,41,52,63,5,67,25,90,36,38,73,96,57,80,82,9,45,57,31,65,51,19,59,11,60,42,13,45,81,50,17,82,87,79,92,2,64,40,69,68,44,52,98,10,83,92,46,78,24,34,86,93,48,67,73,18,82,15,77,22,15,55,17,85,42,62,22,69,10,54,51,85,81,72,88,69,34,78,33,53,52,3,46,55,17,77,86,78,21,66,36,42,82,6,79,24,59,87,22,93,44,51,11,38,33,33,77,38,7,13,58,22,52,19,84,45,17,66,52,98,50,47,42,85,80,84,83,21,3,61,22,49,36,59,23,25,79,19,12,70,33,3,28,18,40,92,87,59,12,98,62,29,58,74,52,85,78,30,21,22,60,42,43,39,26,42,47,90,79,20,91,24,70,89,97,2,32,94,73,17,10,45,34,23,6,37,57,8,92,93,31,72,47,87,69,91,54,52,95,79,18,5,30,18,57,4,96,78,15,69,28,47,16,31,75,45,12,22,30,4,63,75,3,30,64,98,47,86,53,82,18,57,10,36,53,31,94,6,10,58,1,61,37,20,60,32,522801";
        }

        public new string Part1()
        {
            var computer = new IntCodeComputer(Input);
            var tiles = new Dictionary<Point, TileType>();
            while (computer.State != State.Halt)
            {
                var output = computer.RunToOutput();
                if (!output.HasValue)
                    break;
                
                var x = output.Value;

                output = computer.RunToOutput();
                if (!output.HasValue)
                    break;
                
                var y = output.Value;

                output = computer.RunToOutput();
                if (!output.HasValue)
                    break;
                
                var tileId = output.Value;

                tiles[new Point(Convert.ToInt32(x), Convert.ToInt32(y))] = Enum.Parse<TileType>(tileId.ToString());
            }

            return tiles.Count(t => t.Value == TileType.Block).ToString();
        }

        public new string Part2()
        {
            var computer = new IntCodeComputer(Input);
            computer.Mem[0L] = 2;
            var tiles = new Dictionary<Point, TileType>();
            var score = 0L;
            var ballLocation = new Point();
            var paddleLocation = new Point();
            var origin = new Point(Console.CursorLeft, Console.CursorTop);
            var originCursorVisible = Console.CursorVisible;
            var gameStarted = false;
            try
            {
                if (Draw)
                    Console.CursorVisible = false;

                while (computer.State != State.Halt)
                {
                    var output = computer.RunToOutput();
                    if (!output.HasValue)
                        break;

                    var x = output.Value;

                    output = computer.RunToOutput();
                    if (!output.HasValue)
                        break;

                    var y = output.Value;

                    output = computer.RunToOutput();
                    if (!output.HasValue)
                        break;

                    var tileId = output.Value;

                    if (x == -1L && y == 0L)
                    {
                        score = tileId;
                        gameStarted = true;
                        if (Draw)
                        {
                            Console.SetCursorPosition(origin.X + tiles.Max(t => t.Key.X) + 5, origin.Y);
                            Console.Write($"Score: {score}".PadRight(20));
                        }
                    }
                    else
                    {
                        var location = new Point(Convert.ToInt32(x), Convert.ToInt32(y));
                        var tileType = Enum.Parse<TileType>(tileId.ToString());
                        tiles[location] = tileType;
                        if (tileType == TileType.Ball)
                            ballLocation = location;
                        else if (tileType == TileType.Paddle)
                            paddleLocation = location;

                        DrawTile(origin, location, tileType, gameStarted);
                    }

                    if (ballLocation.X < paddleLocation.X)
                        computer.Input = -1L;
                    else if (ballLocation.X > paddleLocation.X)
                        computer.Input = 1L;
                    else
                        computer.Input = 0L;
                }
            }
            finally
            {
                if (Draw)
                {
                    Console.SetCursorPosition(0, origin.Y + tiles.Max(t => t.Key.Y) + 2);
                    Console.CursorVisible = originCursorVisible;
                }
            }

            return score.ToString();
        }

        private void DrawTile(Point origin, Point location, TileType tileType, bool gameStarted)
        {
            if (!Draw)
                return;

            if (gameStarted)
                Thread.Sleep(10);

            Console.SetCursorPosition(origin.X + location.X, origin.Y + location.Y);
            switch (tileType)
            {
                case TileType.Empty:
                    Console.Write(' ');
                    break;
                case TileType.Wall:
                    Console.Write('H');
                    break;
                case TileType.Block:
                    Console.Write('#');
                    break;
                case TileType.Paddle:
                    Console.Write('=');
                    break;
                case TileType.Ball:
                    Console.Write('O');
                    break;
            }
        }
    }

    enum TileType
    {
        Empty = 0,
        Wall = 1, // @
        Block = 2, // #
        Paddle = 3, // =
        Ball = 4 // *
    }
}