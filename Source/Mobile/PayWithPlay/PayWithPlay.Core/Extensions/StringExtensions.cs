using System.Text.RegularExpressions;

namespace PayWithPlay.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool HasInterpolationPlaceholder(this string s)
        {
            return Regex.IsMatch(s, "{\\d+}");
        }
    }
}
