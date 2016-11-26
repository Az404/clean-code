using System.Collections.Generic;
using System.Linq;
using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown
{
    public class MdTokenizer
    {
        private readonly Tokenizer tokenizer;

        private char[] stopChars;
        private List<Selection> selections;

        private readonly Stack<Selection> openedSelections = new Stack<Selection>();
        private readonly TokensStorage tokensStorage = new TokensStorage();

        private Selection CurrentSelection => openedSelections.Count > 0 ? openedSelections.Peek() : null;

        private IEnumerable<Selection> AllowedSelections
            => CurrentSelection != null ? CurrentSelection.AllowedInnerSelections : selections;

        public MdTokenizer(string input)
        {
            tokenizer = new Tokenizer(input);

            BuildLanguage();
        }
        
        public static HashSet<char> GetStopChars(IEnumerable<Selection> selections)
        {
            var result = new HashSet<char>();
            foreach(var selection in selections)
            {
                result.Add(selection.OpeningTag.Name[0]);
                result.Add(selection.ClosingTag.Name[0]);
            }
            return result;
        }

        public IEnumerable<Token> ReadTokens()
        {
            return ReadClosedTokens().Concat(ReadUnclosedTokens());
        }

        private void BuildLanguage()
        {
            var openingBold = new OpeningFormattingTag("__");
            var closingBold = new ClosingFormattingTag("__");
            var openingItalic = new OpeningFormattingTag("_");
            var closingItalic = new ClosingFormattingTag("_");

            var boldSelection = new Selection(openingBold, closingBold, SelectionType.Bold);
            var italicSelection = new Selection(openingItalic, closingItalic, SelectionType.Italic);
            var urlSelection = new Selection(new Tag("["), new ClosingUrlTag(), SelectionType.Url);

            selections = new List<Selection>()
            {
                boldSelection,
                italicSelection,
                urlSelection
            };

            boldSelection.AllowedInnerSelections.Add(italicSelection);
            boldSelection.AllowedInnerSelections.Add(urlSelection);

            italicSelection.AllowedInnerSelections.Add(urlSelection);

            stopChars = GetStopChars(selections).ToArray();
        }

        private IEnumerable<Token> ReadClosedTokens()
        {
            while (!tokenizer.Cursor.EndOfString)
            {
                if (TryCloseCurrentSelection())
                    continue;
                
                if (AllowedSelections.Any(TryOpenSelection))
                    continue;

                tokensStorage.AddRawToken(new RawToken(ReadRawText().Unescape()));
            }
            return tokensStorage.ClosedTokens;
        }

        private bool TryOpenSelection(Selection selection)
        {
            if (selection.OpensAt(tokenizer.Cursor))
            {
                openedSelections.Push(selection);
                tokensStorage.OpenToken(selection.Type);
                SkipTag(selection.OpeningTag);
                return true;
            }
            return false;
        }

        private bool TryCloseCurrentSelection()
        {
            if (CurrentSelection != null && CurrentSelection.ClosesAt(tokenizer.Cursor) && !IsLastTokenEmpty())
            {
                tokensStorage.LastToken.Attributes.AddRange(CurrentSelection.ClosingTag.ExtractAttributes(tokenizer.Cursor));
                SkipTag(CurrentSelection.ClosingTag);
                openedSelections.Pop();
                tokensStorage.CloseLastToken();
                return true;
            }
            return false;
        }

        private bool IsLastTokenEmpty()
        {
            return tokensStorage.LastToken!= null && tokensStorage.LastToken.Body.Count == 0;
        }

        private void SkipTag(Tag tag)
        {
            tokenizer.Position += tag.GetLength(tokenizer.Cursor);
        }

        private string ReadRawText()
        {
            return tokenizer.ReadUntilUnescaped(stopChars);
        }

        private IEnumerable<Token> ReadUnclosedTokens()
        {
            var reversedOpenedSelections = new Stack<Selection>(this.openedSelections);
            var reversedUnclosedTokens = new Stack<FormattedToken>(tokensStorage.UnclosedTokens);
            while (reversedOpenedSelections.Count > 0)
            {
                var selection = reversedOpenedSelections.Pop();
                var token = reversedUnclosedTokens.Pop();
                yield return new RawToken(selection.OpeningTag.Name);
                foreach (var subToken in token.Body)
                {
                    yield return subToken;
                }
            }
        }
    }
}
