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

        private Dictionary<string, Func<Token[], Token>> tagTokens = new Dictionary<string, Func<Token[], Token>>()
        {
            ["__"] = (tokens) => new BoldToken(tokens),
            ["_"] = (tokens) => new ItalicToken(tokens)
        };

        public MdTokenizer(string input)
        {
            tokenizer = new Tokenizer(input);
        }

        public IEnumerable<Token> ReadTokens()
        {
            while (!tokenizer.EndOfString)
            {
                var isTag = false;
                foreach (var tag in tagsFindOrder)
                {
                    if (StandsOnTagExpression(tag))
                    {
                        SkipTag(tag);
                        var body = ReadTagBody(tag);
                        SkipTag(tag);
                        var innerTokens = new MdTokenizer(body).ReadTokens();
                        yield return tagTokens[tag](innerTokens.ToArray());
                        isTag = true;
                        break;
                    }
                }
                if (isTag)
                    continue;
                yield return new RawToken(ReadRawText().Unescape());
            }
        }

        private bool StandsOnTagExpression(string tag)
        {
            return StandsOnTag(tag) && HasClosedTag(tag) &&
                   tokenizer.Input.GetCharAt(tokenizer.Position + tag.Length) != ' ' &&
                   tokenizer.Input.GetCharAt(FindNextTagPosition(tag)-1) != ' ';
        }

        private bool StandsOnTag(string tag)
        {
            return tokenizer.StartsWithFromCurrent(tag);
        }

        private bool HasClosedTag(string tag)
        {
            return FindNextTagPosition(tag) >= 0;
        }

        private int FindNextTagPosition(string tag)
        {
            return tokenizer.Input.IndexOf(tag, tokenizer.Position + tag.Length, StringComparison.Ordinal);
        }

        private string ReadTagBody(string tag)
        {
            var body = "";
            do
            {
                body += tokenizer.ReadUntilUnescaped(tag[0]);
            } while (!StandsOnTag(tag));
            return body;
        }

        private void SkipTag(string tag)
        {
            tokenizer.Position += tag.Length;
        }

        private string ReadRawText()
        {
            return tokenizer.ReadUntilUnescaped('_');
        }
    }
}
