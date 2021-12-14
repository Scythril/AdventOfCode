namespace AdventOfCode2021.Day8
{
    internal class Digit
    {
        private DigitMapping _mapping;

        public Digit(DigitMapping mapping)
        {
            if (!mapping.IsComplete)
                throw new ArgumentException("DigitMapping is not complete!", nameof(mapping));

            _mapping = mapping;
        }

        public int Value(string segments)
        {
            switch (segments.Length)
            {
                case 7:
                    return 8;
                case 6: // 0, 6, 9
                    if (!segments.Contains(_mapping.Middle))
                        return 0;
                    else if (!segments.Contains(_mapping.TopRight))
                        return 6;
                    else if (!segments.Contains(_mapping.BottomLeft))
                        return 9;
                    break;
                case 5: // 2, 3, 5
                    if (!segments.Contains(_mapping.TopLeft) && !segments.Contains(_mapping.BottomRight))
                        return 2;
                    else if (!segments.Contains(_mapping.TopLeft) && !segments.Contains(_mapping.BottomLeft))
                        return 3;
                    else if (!segments.Contains(_mapping.BottomLeft) && !segments.Contains(_mapping.TopRight))
                        return 5;
                    break;
                case 4:
                    return 4;
                case 3:
                    return 7;
                case 2:
                    return 1;
            }

            throw new ArgumentOutOfRangeException(nameof(segments));
        }
    }
}
