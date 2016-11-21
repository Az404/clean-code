namespace Markdown
{
    public static class Md
    {
        public static string Render(string markdownText, params SelectionAttribute[] customAttributes)
        {
            var htmlRenderer = new HtmlRenderer(customAttributes);
            return htmlRenderer.Render(new MdTokenizer(markdownText).ReadTokens());
        }
    }
}
