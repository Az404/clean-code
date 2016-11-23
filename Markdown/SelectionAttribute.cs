﻿namespace Markdown
{
    public enum SelectionAttributeType
    {
        Url,
        Style
    }

    public class SelectionAttribute
    {
        public SelectionAttributeType Type { get; set; }
        public string Value { get; set; }

        public SelectionAttribute(SelectionAttributeType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
