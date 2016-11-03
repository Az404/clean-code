using System;

namespace Markdown
{
    public class Tokenizer
    {
        protected readonly string Input;
        protected int Position;
        public char CurrentChar => Input[Position];
        public bool EndOfString => Position >= Input.Length;

        protected Tokenizer(string input)
        {
            Input = input;
        }

        protected string ReadUntil(params char[] stopChars)
        {
            throw new NotImplementedException();
        }

        protected string ReadUntil(Func<char, bool> isStopChar)
        {
            throw new NotImplementedException();
        }

        protected string ReadEscapedUntil(char stopChar)
        {
            var result = "";
            while (!EndOfString && CurrentChar != stopChar)
                result += ReadEscapedChar();
            return result;
        }

        private string ReadEscapedChar()
        {
            if (CurrentChar == '\\')
                Position++;
            if (EndOfString)
                return "";
            return ReadChar().ToString();
        }

        private char ReadChar()
        {
            Position++;
            return Input[Position-1];
        }
    }
}
