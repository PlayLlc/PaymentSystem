using System.Threading;
using System.Threading.Tasks;

using Play.Core;
using Play.Emv.Configuration;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Terminal.Configuration;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.Session;
using Play.Emv.Terminal.StateMachine;

namespace Play.Emv.Terminal.Services;

internal class TerminalProcess : CommandProcessingQueue
{
    #region Instance Values

    private readonly TerminalStateMachine _TerminalStateMachine;

    #endregion

    #region Constructor

    public TerminalProcess(
        TerminalConfiguration terminalConfiguration, IGetTerminalState terminalStateResolver,
        IGenerateSequenceTraceAuditNumbers sequenceTraceAuditNumberGenerator) : base(new CancellationTokenSource())
    {
        _TerminalStateMachine = new TerminalStateMachine(terminalConfiguration, terminalStateResolver, sequenceTraceAuditNumberGenerator);
    }

    #endregion

    #region Instance Members

    internal void Enqueue(ActivateTerminalRequest request)
    {
        Enqueue((dynamic) request);
    }

    internal void Enqueue(InitiateSettlementRequest request)
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

    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(ActivateTerminalRequest request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(InitiateSettlementRequest request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(QueryTerminalRequest request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    private async Task Handle(OutReaderResponse request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(QueryKernelResponse request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(StopReaderAcknowledgedResponse request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    protected override Task Handle(dynamic command) => Handle(command);

    #endregion
}