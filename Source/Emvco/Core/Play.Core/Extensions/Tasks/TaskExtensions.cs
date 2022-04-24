using System;
using System.Threading;
using System.Threading.Tasks;

namespace Play.Core.Extensions.Tasks;

public static class TaskExtensions
{
    #region Instance Members

    /// <summary>
    ///     TimeoutAfter
    /// </summary>
    /// <param name="task"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
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

    /// <summary>
    ///     WithTimeout
    /// </summary>
    /// <param name="task"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    /// <exception cref="TimeoutException"></exception>
    public static async Task<TResult> WithTimeout<TResult>(this Task<TResult> task, TimeSpan timeout)
    {
        if (await Task.WhenAny(task, Task.Delay(timeout)) != task)
            throw new TimeoutException();

        return await task;
    }

    public static async Task<TResult> WithTimeout<TResult>(this Task<TResult> task, TimeSpan timeout, Func<TResult> timeoutHandler)
    {
        using CancellationTokenSource timeoutCancellationTokenSource = new();

        if (await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token)) != task)
            return timeoutHandler.Invoke();

        timeoutCancellationTokenSource.Cancel();

        return await task;
    }

    public static async Task<TResult> WithTimeout<TResult>(this Task<TResult> task, TimeSpan timeout, Func<Task<TResult>> timeoutHandler)
    {
        using CancellationTokenSource timeoutCancellationTokenSource = new();

        if (await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token)) != task)
            return await timeoutHandler.Invoke();

        timeoutCancellationTokenSource.Cancel();

        return await task;
    }

    public static async Task WithTimeout(this Task task, TimeSpan timeout, Action timeoutHandler)
    {
        using CancellationTokenSource timeoutCancellationTokenSource = new();

        if (await Task.WhenAny(task, Task.Delay(timeout, timeoutCancellationTokenSource.Token)) != task)
        {
            timeoutHandler.Invoke();

            return;
        }

        timeoutCancellationTokenSource.Cancel();

        await task;
    }

    #endregion
}