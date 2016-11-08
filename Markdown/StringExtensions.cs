using System.Linq;

namespace Markdown
{
    public static class StringExtensions
    {
        public static string Unescape(this string input)
        {
            var escaping = false;
            return string.Join("", input.Select(c =>
            {
                if (!escaping && c == '\\')
                {
                    escaping = true;
                    return "";
                }
                escaping = false;
                return c.ToString();
            }));
        }

        public static bool StartsWithFrom(this string source, int startIndex, string sample)
        {
            return source.Substring(startIndex).StartsWith(sample);
        }

        public static char? GetCharAt(this string source, int position)
        {
            if (0 <= position && position < source.Length)
                return source[position];
            return null;
        }
    }
}
