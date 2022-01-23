using System;
using System.Threading;
using System.Threading.Tasks;

namespace Play.Core.Extensions.Tasks;

public static class TaskExtensions
{
    #region Instance Members

    public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
    {
        using (CancellationTokenSource? timeoutCancellationTokenSource = new())
        {
            Task? completedTask = await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token));

            if (completedTask == task)
            {
                timeoutCancellationTokenSource.Cancel();

                return await task; // Very important in order to propagate exceptions
            }

            throw new TimeoutException("The operation has timed out.");
        }
    }

    public static async Task<TResult> WithTimeout<TResult>(this Task<TResult> task, TimeSpan timeout)
    {
        if (await Task.WhenAny(task, Task.Delay(timeout)) != task)
            throw new TimeoutException();

        return await task;
    }

    #endregion
}