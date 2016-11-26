using System.Collections.Generic;
using Markdown.Tokens;

namespace Markdown
{
    public class TokensStorage
    {
        private readonly Stack<FormattedToken> openedTokens = new Stack<FormattedToken>();
        private readonly List<Token> closedTokens = new List<Token>();

        public IEnumerable<Token> ClosedTokens => closedTokens;
        public IEnumerable<FormattedToken> UnclosedTokens => openedTokens;

        public FormattedToken LastToken => openedTokens.Count > 0 ? openedTokens.Peek() : null;

        public void OpenToken(SelectionType type)
        {
            openedTokens.Push(new FormattedToken(type));
        }

        public void CloseLastToken()
        {
            var lastToken = openedTokens.Pop();
            if (openedTokens.Count > 0)
                openedTokens.Peek().Body.Add(lastToken);
            else
                closedTokens.Add(lastToken);
        }

        public void AddRawToken(RawToken token)
        {
            if (LastToken != null)
                LastToken.Body.Add(token);
            else
                closedTokens.Add(token);
        }
    }
}
