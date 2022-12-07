using System;
using System.Threading.Tasks;

namespace Play.Core.Extensions.Funcs;

public static class FuncExtensions
{
    #region Instance Members

    public static _ RetryOnFault<_>(Func<_> function, int maxTries)
    {
        for (int i = 0; i < maxTries; i++)
            try
            {
                return function();
            }
            catch
            {
                if (i == (maxTries - 1))
                    throw;
            }

        return default;
    }

    public static async Task<_> RetryOnFault<_>(Func<Task<_>> function, int maxTries)
    {
        for (int i = 0; i < maxTries; i++)
            try
            {
                return await function().ConfigureAwait(false);
            }
            catch
            {
                if (i == (maxTries - 1))
                    throw;
            }

        return default;
    }

    public static async Task<_> RetryOnFault<_>(Func<Task<_>> function, int maxTries, Func<Task> retryWhen)
    {
        for (int i = 0; i < maxTries; i++)
        {
            try
            {
                return await function().ConfigureAwait(false);
            }
            catch
            {
                if (i == (maxTries - 1))
                    throw;
            }

            await retryWhen().ConfigureAwait(false);
        }

        return default;
    }

    #endregion
}