using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day4
{
    internal class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = await ReadInput();
            var numbers = input[0].Split(',').Select(x => int.Parse(x));
            var boards = new List<BingoBoard>();
            for (var i = 2; i < input.Length; i += 6)
            {
                var board = new List<string>();
                for (var j = 0; j < 5; j++)
                    board.Add(input[i + j]);

                boards.Add(new BingoBoard(board));
            }

            foreach (var num in numbers)
            {
                LogMessage($"Calling number {num}");
                foreach (var board in boards)
                {
                    board.Mark(num);
                    //LogMessage(board.ToString());
                    //board.PrintBoard();
                    LogMessage("");
                }

                if (boards.Any(x => x.HasBingo))
                {
                    LogMessage("BINGO!");
                    var winner = boards.First(x => x.HasBingo);
                    return winner.Score.ToString();
                }
            }

            return String.Empty;
        }

        public new async Task<string> Part2()
        {
            var input = await ReadInput();
            var numbers = input[0].Split(',').Select(x => int.Parse(x));
            var boards = new List<BingoBoard>();
            for (var i = 2; i < input.Length; i += 6)
            {
                var board = new List<string>();
                for (var j = 0; j < 5; j++)
                    board.Add(input[i + j]);

                boards.Add(new BingoBoard(board));
            }

            foreach (var num in numbers)
            {
                LogMessage($"Calling number {num}");
                foreach (var board in boards)
                {
                    board.Mark(num);
                }

                if (boards.Count == 1 && boards[0].HasBingo)
                {
                    return boards[0].Score.ToString();
                }

                boards.RemoveAll(x => x.HasBingo);
            }

            return String.Empty;
        }

        private string[] GetSample()
        {
            var sample = @"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7";
            return sample.Split(Environment.NewLine);
        }
    }
}
