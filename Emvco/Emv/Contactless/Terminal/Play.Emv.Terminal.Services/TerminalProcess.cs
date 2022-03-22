using System.Threading;
using System.Threading.Tasks;

using Play.Core;
using Play.Emv.Acquirer.Contracts.SignalOut;
using Play.Emv.Kernel.Contracts;
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
        _TerminalStateMachine = default;
    }

    #endregion

    #region Instance Members

    internal void Enqueue(InitiateSettlementRequest request)
    {
        Enqueue((dynamic) request);
    }

    internal void Enqueue(AcquirerResponseSignal request)
    {
        Enqueue((dynamic) request);
    }

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

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    private async Task Handle(InitiateSettlementRequest request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    private async Task Handle(AcquirerResponseSignal request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    private async Task Handle(ActivateTerminalRequest request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    private async Task Handle(QueryTerminalRequest request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    private async Task Handle(OutReaderResponse request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    private async Task Handle(QueryKernelResponse request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exceptions.RequestOutOfSyncException"></exception>
    private async Task Handle(StopReaderAcknowledgedResponse request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    protected override Task Handle(dynamic command) => Handle(command);

    #endregion
}