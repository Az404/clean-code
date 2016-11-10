using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class CharExtensions
    {
        public static bool IsDigit(this char ch)
        {
            return '0' <= ch && ch <= '9';
        }
    }
}
