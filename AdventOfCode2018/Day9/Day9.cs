using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.Day9
{
    class Day9
    {
        public long Part1(string input)
        {
            var (numPlayers, lastMarble) = ParseLine(input);
            var marbles = new MarbleList<long>();
            var players = Enumerable.Range(0, numPlayers).Select(x => new Player(x)).ToList();
            var currentPlayer = 0;
            var currentMarble = marbles.AddFirst(0);
            for (var marbleValue = 1; marbleValue <= lastMarble; marbleValue++)
            {
                if (marbleValue % 23 == 0)
                {
                    players[currentPlayer].Score += marbleValue;
                    for(var i = 0; i < 7; i++)
                    {
                        currentMarble = currentMarble.Previous ?? currentMarble.List.Last;
                    }

                    var newCurrentMarble = currentMarble.Next ?? currentMarble.List.First;
                    players[currentPlayer].Score += currentMarble.Value;
                    marbles.Remove(currentMarble);
                    currentMarble = newCurrentMarble;
                }
                else
                {
                    currentMarble = currentMarble.Next ?? currentMarble.List.First;
                    currentMarble = marbles.AddAfter(currentMarble, marbleValue);
                }

                currentPlayer++;
                if (currentPlayer >= players.Count)
                {
                    currentPlayer = 0;
                }
            }

            return players.Max(x => x.Score);
        }

        public long Part2(string input)
        {
            return Part1(input);
        }

        private (int, int) ParseLine(string line)
        {
            var match = Regex.Match(line, @"(\d*) players; last marble is worth (\d*) points");
            return (int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
        }

        public static void Run()
        {
            var input = GetInput();
            var Day9 = new Day9();
            Console.WriteLine("\n###############\n###############\nDay9\n###############\n###############\n");
            Console.WriteLine("\n###############\nPart 1\n###############\n");
            Console.WriteLine(Day9.Part1("9 players; last marble is worth 25 points: high score is 32"));
            Console.WriteLine(Day9.Part1("10 players; last marble is worth 1618 points: high score is 8317"));
            Console.WriteLine(Day9.Part1("13 players; last marble is worth 7999 points: high score is 146373"));
            Console.WriteLine(Day9.Part1("17 players; last marble is worth 1104 points: high score is 2764"));
            Console.WriteLine(Day9.Part1("21 players; last marble is worth 6111 points: high score is 54718"));
            Console.WriteLine(Day9.Part1("30 players; last marble is worth 5807 points: high score is 37305"));
            Console.WriteLine(Day9.Part1(input));
            
            Console.WriteLine("\n###############\nPart 2\n###############\n");
            Console.WriteLine(Day9.Part2(@"405 players; last marble is worth 7170000 points"));
        }

        private static string GetInput()
        {
            return @"405 players; last marble is worth 71700 points";
        }
    }

    class Player
    {
        public int Id { get; set; }
        public long Score { get; set; }

        public Player(int id)
        {
            Id = id;
        }
    }

    public class MarbleList<T>
    {
        public Marble<T> First { get; private set; }
        public Marble<T> Last { get; private set; }

        public Marble<T> AddFirst(T value)
        {
            var marble = new Marble<T>(this, value);

            if (First == null)
            {
                First = marble;
                Last = marble;
            }
            else
            {
                marble.Next = First;
                First.Previous = marble;
                First = marble;
            }

            return marble;
        }

        public Marble<T> AddAfter(Marble<T> marble, T value)
        {
            var newMarble = new Marble<T>(this, value);
            newMarble.Previous = marble;
            var prevNext = marble.Next;
            marble.Next = newMarble;
            if (prevNext != null)
            {
                prevNext.Previous = newMarble;
                newMarble.Next = prevNext;
            }
            else
            {
                Last = newMarble;
            }

            return newMarble;
        }

        public Marble<T> Remove(Marble<T> marble)
        {
            if (marble.Next != null)
            {
                marble.Next.Previous = marble.Previous;
            }
            else
            {
                marble.List.Last = marble.Previous;
            }

            if (marble.Previous != null)
            {
                marble.Previous.Next = marble.Next;
            }
            else
            {
                marble.List.First = marble.Next;
            }

            return marble;
        }

        public override string ToString()
        {
            var marbles = new List<T>();
            var marble = First;
            while (marble != null)
            {
                marbles.Add(marble.Value);
                marble = marble.Next;
            }

            return string.Join(',', marbles);
        }
    }

    public class Marble<T>
    {
        public Marble<T> Next { get; set; }
        public Marble<T> Previous { get; set; }
        public MarbleList<T> List { get; }
        public T Value { get; }

        internal Marble(MarbleList<T> list, T value)
        {
            List = list;
            Value = value;
        }
    }
}
