namespace Markdown
{
    class ItalicToken : FormattedToken
    {
        public ItalicToken(params Token[] tokens) : base(tokens)
        {
        }

        public override string ToHtml()
        {
            return $"<em>{base.ToHtml()}</em>";
        }
    }
}
