using System;
using System.Linq;

namespace Markdown
{
    public class Tokenizer
    {
        public string Input { get; }
        public int Position { get; set; }
        public char CurrentChar => Input[Position];
        public bool EndOfString => Position >= Input.Length;

        public Tokenizer(string input)
        {
            Input = input;
        }

        private string ExtractReadString(Action readAction)
        {
            var startPosition = Position;
            readAction();
            if (startPosition < Input.Length)
                return Input.Substring(startPosition, Position - startPosition);
            return "";
        }

        public string ReadUntil(params char[] stopChars)
        {
            return ExtractReadString(() =>
            {
                do
                {
                    Position++;
                } while (!EndOfString && !stopChars.Contains(CurrentChar));
            });
        }

        public string ReadUntil(Func<char, bool> isStopChar)
        {
            return ExtractReadString(() =>
            {
                do
                {
                    Position++;
                } while (!EndOfString && !isStopChar(CurrentChar));
            });
        }

        public string ReadUntilUnescaped(char stopChar)
        {
            var escaping = false;
            return ReadUntil(c =>
            {
                var result = !escaping && c == stopChar;
                escaping = !escaping && c == '\\';
                return result;
            });
        }

        public bool StartsWithFromCurrent(string sample)
        {
            return Input.StartsWithFrom(Position, sample);
        }
    }
}