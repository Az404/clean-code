using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Should
    {
        [Test]
        public void NotChangeString_WhenNoFormatting()
        {
            const string text = "Simple text";
            Md.Render(text).Should().Be(text);
        }

        [TestCase("_italic words_", ExpectedResult = "<em>italic words</em>", TestName = "only italic")]
        [TestCase("Regular _and italic_ text _together_", ExpectedResult = "Regular <em>and italic</em> text <em>together</em>", TestName = "italic and regular")]
        [TestCase(@"_some\_text_", ExpectedResult = "<em>some_text</em>", TestName = "italic with escaped underscore")]

        [TestCase("__bold words__", ExpectedResult = "<strong>bold words</strong>", TestName = "only bold")]
        [TestCase("Regular __and bold__ text __together__",
            ExpectedResult = "Regular <strong>and bold</strong> text <strong>together</strong>", TestName = "bold and regular")]
        [TestCase(@"__some\_\_text__", ExpectedResult = "<strong>some__text</strong>", TestName = "bold with escaped underscores")]

        [TestCase("_Italic,_ __bold__ and regular",
            ExpectedResult = "<em>Italic,</em> <strong>bold</strong> and regular", TestName = "italic, bold and regular")]
        [TestCase("__abc _italic_ def__",
            ExpectedResult = "<strong>abc <em>italic</em> def</strong>", TestName = "italic works inside bold")]

        [TestCase("abc_ def_", ExpectedResult = "abc_ def_", TestName = "regular if space after opening _")]
        [TestCase("abc _def _", ExpectedResult = "abc _def _", TestName = "regular if space before closing _")]
        public string RenderTextWithFormatting_Correctly(string text)
        {
            return Md.Render(text);
        }
    }
}
