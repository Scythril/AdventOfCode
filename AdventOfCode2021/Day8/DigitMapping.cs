namespace AdventOfCode2021.Day8
{
    internal class DigitMapping
    {
        private char _top = '.';
        private char _middle = '.';
        private char _bottom = '.';
        private char _topLeft = '.';
        private char _bottomLeft = '.';
        private char _topRight = '.';
        private char _bottomRight = '.';

        public List<char> AllChars { get; set; }
        public List<char> PossibleTop { get; set; }
        public List<char> PossibleMiddle { get; set; }
        public List<char> PossibleBottom { get; set; }
        public List<char> PossibleTopLeft { get; set; }
        public List<char> PossibleBottomLeft { get; set; }
        public List<char> PossibleTopRight { get; set; }
        public List<char> PossibleBottomRight { get; set; }

        public char Top { get { return _top; } set { if (!AllChars.Contains(value)) { throw new ArgumentException(); } _top = value; AllChars.Remove(value); } }
        public char Middle { get { return _middle; } set { if (!AllChars.Contains(value)) { throw new ArgumentException(); } _middle = value; AllChars.Remove(value); } }
        public char Bottom { get { return _bottom; } set { if (!AllChars.Contains(value)) { throw new ArgumentException(); } _bottom = value; AllChars.Remove(value); } }
        public char TopLeft { get { return _topLeft; } set { if (!AllChars.Contains(value)) { throw new ArgumentException(); } _topLeft = value; AllChars.Remove(value); } }
        public char BottomLeft { get { return _bottomLeft; } set { if (!AllChars.Contains(value)) { throw new ArgumentException(); } _bottomLeft = value; AllChars.Remove(value); } }
        public char TopRight { get { return _topRight; } set { if (!AllChars.Contains(value)) { throw new ArgumentException(); } _topRight = value; AllChars.Remove(value); } }
        public char BottomRight { get { return _bottomRight; } set { if (!AllChars.Contains(value)) { throw new ArgumentException(); } _bottomRight = value; AllChars.Remove(value); } }

        public bool IsComplete => AllChars.Count < 1;

        public DigitMapping(List<char> allChars)
        {
            AllChars = allChars;
            PossibleTop = new List<char>();
            PossibleBottom = new List<char>();
            PossibleMiddle = new List<char>();
            PossibleTopLeft = new List<char>();
            PossibleBottomLeft = new List<char>();
            PossibleTopRight = new List<char>();
            PossibleBottomRight = new List<char>();
        }

        public new string ToString()
        {
            return @$" {Top}{Top}{Top}{Top} 
{TopLeft}    {TopRight}
{TopLeft}    {TopRight}
 {Middle}{Middle}{Middle}{Middle} 
{BottomLeft}    {BottomRight}
{BottomLeft}    {BottomRight}
 {Bottom}{Bottom}{Bottom}{Bottom} ";
        }
    }
}
