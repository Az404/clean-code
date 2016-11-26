using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class StringExtensions_Should
    {
        [TestCase("abcd", ExpectedResult = "abcd", TestName = "when no escaping")]
        [TestCase(@"ab\cd\ ", ExpectedResult = "abcd ", TestName = "when escaping ordinary symbols")]
        [TestCase(@"ab\\cd", ExpectedResult = @"ab\cd", TestName = "when escaping backslash")]
        public string Unescape_String_Correctly(string source)
        {
            return source.Unescape();
        }
    }
}
