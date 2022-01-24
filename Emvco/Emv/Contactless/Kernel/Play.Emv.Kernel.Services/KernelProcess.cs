using System.Threading;
using System.Threading.Tasks;

using Play.Core.Threads;
using Play.Emv.DataElements;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel.Services;

public abstract class KernelProcess : CommandProcessingQueue
{
    #region Constructor

    protected KernelProcess(CancellationTokenSource cancellationTokenSource) : base(cancellationTokenSource)
    { }

    #endregion

    #region Instance Members

    public abstract KernelId GetKernelId();
    public virtual void Enqueue(ActivateKernelRequest message) => Enqueue((dynamic) message);
    public virtual void Enqueue(CleanKernelRequest message) => Enqueue((dynamic) message);
    public virtual void Enqueue(QueryKernelRequest message) => Enqueue((dynamic) message);
    public virtual void Enqueue(StopKernelRequest message) => Enqueue((dynamic) message);
    public virtual void Enqueue(UpdateKernelRequest message) => Enqueue((dynamic) message);
    public virtual void Enqueue(QueryPcdResponse message) => Enqueue((dynamic) message);
    public virtual void Enqueue(QueryTerminalResponse message) => Enqueue((dynamic) message);
    protected abstract Task Handle(ActivateKernelRequest command);
    protected abstract Task Handle(CleanKernelRequest signal);
    protected abstract Task Handle(QueryKernelRequest command);
    protected abstract Task Handle(StopKernelRequest command);
    protected abstract Task Handle(UpdateKernelRequest command);
    protected abstract Task Handle(QueryPcdResponse command);
    protected abstract Task Handle(QueryTerminalResponse command);
    protected override async Task Handle(dynamic command) => await Handle(command).ConfigureAwait(false);

    #endregion
}