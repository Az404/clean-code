namespace Markdown.Tags
{
    public class ClosingFormattingTag : FormattingTag
    {
        public ClosingFormattingTag(string name) : base(name)
        {
        }

        public override bool PresentsAt(Cursor cursor)
        {
            var beforeTagChar = cursor.PreviousChar;
            return base.PresentsAt(cursor) && beforeTagChar != ' ';
        }
    }
}
