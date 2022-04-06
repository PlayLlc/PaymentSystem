using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Play.Core;

/// <summary>
///     This class takes commands as input and stores those commands in a queue. The commands are handled one by
///     one on a separate thread until those threads have completed. Once the commands have finished processing
///     this class awaits additional commands to enqueue
/// </summary>
public abstract class CommandProcessingQueue
{
    #region Instance Values

    protected readonly CancellationTokenSource _CancellationTokenSource;
    private readonly ProcessState _ProcessState;

    #endregion

    #region Constructor

    protected CommandProcessingQueue(CancellationTokenSource cancellationTokenSource)
    {
        _CancellationTokenSource = cancellationTokenSource;
        _ProcessState = new ProcessState();
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Communicates a request for cancellation for the current process
    /// </summary>
    protected void Cancel() => _CancellationTokenSource.Cancel();

    protected void Clear()
    {
        lock (_ProcessState)
        {
            _ProcessState.Clear();
        }
    }

    /// <summary>
    ///     This handler is syntactic sugar to allow the most derived type to be added to the concurrent queue. This method
    ///     can be used to enqueue new signals as they come in
    /// </summary>
    /// <param name="command"></param>
    protected void Enqueue(dynamic command) // HACK: Update this to use Message or something strongly typed
    {
        lock (_ProcessState)
        {
            _ProcessState.Enqueue(command);

            if (!_ProcessState.IsActive())
                Task.Run(() => { Process().ConfigureAwait(false); }, _CancellationTokenSource.Token).ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     This handler is syntactic sugar to allow the most derived type to be dequeued from the concurrent queue. This
    ///     method will rely on subsequent
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    protected abstract Task Handle(dynamic command);

    private async Task Process()
    {
        try
        {
            Switch();

            while (TryDequeue(out dynamic? command))
                Task.WhenAny(await Handle(command).ConfigureAwait(false));
        }
        catch
        {
            // log
        }
        finally
        {
            Switch();
        }
    }

    private void Switch()
    {
        lock (_ProcessState)
        {
            _ProcessState.Switch();
        }
    }

    protected bool TryDequeue(out dynamic? command)
    {
        lock (_ProcessState)
        {
            return _ProcessState.TryDequeue(out command);
        }
    }

    #endregion

    internal sealed class ProcessState
    {
        #region Instance Values

        private readonly ConcurrentQueue<dynamic> _Queue;
        private readonly SignalSwitch _SignalSwitch;

        #endregion

        #region Constructor

        public ProcessState()
        {
            _SignalSwitch = new SignalSwitch();
            _Queue = new ConcurrentQueue<dynamic>();
        }

        #endregion

        #region Instance Members

        public void Clear()
        {
            lock (_Queue)
            {
                _Queue.Clear();
            }
        }

        public void Enqueue(dynamic command)
        {
            lock (_Queue)
            {
                _Queue.Enqueue(command);
            }
        }

        public bool IsActive()
        {
            lock (_SignalSwitch)
            {
                return _SignalSwitch.IsActive();
            }
        }

        public void Switch()
        {
            lock (_SignalSwitch)
            {
                _SignalSwitch.Switch();
            }
        }

        public bool TryDequeue(out dynamic? result)
        {
            lock (_Queue)
            {
                return _Queue.TryDequeue(out result);
            }
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
}