using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventOfCode2020.Day5
{
    class Main : BaseClass
    {
        public new async Task<string> Part1()
        {
            var input = await ReadInput();
            var highestSeat = 0;
            foreach (var line in input)
            {
                var seatId = GetSeatId(line);
                if (seatId > highestSeat)
                    highestSeat = seatId;
            }

            return highestSeat.ToString();
        }

        public new async Task<string> Part2()
        {
            var input = await ReadInput();
            var seatIds = new SortedSet<int>();
            foreach (var line in input)
            {
                var seatId = GetSeatId(line);
                seatIds.Add(seatId);
            }

            var lastSeatId = 0;
            foreach (var seatId in seatIds)
            {
                if (lastSeatId > 0 && seatId == lastSeatId + 2)
                    return (seatId - 1).ToString();

                lastSeatId = seatId;
            }

            return "Can't find your seat!";
        }

        private int GetSeatId(string seat)
        {
            var (lRow, uRow, lCol, uCol) = (0, 127, 0, 7);
            for (var i = 0; i < 7; i++)
            {
                var dist = (uRow - lRow + 1) / 2;
                if (seat[i] == 'F')
                    uRow -= dist;
                else if (seat[i] == 'B')
                    lRow += dist;
            }

            for (var i = 7; i < 10; i++)
            {
                var dist = (uCol - lCol + 1) / 2;
                if (seat[i] == 'L')
                    uCol -= dist;
                else if (seat[i] == 'R')
                    lCol += dist;
            }

            return lRow * 8 + lCol;
        }

        private string[] GetSample()
        {
            return @"BBFFBBFRLL".Split(Environment.NewLine);
        }
    }
}
