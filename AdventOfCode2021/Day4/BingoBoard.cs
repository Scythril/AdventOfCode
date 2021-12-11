using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day4
{
    internal class BingoBoard
    {
        private List<List<BoardSpot>> BoardSpots { get; set; }
        private int LastNumber { get; set; }

        public BingoBoard(List<string> input)
        {
            BoardSpots = new List<List<BoardSpot>>();

            foreach (var item in input)
            {
                var spots = item.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                BoardSpots.Add(spots.Select(x => new BoardSpot(int.Parse(x))).ToList());
            }
        }

        public void Mark(int value)
        {
            LastNumber = value;

            foreach (var row in BoardSpots)
            {
                foreach (var spot in row)
                {
                    if (spot.Value == value)
                        spot.Mark();
                }
            }
        }

        public bool HasBingo
        {
            get
            {
                var columns = new Dictionary<int, bool> { { 0, true }, { 1, true }, { 2, true }, { 3, true }, { 4, true } };
                foreach (var row in BoardSpots)
                {
                    if (row.All(x => x.Marked))
                        return true;

                    for (int i = 0; i < row.Count; i++)
                    {
                        if (!columns[i])
                            continue;

                        if (!row[i].Marked)
                            columns[i] = false;
                    }
                }

                return columns.Any(x => x.Value);
            }
        }

        public int Score
        {
            get
            {
                var sumUnmarked = BoardSpots.SelectMany(x => x).Where(x => !x.Marked).Sum(x => x.Value);
                return sumUnmarked * LastNumber;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var row in BoardSpots)
            {
                foreach (var spot in row)
                {
                    sb.Append(spot.ToString());
                    sb.Append(' ');
                }

                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }

        public void PrintBoard()
        {
            foreach (var row in BoardSpots)
            {
                foreach (var spot in row)
                {
                    Console.ForegroundColor = spot.Marked ? ConsoleColor.Red : ConsoleColor.Green;
                    Console.Write(spot.ToString() + ' ');
                }

                Console.ResetColor();
                Console.WriteLine();
            }
        }
    }
}
