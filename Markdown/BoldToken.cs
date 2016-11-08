namespace Markdown
{
    class BoldToken : FormattedToken
    {
        public BoldToken(params Token[] tokens) : base(tokens)
        {
        }

        public override string ToHtml()
        {
            return $"<strong>{base.ToHtml()}</strong>";
        }
    }
}
