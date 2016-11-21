using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    class ClosingUrlTag : Tag
    {
        private const string Begin = "](";
        private const string End = ")";

        public ClosingUrlTag() : base(Begin) { }

        public override int GetLength(Cursor cursor) => Begin.Length + ExtractUrl(cursor).Length + End.Length;

        private string ExtractUrl(Cursor cursor)
        {
            if (!cursor.StartsWithFromCurrent(Begin))
                return null;
            cursor.Position += Begin.Length;
            var tokenizer = new Tokenizer(cursor);
            var url = tokenizer.ReadUntilUnescaped(End[0]);
            if (tokenizer.Cursor.CurrentChar != End[0])
                return null;
            return url;
        }

        public override bool PresentsAt(Cursor cursor)
        {
            return ExtractUrl(cursor) != null;
        }

        public override IEnumerable<SelectionAttribute> ExtractAttributes(Cursor cursor)
        {
            return base.ExtractAttributes(cursor)
                .Concat(new [] { new SelectionAttribute(SelectionAttributeType.Url, ExtractUrl(cursor).Unescape()) });
        }
    }
}
