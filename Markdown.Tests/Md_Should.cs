using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown.Tests
{
    [TestFixture]
    public class Md_Should
    {
        private Md md;

        [SetUp]
        public void SetUp()
        {
            md = new Md();
        }


        [Test]
        public void NotChangeString_WhenNoFormatting()
        {
            const string text = "Simple text";
            md.Render(text).Should().Be(text);
        }

        [TestCase("_italic words_", ExpectedResult = "<em>italic words</em>", TestName = "only italic")]
        [TestCase("Regular _and italic_ text _together_", ExpectedResult = "Regular <em>and italic</em> text <em>together</em>", TestName = "italic and regular")]
        [TestCase(@"_some\_text_", ExpectedResult = "<em>some_text</em>", TestName = "italic with escaped underscore")]
        [TestCase(@"\_regular\_", ExpectedResult = "_regular_", TestName = "regular when escaped _ tags")]

        [TestCase("__bold words__", ExpectedResult = "<strong>bold words</strong>", TestName = "only bold")]
        [TestCase("Regular __and bold__ text __together__",
            ExpectedResult = "Regular <strong>and bold</strong> text <strong>together</strong>", TestName = "bold and regular")]
        [TestCase(@"__some\_\_text__", ExpectedResult = "<strong>some__text</strong>", TestName = "bold with escaped underscores")]
        [TestCase(@"\_\_regular\_\_", ExpectedResult = "__regular__", TestName = "regular when escaped __ tags")]

        [TestCase("_Italic,_ __bold__ and regular",
            ExpectedResult = "<em>Italic,</em> <strong>bold</strong> and regular", TestName = "italic, bold and regular")]
        [TestCase("__abc _italic_ def__",
            ExpectedResult = "<strong>abc <em>italic</em> def</strong>", TestName = "italic works inside bold")]
        [TestCase("_abc __regular__ def_",
            ExpectedResult = "<em>abc _</em>regular<em>_ def</em>", TestName = "bold isn't works inside italic")]
        [TestCase("___abc___",
            ExpectedResult = "<strong><em>abc</em></strong>", TestName = "triple underscore works")]

        [TestCase("abc_ def_", ExpectedResult = "abc_ def_", TestName = "regular if space after opening _")]
        [TestCase("abc _def _", ExpectedResult = "abc _def _", TestName = "regular if space before closing _")]
        [TestCase("abc__ def__", ExpectedResult = "abc<em>_ def</em>_", TestName = "italic if space after opening __")]
        [TestCase("abc __def __", ExpectedResult = "abc __def __", TestName = "regular if space before closing __")]

        [TestCase("__ab", ExpectedResult = "__ab", TestName = "regular if non-closed __")]
        [TestCase("_ab", ExpectedResult = "_ab", TestName = "regular if non-closed _")]
        [TestCase("ab__", ExpectedResult = "ab__", TestName = "regular if non-opened __")]
        [TestCase("ab_", ExpectedResult = "ab_", TestName = "regular if non-closed _")]
        [TestCase("__ab _c", ExpectedResult = "__ab _c", TestName = "regular if non-closed opening __ and _")]
        [TestCase("_ab __c", ExpectedResult = "<em>ab _</em>c", TestName = "italic if unpaired opening __ after unpaired opening _")]
        [TestCase("d__ ab_ c", ExpectedResult = "d<em>_ ab</em> c", TestName = "italic if unpaired closing __ before unpaired closing _")]
        [TestCase("ab_ c__", ExpectedResult = "ab_ c__", TestName = "regular if non-opened closing __ and _")]

        [TestCase("_abc5_d_", ExpectedResult = "<em>abc5_d</em>", TestName = "_ inside word with numbers isn't works")]
        [TestCase("_abc5_ d_", ExpectedResult = "<em>abc5</em> d_", TestName = "_ on word boundary with numbers works")]

        [TestCase("__", ExpectedResult = "__", TestName = "regular if empty text between _")]
        [TestCase("____", ExpectedResult = "____", TestName = "regular if empty text between __")]

        [TestCase("[text](link)", ExpectedResult = "<a href=link>text</a>", TestName = "only url")]
        [TestCase(@"[te\]xt](li\)nk)", ExpectedResult = "<a href=li)nk>te]xt</a>", TestName = "url with escaping")]
        [TestCase("__ab [text](link) cd__", ExpectedResult = "<strong>ab <a href=link>text</a> cd</strong>", TestName = "url inside bold")]
        [TestCase("_ab [text](link) cd_", ExpectedResult = "<em>ab <a href=link>text</a> cd</em>", TestName = "url inside italic")]
        [TestCase("[abc", ExpectedResult = "[abc", TestName = "unclosed url")]
        [TestCase("[abc](a", ExpectedResult = "[abc](a", TestName = "badly closed url")]
        [TestCase("[](a)", ExpectedResult = "[](a)", TestName = "regular if empty text in url")]
        [TestCase("[a]()", ExpectedResult = "[a]()", TestName = "url if empty link in url")]
        public string RenderTextWithFormatting_Correctly(string text)
        {
            return md.Render(text);
        }

        [TestCase("_aa_", ExpectedResult = "<em style=abc>aa</em>", TestName = "with italic")]
        [TestCase("__aa__", ExpectedResult = "<strong style=abc>aa</strong>", TestName = "with bold")]
        [TestCase("[text](link)", ExpectedResult = "<a style=abc href=link>text</a>", TestName = "with url")]
        public string RenderTextWithFormatting_AndCustomAttribute_Correctly(string text)
        {
            md.CssStyle = "abc";
            return md.Render(text);
        }
        
        [TestCase("[text](page.htm)", ExpectedResult = "<a href=http://example.com/page.htm>text</a>", TestName = "when relative url")]
        [TestCase("[text](http://google.com/search)", ExpectedResult = "<a href=http://google.com/search>text</a>", TestName = "when absolute url")]
        [TestCase("[text](ftp://mirror.yandex.ru/)", ExpectedResult = "<a href=ftp://mirror.yandex.ru/>text</a>", TestName = "when absolute url with different protocol")]
        [TestCase("[text](http://example.com/page.htm)", ExpectedResult = "<a href=http://example.com/page.htm>text</a>", TestName = "when absolute url starts with base url")]
        public string RenderTextWithUrl_AndGivenBaseUrl_Correctly(string text)
        {
            md.BaseUrl = "http://example.com/";
            return md.Render(text);
        }

        private void RenderSamples(string sample, int count)
        {
            md.Render(string.Concat(Enumerable.Repeat(sample, count)));
        }

        [TestCase(100)]
        [TestCase(200)]
        [TestCase(400)]
        [TestCase(1000)]
        [Timeout(1000)]
        public void RenderTextInLinearTime_WhenOrdinaryTags(int count)
        {
            RenderSamples("_abc_ def __ghi__", count);
        }

        [TestCase(100)]
        [TestCase(200)]
        [TestCase(400)]
        [TestCase(1000)]
        [Timeout(1000)]
        public void RenderTextInLinearTime_WhenOnlyUnclosedTags(int count)
        {
            RenderSamples("_a _a ", count);
        }

        [TestCase(100)]
        [TestCase(200)]
        [TestCase(400)]
        [TestCase(1000)]
        [Timeout(1000)]
        public void RenderTextInLinearTime_WhenNestedTags(int count)
        {
            RenderSamples("__a _b_ _c_ d__", count);
        }

        [TestCase(100)]
        [TestCase(200)]
        [TestCase(400)]
        [TestCase(1000)]
        [Timeout(1000)]
        public void RenderTextInLinearTime_WhenManySimilarTags(int count)
        {
            RenderSamples("_a_a", count);
        }
    }
}
