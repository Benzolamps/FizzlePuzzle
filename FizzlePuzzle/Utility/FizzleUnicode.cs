using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace FizzlePuzzle.Utility
{
    internal static class FizzleUnicode
    {
        internal static string Encode(string str)
        {
            StringBuilder unicode = new StringBuilder();
            foreach (var chr in str)
            {
                if (chr > 127)
                {
                    unicode.Append("\\u" + ((int) chr).ToString("X"));
                }
                else
                {
                    unicode.Append(chr);
                }
            }

            return unicode.ToString();
        }

        internal static string Decode(string unicode)
        {
            StringBuilder str = new StringBuilder(unicode);
            Regex regex = new Regex("\\\\u\\d+");
            Match match = regex.Match(unicode);
            while (match.Success)
            {
                int ascii = int.Parse(match.Value.Replace("\\u", string.Empty), NumberStyles.HexNumber);
                str.Replace(match.Value, ((char) ascii).ToString());
                match = match.NextMatch();
            }
            return str.ToString();
        }
    }
}
