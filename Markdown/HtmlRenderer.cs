using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public static class HtmlRenderer
    {
        public static string Render(IEnumerable<Token> tokens)
        {
            var result = new StringBuilder();
            foreach (var token in tokens)
                result.Append(token.ToHtml());
            return result.ToString();
        }
    }
}
