namespace Markdown.Tags
{
    public class OpeningFormattingTag : FormattingTag
    {
        public OpeningFormattingTag(string name) : base(name)
        {
        }

        public override bool PresentsAt(Cursor cursor)
        {
            var afterTagChar = cursor.CharAt(Name.Length);
            return base.PresentsAt(cursor) && afterTagChar != ' ';
        }
    }
}
