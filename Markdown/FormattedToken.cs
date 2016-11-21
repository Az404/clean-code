using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    class FormattedToken : Token
    {
        public SelectionType Type { get; }
        public List<Token> Body { get; }
        public List<SelectionAttribute> Attributes { get; } = new List<SelectionAttribute>();

        public FormattedToken(SelectionType type, params Token[] body)
        {
            Type = type;
            Body = body.ToList();
        }
    }
}
