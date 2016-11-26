namespace Markdown.Tags
{
    public class FormattingTag : Tag
    {
        public FormattingTag(string name) : base(name)
        {
        }
        
        public override bool PresentsAt(Cursor cursor)
        {
            var beforeTagChar = cursor.PreviousChar;
            var afterTagChar = cursor.CharAt(Name.Length);
            return base.PresentsAt(cursor) &&
                !(beforeTagChar.HasValue && afterTagChar.HasValue &&
                beforeTagChar != ' ' && afterTagChar != ' ' &&
                (char.IsDigit(beforeTagChar.Value) || char.IsDigit(afterTagChar.Value)));
        }
    }
}
