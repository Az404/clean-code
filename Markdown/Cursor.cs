namespace Markdown
{
    public struct Cursor
    {
        // CR: Public non-readonly fields - bad practice
        public string Text;
        public int Position;

        public bool EndOfString => Position >= Text.Length;

        public char? CurrentChar => Text.GetCharAt(Position);
        public char? PreviousChar => Text.GetCharAt(Position - 1);

        public char? CharAt(int offset) => Text.GetCharAt(Position + offset);

        public bool StartsWithFromCurrent(string sample)
        {
            return Text.StartsWithFrom(Position, sample);
        }
    }
}
