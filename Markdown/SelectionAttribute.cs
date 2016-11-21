namespace Markdown
{
    public enum SelectionAttributeType
    {
        Url,
        Style
    }

    public class SelectionAttribute
    {
        public SelectionAttributeType Type { get; }
        public string Value { get; }

        public SelectionAttribute(SelectionAttributeType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
