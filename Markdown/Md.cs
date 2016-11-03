using System;

namespace Markdown
{
    public class Md
    {
        public string Render(string markdownText)
        {
            return HtmlRenderer.Render(new MdTokenizer(markdownText).ReadTokens());
        }
    }
}
