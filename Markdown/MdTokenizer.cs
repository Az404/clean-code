using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MdTokenizer : Tokenizer
    {
        public MdTokenizer(string input) : base(input)
        {
        }

        public IEnumerable<Token> ReadTokens()
        {
            throw new NotImplementedException();
        }

        private string ReadWord()
        {
            throw new NotImplementedException();
        }

        private string ReadSpaces()
        {
            throw new NotImplementedException();
        }

        private string ReadUnderscores()
        {
            throw new NotImplementedException();
        }
    }
}
