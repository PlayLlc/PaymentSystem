using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;

namespace Play.Messaging.Threads;

/// <summary>
///     This class takes commands as input and stores those commands in a queue. The commands are handled one by
///     one on a separate thread until those threads have completed. Once the commands have finished processing
///     this class awaits additional commands to enqueue
/// </summary>
public abstract class CommandProcessingQueue<T>
{
    #region Instance Values

    private readonly ILogger _Logger;

    protected readonly CancellationTokenSource _CancellationTokenSource;
    private readonly ConcurrentQueue<T> _Queue;
    private readonly SignalSwitch _SignalSwitch;

    private static readonly object _SyncLock = new object();

    #endregion

    #region Constructor

    protected CommandProcessingQueue(CancellationTokenSource cancellationTokenSource)
    {
        _CancellationTokenSource = cancellationTokenSource;

        _SignalSwitch = new SignalSwitch();
        _Queue = new ConcurrentQueue<T>();
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Communicates a request for cancellation for the current process
    /// </summary>
    protected void Cancel() => _CancellationTokenSource.Cancel();

    protected void Clear()
    {
        _Queue.Clear();
    }

    /// <summary>
    ///     This handler is syntactic sugar to allow the most derived type to be added to the concurrent queue. This method
    ///     can be used to enqueue new signals as they come in
    /// </summary>
    /// <param name="command"></param>
    public virtual void Enqueue(T command)
    {
        _Queue.Enqueue(command);

        if (!_SignalSwitch.IsActive())
            Task.Run(() => { Process(); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     This handler is syntactic sugar to allow the most derived type to be dequeued from the concurrent queue. This
    ///     method will rely on subsequent
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    protected abstract void Handle(T command);

    private void Process()
    {
        try
        {
            lock (_SyncLock)
            {
                _SignalSwitch.Switch();

                while (TryDequeue(out T? command))
                {
                    _Logger.LogInformation("Processing command :" + command.ToString());

                    Handle(command);
                }

                _SignalSwitch.Switch();
            }
        }
        catch(Exception e)
        {
            _Logger.LogError(e, "Processing command exception");
        }
    }

    protected bool TryDequeue(out T? command)
    {
        return _Queue.TryDequeue(out command);
    }

    #endregion

    private sealed class SignalSwitch
    {
        #region Instance Values

        private bool _IsActive;

        #endregion

        #region Instance Members

        public bool IsActive() => _IsActive;
        public void Switch() => _IsActive = !_IsActive;

        #endregion
    }
}