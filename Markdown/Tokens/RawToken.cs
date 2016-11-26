namespace Markdown.Tokens
{
    public class RawToken : Token
    {
        public string Value { get; }

        public RawToken(string value)
        {
            Value = value;
        }

        public override string Render(ITokenRenderer renderer)
        {
            return renderer.Render(this);
        }
    }
}
