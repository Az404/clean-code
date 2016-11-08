namespace Markdown
{
    public static class Md
    {
        public static string Render(string markdownText)
        {
            return HtmlRenderer.Render(new MdTokenizer(markdownText).ReadTokens());
        }
    }
}
