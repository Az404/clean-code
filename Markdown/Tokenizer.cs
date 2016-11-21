using System;
using System.Linq;

namespace Markdown
{
    public class Tokenizer
    {
        public string Input { get; }
        public int Position { get; set; }

        public Cursor Cursor => new Cursor { Text = Input, Position = Position };

        public Tokenizer(string input)
        {
            Input = input;
        }

        public Tokenizer(Cursor cursor)
        {
            Input = cursor.Text;
            Position = cursor.Position;
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
                } while (Cursor.CurrentChar != null && !stopChars.Contains(Cursor.CurrentChar.Value));
            });
        }

        public string ReadUntil(Func<char, bool> isStopChar)
        {
            return ExtractReadString(() =>
            {
                do
                {
                    Position++;
                } while (Cursor.CurrentChar != null && !isStopChar(Cursor.CurrentChar.Value));
            });
        }

        public string ReadUntilUnescaped(params char[] stopChars)
        {
            var escaping = Cursor.CurrentChar == '\\';
            return ReadUntil(c =>
            {
                var result = !escaping && Array.IndexOf(stopChars, c) >= 0;
                escaping = !escaping && c == '\\';
                return result;
            });
        }
    }
}