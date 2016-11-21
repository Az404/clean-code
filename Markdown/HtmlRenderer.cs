using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.String;

namespace Markdown
{
    public class HtmlRenderer
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
            {
                if (token is RawToken)
                    result.Append(((RawToken)token).Value);
                else if (token is FormattedToken)
                {
                    var formattedToken = (FormattedToken) token;
                    var renderedBody = Render(formattedToken.Body);
                    var attributes = customAttributes.Concat(formattedToken.Attributes);
                    result.Append(WrapWithTag(renderedBody, formattedToken.Type, attributes));
                }
            }
            return result.ToString();
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
