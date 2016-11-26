namespace Markdown.Tokens
{
    public abstract class Token
    {
        public abstract string Render(ITokenRenderer renderer);
    }
}
