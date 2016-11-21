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
            if (!PresentsAt(cursor))
            {
                throw new ArgumentException("Tag not found");
            }
            return new SelectionAttribute[] { };
        }
    }
}
