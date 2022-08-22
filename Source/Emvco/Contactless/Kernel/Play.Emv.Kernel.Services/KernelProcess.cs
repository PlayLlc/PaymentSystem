using System.Threading;
using System.Threading.Tasks;

using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Messaging;
using Play.Messaging.Threads;

namespace Play.Emv.Kernel.Services;

public abstract class KernelProcess : CommandProcessingQueue<Message>
{
    #region Instance Values

    protected readonly KernelStateMachine _KernelStateMachine;

    #endregion

    #region Constructor

    protected KernelProcess(KernelStateMachine kernelStateMachine, CancellationTokenSource cancellationTokenSource) : base(cancellationTokenSource)
    {
        _KernelStateMachine = kernelStateMachine;
    }

    #endregion

    #region Instance Members

    public virtual void Enqueue(Message message)
    {
        base.Enqueue(message);
    }

    public abstract KernelId GetKernelId();

    protected async Task Handle(ActivateKernelRequest signal)
    {
        Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected async Task Handle(CleanKernelRequest signal)
    {
        Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected async Task Handle(QueryKernelRequest signal)
    {
        Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected async Task Handle(StopKernelRequest signal)
    {
        Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected async Task Handle(UpdateKernelRequest signal)
    {
        Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected async Task Handle(QueryPcdResponse signal)
    {
        Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected async Task Handle(QueryTerminalResponse signal)
    {
        Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected override void Handle(Message command)
    {
        if (command is ActivateKernelRequest activateKernelRequest)
        {
            Handle(activateKernelRequest).ConfigureAwait(false);
            return;
        }

        if (command is CleanKernelRequest cleanKernelRequest)
        {
            Handle(cleanKernelRequest).ConfigureAwait(false);
            return;
        }

        if (command is QueryKernelRequest queryKernelRequest)
        {
            Handle(queryKernelRequest).ConfigureAwait(false);
            return;
        }

        if (command is StopKernelRequest stopKernelRequest)
        {
            Handle(stopKernelRequest).ConfigureAwait(false);
            return;
        }

        if (command is UpdateKernelRequest updateKernelRequest)
        {
            Handle(updateKernelRequest).ConfigureAwait(false);
            return;
        }

        if (command is QueryPcdResponse queryPcdResponse)
        {
            Handle(queryPcdResponse).ConfigureAwait(false);
            return;
        }

        if (command is QueryTerminalResponse queryTerminalResponse)
        {
            Handle(queryTerminalResponse).ConfigureAwait(false);
            return;
        }
    }

    #endregion
}