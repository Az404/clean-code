using System.Linq;
using static System.String;

namespace Markdown
{
    class FormattedToken : Token
    {
        public Token[] Body { get; private set; }

        public FormattedToken(params Token[] body)
        {
            Body = body;
        }

        public override string ToHtml()
        {
            return Concat(Body.Select(token => token.ToHtml()));
        }
    }
}
