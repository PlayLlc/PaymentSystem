using System.Globalization;
using System.Text.RegularExpressions;

namespace PayWithPlay.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool HasInterpolationPlaceholder(this string s)
        {
            return Regex.IsMatch(s, "{\\d+}");
        }

        public static bool ComplexContains(this string source, string value)
        {
            var index = CultureInfo.InvariantCulture.CompareInfo.IndexOf
                (source, value, CompareOptions.IgnoreCase |
                 CompareOptions.IgnoreSymbols | CompareOptions.IgnoreNonSpace);
            return index != -1;
        }
    }
}
