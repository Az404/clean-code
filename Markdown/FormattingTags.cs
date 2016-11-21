namespace Markdown
{
    class OpeningFormattingTag : FormattingTag
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

    class ClosingFormattingTag : FormattingTag
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

    class FormattingTag : Tag
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
