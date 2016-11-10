using System.Collections.Generic;
using System.Linq;
using static System.String;

namespace Markdown
{
    class FormattedToken : Token
    {
        public List<Token> Body { get; private set; }

        public FormattedToken(params Token[] body)
        {
            Body = body.ToList();
        }

        public override string ToHtml()
        {
            return Concat(Body.Select(token => token.ToHtml()));
        }
    }
}
