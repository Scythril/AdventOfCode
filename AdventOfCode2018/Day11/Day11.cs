using System;
using System.Collections.Generic;

namespace AdventOfCode2018.Day11
{
    class Day11
    {
        public string Part1(int input)
        {
            var grid = GetGrid(input);

            FuelCell maxCell = null;
            var maxPowerLevel = 0;
            for (var y = 0; y <= 297; y++)
            {
                for (var x = 0; x <= 297; x++)
                {
                    var powerLevel = GetCumulativePowerLevel(grid, grid[y][x]);
                    if (powerLevel > maxPowerLevel)
                    {
                        maxPowerLevel = powerLevel;
                        maxCell = grid[y][x];
                    }
                }
            }

            return $"{maxCell.X},{maxCell.Y}";
        }

        public string Part2(int input)
        {
            var grid = GetGrid(input);

            FuelCell maxCell = null;
            var maxPowerLevel = 0;
            var maxSize = 1;
            for (var size = 1; size <= 300; size++)
            {
                for (var y = 0; y <= (300 - size); y++)
                {
                    for (var x = 0; x <= (300 - size); x++)
                    {
                        var powerLevel = GetCumulativePowerLevel(grid, grid[y][x], size);
                        if (powerLevel > maxPowerLevel)
                        {
                            maxPowerLevel = powerLevel;
                            maxCell = grid[y][x];
                            maxSize = size;
                        }
                    }
                }
            }

            return $"{maxCell.X},{maxCell.Y},{maxSize}";
        }

        private int GetCumulativePowerLevel(List<List<FuelCell>> grid, FuelCell fuelCell, int size = 3)
        {
            var powerLevel = 0;
            for (var y = -1; y < (size - 1); y++)
            {
                for (var x = -1; x < (size - 1); x++)
                {
                    powerLevel += grid[y + fuelCell.Y][x + fuelCell.X].PowerLevel;
                }
            }

            return powerLevel;
        }

        private void SetPowerLevel(FuelCell fuelCell, int serialNumber)
        {
            var rackId = fuelCell.X + 10;
            var powerLevel = (rackId * fuelCell.Y + serialNumber) * rackId;
            if (powerLevel < 100)
            {
                powerLevel = -5;
            }
            else
            {
                powerLevel = (powerLevel / 100 % 10) - 5;
            }

            fuelCell.PowerLevel = powerLevel;
        }

        private List<List<FuelCell>> GetGrid(int serialNumber)
        {
            var grid = new List<List<FuelCell>>(300);
            for (var y = 1; y <= 300; y++)
            {
                var row = new List<FuelCell>(300);
                for (var x = 1; x <= 300; x++)
                {
                    var fuelCell = new FuelCell(x, y);
                    SetPowerLevel(fuelCell, serialNumber);
                    row.Add(fuelCell);
                }

                grid.Add(row);
            }

            return grid;
        }

        public static void Run()
        {
            var Day11 = new Day11();
            Console.WriteLine("\n###############\n###############\nDay11\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(Day11.Part1(18));
            Console.WriteLine(Day11.Part1(42));
            Console.WriteLine(Day11.Part1(GetInput()));
            
            Console.WriteLine("\n###############\nPart 2\n###############\n");
            //Console.WriteLine(Day11.Part2(18));
            //Console.WriteLine(Day11.Part2(42));
            Console.WriteLine(Day11.Part2(GetInput()));
        }

        private static int GetInput()
        {
            return 7857;
        }
    }

    class FuelCell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int PowerLevel { get; set; }

        public FuelCell(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
