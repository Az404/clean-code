using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    class String_Should
    {
        [TestCase("abcd", ExpectedResult = "abcd", TestName = "when no escaping")]
        [TestCase(@"ab\cd\ ", ExpectedResult = "abcd ", TestName = "when escaping ordinary symbols")]
        [TestCase(@"ab\\cd", ExpectedResult = @"ab\cd", TestName = "when escaping backslash")]
        public string Unescape_Itself_Correctly(string source)
        {
            return source.Unescape();
        }
    }
}
