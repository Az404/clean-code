namespace Markdown
{
    public class Token
    {
        public string Value { get; private set; }
        public TokenAttribute[] Attributes { get; private set; }

        public Token(string value, params TokenAttribute[] attributes)
        {
            Value = value;
            Attributes = attributes;
        }
    }

    public enum TokenAttribute
    {
        Raw, Bold, Italic
    }
}
