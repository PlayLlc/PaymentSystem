using System.Collections.Generic;

namespace Play.Core.Extensions.IEnumerable;

public static class IEnumeratorExtensions
{
    #region Instance Members

    public static IEnumerator<T> GetEnumerator<T>(this IEnumerator<T> enumerator) => enumerator;

    #endregion
}