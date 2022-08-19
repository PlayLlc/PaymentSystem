using System.Threading;
using System.Threading.Tasks;

using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Terminal.Configuration;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Messaging;
using Play.Messaging.Threads;

namespace Play.Emv.Terminal.Services;

internal class TerminalProcess : CommandProcessingQueue<Message>
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

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(InitiateSettlementRequest request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    //private async Task Handle(AcquirerResponseSignal request)
    //{
    //    await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    //}

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(ActivateTerminalRequest request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
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
    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(QueryKernelResponse request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(StopReaderAcknowledgedResponse request)
    {
        await Task.Run(() => { _TerminalStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    protected override async Task Handle(Message command)
    {
        if (command is StopReaderAcknowledgedResponse stopReaderAcknowledgedResponse)
        {
            await Handle(stopReaderAcknowledgedResponse).ConfigureAwait(false);
            return;
        }

        if (command is QueryKernelResponse queryKernelResponse)
        {
            await Handle(queryKernelResponse).ConfigureAwait(false);
            return;
        }

        if (command is OutReaderResponse outReaderResponse)
        {
            await Handle(outReaderResponse).ConfigureAwait(false);
            return;
        }

        if (command is QueryTerminalRequest queryTerminalRequest)
        {
            await Handle(queryTerminalRequest).ConfigureAwait(false);
            return;
        }

        if (command is ActivateTerminalRequest activateTerminalRequest)
        {
            await Handle(activateTerminalRequest).ConfigureAwait(false);
            return;
        }

        if (command is InitiateSettlementRequest initiateSettlementRequest)
        {
            await Handle(initiateSettlementRequest).ConfigureAwait(false);
            return;
        }
    }

    #endregion
}