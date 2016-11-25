using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Tag
    {
        public string Name { get; }

        public virtual int GetLength(Cursor cursor) => Name.Length;

        public Tag(string name)
        {
            Name = name;
        }

        public virtual bool PresentsAt(Cursor cursor)
        {
            return cursor.StartsWithFromCurrent(Name);
        }

        public virtual IEnumerable<SelectionAttribute> ExtractAttributes(Cursor cursor)
        {
            // CR: Be consistent, you either use brackets for one line if or you do not
            if (!PresentsAt(cursor))
            {
                throw new ArgumentException("Tag not found");
            }
            // CR: Why initialize array when you have Enumerable.Empty?
            return new SelectionAttribute[] { };
        }
    }
}
