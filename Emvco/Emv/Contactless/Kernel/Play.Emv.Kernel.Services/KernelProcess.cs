using System.Threading;
using System.Threading.Tasks;

using Play.Core.Threads;
using Play.Emv.DataElements.Emv;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel.Services;

public abstract class KernelProcess : CommandProcessingQueue
{
    #region Instance Values

    protected readonly KernelStateMachine _KernelStateMachine;

    #endregion

    #region Constructor

    protected KernelProcess(KernelStateMachine kernelStateMachine, CancellationTokenSource cancellationTokenSource) : base(
        cancellationTokenSource)
    {
        _KernelStateMachine = kernelStateMachine;
    }

    #endregion

    #region Instance Members

    public abstract KernelId GetKernelId();
    public virtual void Enqueue(ActivateKernelRequest message) => Enqueue((dynamic) message);
    public virtual void Enqueue(CleanKernelRequest message) => Enqueue((dynamic) message);
    public virtual void Enqueue(QueryKernelRequest message) => Enqueue((dynamic) message);
    public virtual void Enqueue(StopKernelRequest message) => Enqueue((dynamic) message);
    public virtual void Enqueue(UpdateKernelRequest message) => Enqueue((dynamic) message);
    public virtual void Enqueue(QueryPcdResponse message) => Enqueue((dynamic) message);

    protected async Task Handle(ActivateKernelRequest signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected async Task Handle(CleanKernelRequest signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected async Task Handle(QueryKernelRequest signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected async Task Handle(StopKernelRequest signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected async Task Handle(UpdateKernelRequest signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected async Task Handle(QueryPcdResponse signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected async Task Handle(QueryTerminalResponse signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    public virtual void Enqueue(QueryTerminalResponse message) => Enqueue((dynamic) message);
    protected override async Task Handle(dynamic command) => await Handle(command).ConfigureAwait(false);

    #endregion
}