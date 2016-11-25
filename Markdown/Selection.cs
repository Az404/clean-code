using System.Collections.Generic;

namespace Markdown
{
    // CR: Visibility
    class Selection
    {
        public Tag OpeningTag { get; }
        public Tag ClosingTag { get; }
        public SelectionType Type { get; }

        public List<Selection> AllowedInnerSelections { get; }

        public Selection(Tag openingTag, Tag closingTag, SelectionType type)
        {
            OpeningTag = openingTag;
            ClosingTag = closingTag;
            Type = type;
            AllowedInnerSelections = new List<Selection>();
        }

        public virtual bool OpensAt(Cursor cursor)
        {
            return OpeningTag.PresentsAt(cursor);
        }

        public virtual bool ClosesAt(Cursor cursor)
        {
            return ClosingTag.PresentsAt(cursor);
        }
    }
}
