using System.Threading;
using System.Threading.Tasks;

using Play.Emv.DataElements;
using Play.Emv.Kernel.Contracts.SignalIn;
using Play.Emv.Kernel.Services;
using Play.Emv.Kernel.State;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;

namespace Play.Emv.Kernel2.Services;

public class Kernel2Process : KernelProcess
{
    #region Instance Values

    private readonly KernelStateMachine _KernelStateMachine;

    #endregion

    #region Constructor

    public Kernel2Process(KernelStateMachine kernelStateMachine) : base(new CancellationTokenSource())
    {
        _KernelStateMachine = kernelStateMachine;
    }

    #endregion

    #region Instance Members

    public override KernelId GetKernelId()
    {
        return ShortKernelId.Kernel2;
    }

    public override void Enqueue(StopKernelRequest message)
    {
        // BUG: Possible race condition between these 3 statements
        Clear();
        base.Enqueue(message);
        base.Enqueue(new CleanKernelRequest(message.GetKernelSessionId()));
    }

    protected override async Task Handle(ActivateKernelRequest signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected override async Task Handle(CleanKernelRequest signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected override async Task Handle(QueryKernelRequest signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected override async Task Handle(StopKernelRequest signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected override async Task Handle(UpdateKernelRequest signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected override async Task Handle(QueryPcdResponse signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected override async Task Handle(QueryTerminalResponse signal)
    {
        await Task.Run(() => { _KernelStateMachine.Handle(signal); }).ConfigureAwait(false);
    }

    protected override async Task Handle(dynamic command)
    {
        await Handle(command).ConfigureAwait(false);
    }

    #endregion
}