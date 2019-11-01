using System;

namespace FizzlePuzzle.Extension
{
    internal static class StringExtension
    {
        internal static string[] Split(this string str, string separator)
        {
            return str.Split(new[] {separator}, StringSplitOptions.None);
        }
    }
}
