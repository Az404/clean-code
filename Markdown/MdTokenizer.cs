using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MdTokenizer
    {
        private Tokenizer tokenizer;

        private List<string> tagsFindOrder = new List<string>()
        {
            "__",
            "_"
        };

        private Dictionary<string, Func<FormattedToken>> tagTokens = new Dictionary<string, Func<FormattedToken>>()
        {
            ["__"] = () => new BoldToken(),
            ["_"] = () => new ItalicToken()
        };

        private Dictionary<string, HashSet<string>> allowedInnerTags = new Dictionary<string, HashSet<string>>()
        {
            ["__"] = new HashSet<string>() { "_" },
            ["_"] = new HashSet<string>()
        };

        private Stack<string> openedTags;
        private Stack<FormattedToken> openedTokens;
        private List<Token> closedTokens = new List<Token>();

        public MdTokenizer(string input)
        {
            tokenizer = new Tokenizer(input);
            openedTags = new Stack<string>();
            openedTokens = new Stack<FormattedToken>();
        }

        public IEnumerable<Token> ReadTokens()
        {
            return ReadClosedTokens().Concat(ReadUnclosedTokens());
        }

        private IEnumerable<Token> ReadClosedTokens()
        {
            while (!tokenizer.EndOfString)
            {
                var isTag = false;
                foreach (var tag in tagsFindOrder)
                {
                    if (StandsOnClosingTag(tag) && IsOnStackTop(tag) && !IsLastTokenEmpty())
                    {
                        openedTags.Pop();
                        CloseToken(openedTokens.Pop());

                    } else if (StandsOnOpeningTag(tag) && IsOpeningTagAllowed(tag))
                        OpenToken(tag);
                    else 
                        continue;

                    isTag = true;
                    SkipTag(tag);
                    break;
                }
                if (isTag)
                    continue;
                CloseToken(new RawToken(ReadRawText().Unescape()));
            }
            return closedTokens;
        }

        private void OpenToken(string tag)
        {
            openedTags.Push(tag);
            openedTokens.Push(tagTokens[tag]());
        }

        private void CloseToken(Token token)
        {
            if (openedTokens.Count > 0)
                openedTokens.Peek().Body.Add(token);
            else
                closedTokens.Add(token);
        }

        private bool IsOnStackTop(string tag)
        {
            return openedTags.Count > 0 && tag == openedTags.Peek();
        }

        private bool IsOpeningTagAllowed(string tag)
        {
            return openedTags.Count == 0 || allowedInnerTags[openedTags.Peek()].Contains(tag);
        }

        private bool IsLastTokenEmpty()
        {
            return openedTokens.Count != 0 && openedTokens.Peek().Body.Count == 0;
        }

        private bool StandsOnOpeningTag(string tag)
        {
            var afterTagChar = GetAfterTagChar(tag);
            return StandsOnTag(tag) && afterTagChar != ' ' && afterTagChar != null;
        }

        private bool StandsOnClosingTag(string tag)
        {
            var beforeTagChar = GetBeforeTagChar();
            return StandsOnTag(tag) && beforeTagChar != ' ' && beforeTagChar != null;
        }

        private bool StandsOnTag(string tag)
        {
            var beforeTagChar = GetBeforeTagChar();
            var afterTagChar = GetAfterTagChar(tag);
            return tokenizer.StartsWithFromCurrent(tag) &&
                !(beforeTagChar.HasValue && afterTagChar.HasValue &&
                beforeTagChar != ' ' && afterTagChar != ' ' &&
                (char.IsDigit(beforeTagChar.Value) || char.IsDigit(afterTagChar.Value)));
        }

        private char? GetBeforeTagChar()
        {
            return tokenizer.Input.GetCharAt(tokenizer.Position - 1);
        }

        private char? GetAfterTagChar(string tag)
        {
            return tokenizer.Input.GetCharAt(tokenizer.Position + tag.Length);
        }

        private void SkipTag(string tag)
        {
            tokenizer.Position += tag.Length;
        }

        private string ReadRawText()
        {
            return tokenizer.ReadUntilUnescaped('_');
        }

        private IEnumerable<Token> ReadUnclosedTokens()
        {
            openedTags = new Stack<string>(openedTags);
            openedTokens = new Stack<FormattedToken>(openedTokens);
            while (openedTags.Count > 0)
            {
                var tag = openedTags.Pop();
                var token = openedTokens.Pop();
                yield return new RawToken(tag);
                foreach (var subToken in token.Body)
                {
                    yield return subToken;
                }
            }
        }
    }
}
