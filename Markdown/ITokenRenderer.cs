using Markdown.Tokens;

namespace Markdown
{
    public interface ITokenRenderer
    {
        string Render(RawToken token);
        string Render(FormattedToken token);
    }
}
