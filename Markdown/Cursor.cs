namespace Markdown
{
    public struct Cursor
    {
        public string Text { get; }
        public int Position { get; set; }

        public Cursor(string text, int position)
        {
            Text = text;
            Position = position;
        }

        public bool EndOfString => Position >= Text.Length;

        public char? CurrentChar => Text.GetCharAt(Position);
        public char? PreviousChar => Text.GetCharAt(Position - 1);

        public char? CharAt(int offset) => Text.GetCharAt(Position + offset);

        public bool StartsWithFromCurrent(string sample)
        {
            if (Position < 0 || EndOfString)
                return false;
            return Text.StartsWithFrom(Position, sample);
        }
    }
}
