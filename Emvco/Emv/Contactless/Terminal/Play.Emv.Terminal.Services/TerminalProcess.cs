using System.Threading;
using System.Threading.Tasks;

using Play.Core.Threads;
using Play.Emv.Kernel.Contracts.SignalOut;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Terminal.Configuration;
using Play.Emv.Terminal.Contracts.SignalIn;

namespace Play.Emv.Terminal.Services;

internal class TerminalProcess : CommandProcessingQueue
{
    #region Instance Values

    private readonly ITerminalConfigurationRepository _TerminalConfigurationRepository;
    private readonly TerminalStateMachine _TerminalStateMachine;

    #endregion

    #region Constructor

    public TerminalProcess(ITerminalConfigurationRepository terminalConfigurationRepository) : base(new CancellationTokenSource())
    {
        _TerminalConfigurationRepository = terminalConfigurationRepository;

        // BUG: Create a factory or manually initialize the state machine. We can't use default
        _TerminalStateMachine = default; //  new TerminalStateMachine()
    }

    #endregion

    #region Instance Members

    internal void Enqueue(ActivateTerminalRequest request)
    {
        Enqueue((dynamic) request);
    }

    internal void Enqueue(QueryTerminalRequest request)
    {
        Enqueue((dynamic) request);
    }

    internal void Enqueue(OutReaderResponse request)
    {
        Enqueue((dynamic) request);
    }

    internal void Enqueue(QueryKernelResponse request)
    {
        Enqueue((dynamic) request);
    }

    internal void Enqueue(StopReaderAcknowledgedResponse request)
    {
        Enqueue((dynamic) request);
    }

    private async Task Handle(ActivateTerminalRequest request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    private async Task Handle(QueryTerminalRequest request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    private async Task Handle(OutReaderResponse request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    private async Task Handle(QueryKernelResponse request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    private async Task Handle(StopReaderAcknowledgedResponse request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    protected override Task Handle(dynamic command)
    {
        return Handle(command);
    }

    #endregion
}