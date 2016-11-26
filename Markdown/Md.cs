using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown
{
    public class Md
    {
        public string BaseUrl { get; set; }
        public string CssStyle { get; set; }

        public string Render(string markdownText)
        {
            var htmlRenderer = new HtmlRenderer(FormatCustomAttributes());
            var tokens = new MdTokenizer(markdownText).ReadTokens().ToArray();
            if (BaseUrl != null)
                AppendToRelativeUrls(BaseUrl, tokens);
            return htmlRenderer.Render(tokens);
        }

        private SelectionAttribute[] FormatCustomAttributes()
        {
            if (CssStyle != null)
                return new[] {new SelectionAttribute(SelectionAttributeType.Style, CssStyle)};
            return new SelectionAttribute[] {};
        }

        private static void AppendToRelativeUrls(string baseUrl, IEnumerable<Token> tokens)
        {
            foreach (var formattedToken in tokens.Where(token => token is FormattedToken).Cast<FormattedToken>())
            {
                var urlAttribute = formattedToken.Attributes.Find(attr => attr.Type == SelectionAttributeType.Url);
                if (urlAttribute != null)
                {
                    if (!IsAbsoluteUrl(urlAttribute.Value))
                        urlAttribute.Value = baseUrl + urlAttribute.Value;
                }

                AppendToRelativeUrls(baseUrl, formattedToken.Body);
            }
        }

        private static bool IsAbsoluteUrl(string url)
        {
            return url.Contains("://");
        }
    }
}
