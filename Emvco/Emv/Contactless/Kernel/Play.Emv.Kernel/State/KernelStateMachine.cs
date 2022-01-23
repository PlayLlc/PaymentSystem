using System.Threading;

using Play.Emv.Kernel.Contracts.SignalIn;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel.State;

public class KernelStateMachine
{
    #region Instance Values

    private readonly Mutex _Lock;
    private KernelState _KernelState;

    #endregion

    #region Constructor

    public KernelStateMachine(KernelState kernelState)
    {
        _KernelState = kernelState;
        _Lock = new Mutex();
    }

    #endregion

    #region Instance Members

    public void Handle(ActivateKernelRequest signal)
    {
        _Lock.WaitOne();
        _KernelState = _KernelState.Handle(signal);
        _Lock.ReleaseMutex();
    }

    public void Handle(CleanKernelRequest signal)
    {
        _Lock.WaitOne();
        _KernelState = _KernelState.Handle(signal);
        _Lock.ReleaseMutex();
    }

    public void Handle(QueryKernelRequest signal)
    {
        _Lock.WaitOne();
        _KernelState = _KernelState.Handle(signal);
        _Lock.ReleaseMutex();
    }

    public void Handle(StopKernelRequest signal)
    {
        _Lock.WaitOne();
        _KernelState = _KernelState.Handle(signal);
        _Lock.ReleaseMutex();
    }

    public void Handle(UpdateKernelRequest signal)
    {
        _Lock.WaitOne();
        _KernelState = _KernelState.Handle(signal);
        _Lock.ReleaseMutex();
    }

    public void Handle(QueryPcdResponse signal)
    {
        _Lock.WaitOne();
        _KernelState = _KernelState.Handle(signal);
        _Lock.ReleaseMutex();
    }

    public void Handle(QueryTerminalResponse signal)
    {
        _Lock.WaitOne();
        _KernelState = _KernelState.Handle(signal);
        _Lock.ReleaseMutex();
    }

    #endregion
}