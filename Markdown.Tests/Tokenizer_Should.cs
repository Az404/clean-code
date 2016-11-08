using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    internal class Tokenizer_Should
    {
        [TestCase("some string", ' ', ExpectedResult = "some", TestName = "single char")]
        [TestCase("str|with_differentSeparators", '_', 'S', '|',ExpectedResult = "str", TestName = "first of several chars")]
        [TestCase("abc", 'z', ExpectedResult = "abc", TestName = "when no stop char")]
        public string ReadStringUntilStopChars(string input, params char[] stopChars)
        {
            return new Tokenizer(input).ReadUntil(stopChars);
        }

        private static IEnumerable<TestCaseData> ReadUntilLambdaIsTrue_TestCases()
        {
            yield return new TestCaseData("0120123456", (Func<char, bool>) (c => int.Parse(c.ToString()) > 2))
                .Returns("012012")
                .SetName("when ordinary lambda");

            yield return new TestCaseData("abc", (Func<char, bool>)(c => false))
                .Returns("abc")
                .SetName("when lambda is always false");
        }

        [TestCaseSource(nameof(ReadUntilLambdaIsTrue_TestCases))]
        public string ReadString_UntilLambdaIsTrue(string input, Func<char, bool> isStopChar)
        {
            return new Tokenizer(input).ReadUntil(isStopChar);
        }

        [Test]
        public void ReadUntil_Repeately()
        {
            var tokenizer = new Tokenizer("abc def_ghi");
            tokenizer.ReadUntil(' ').Should().Be("abc");
            tokenizer.ReadUntil('_').Should().Be(" def");
        }

        [Test]
        public void ChangePosition_AfterReadUntilStopChars()
        {
            var tokenizer = new Tokenizer("abc def");
            tokenizer.ReadUntil(' ');
            tokenizer.Position.Should().Be(3);
        }

        [Test]
        public void ChangePosition_AfterReadUntilLambdaIsTrue()
        {
            var tokenizer = new Tokenizer("abc def");
            tokenizer.ReadUntil(c => c == 'd');
            tokenizer.Position.Should().Be(4);
        }

        [Test]
        public void ReadUntilUnescaped_WhenNoEscaping()
        {
            new Tokenizer("some string")
                .ReadUntilUnescaped(' ')
                .Should()
                .Be("some");
        }

        [Test]
        public void SkipEscapedStopChar_InReadUntilUnescaped()
        {
            new Tokenizer(@"some\ string with escaping")
                .ReadUntilUnescaped(' ')
                .Should()
                .Be(@"some\ string");
        }
    }
}
