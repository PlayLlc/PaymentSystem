using System.Collections.Generic;
using System.Text;

namespace Play.Core.Extensions.IEnumerable;

public static class EnumeratorExtensions
{
    #region Instance Members

    public static IEnumerator<_> GetEnumerator<_>(this IEnumerator<_> enumerator) => enumerator;

    public static string ToStringAsConcatenatedValues(this IEnumerator<string> enumerator)
    {
        StringBuilder stringBuilder = new();

        foreach (string? value in enumerator)
            stringBuilder.Append($"[{value}] ");

        return stringBuilder.ToString();
    }

    public static string ToStringAsConcatenatedValues(this IEnumerable<string> enumerator)
    {
        StringBuilder stringBuilder = new();

        foreach (string? value in enumerator)
            stringBuilder.Append($"[{value}] ");

        return stringBuilder.ToString();
    }

    #endregion
}