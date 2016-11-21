﻿namespace Markdown
{
    class RawToken : Token
    {
        public string Value { get; }

        public RawToken(string value)
        {
            Value = value;
        }
    }
}
