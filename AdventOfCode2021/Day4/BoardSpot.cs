using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2021.Day4
{
    internal class BoardSpot
    {
        public int Value { get; }

        public bool Marked { get; private set; }

        public BoardSpot(int value)
        {
            Value = value;
            Marked = false;
        }

        public void Mark()
        {
            Marked = true;
        }

        public override string ToString()
        {
            return Value.ToString().PadLeft(2);
        }
    }
}
