using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Tokens;
using static System.String;

namespace Markdown
{
    public class HtmlRenderer : ITokenRenderer
    {
        private readonly SelectionAttribute[] customAttributes;

        private static readonly Dictionary<SelectionType, string> HtmlTags = new Dictionary<SelectionType, string>()
        {
            [SelectionType.Bold] = "strong",
            [SelectionType.Italic] = "em",
            [SelectionType.Url] = "a"
        };

        private static readonly Dictionary<SelectionAttributeType, string> HtmlAttributes = new Dictionary<SelectionAttributeType, string>()
        {
            [SelectionAttributeType.Url] = "href",
            [SelectionAttributeType.Style] = "style"
        };

        public HtmlRenderer(params SelectionAttribute[] customAttributes)
        {
            this.customAttributes = customAttributes;
        }

        public string Render(IEnumerable<Token> tokens)
        {
            var result = new StringBuilder();
            foreach (var token in tokens)
                result.Append(token.Render(this));
            return result.ToString();
        }

        public string Render(RawToken token)
        {
            return token.Value;
        }

        public string Render(FormattedToken token)
        {
            var renderedBody = Render(token.Body);
            var attributes = customAttributes.Concat(token.Attributes);
            return WrapWithTag(renderedBody, token.Type, attributes);
        }

        public static string WrapWithTag(string body, SelectionType selectionType, IEnumerable<SelectionAttribute> attributes)
        {
            var formattedAttributes = FormatAttributes(attributes);
            var tag = HtmlTags[selectionType];
            return $"<{tag}{formattedAttributes}>{body}</{tag}>";
        }

        public static string FormatAttributes(IEnumerable<SelectionAttribute> attributes)
        {
            return Concat(attributes.Select(attr => $" {HtmlAttributes[attr.Type]}={attr.Value}"));
        }
    }
}
