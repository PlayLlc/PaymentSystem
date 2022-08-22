using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Reader.Configuration;
using Play.Emv.Selection.Configuration;
using Play.Emv.Selection.Contracts;
using Play.Messaging;
using Play.Messaging.Threads;

namespace Play.Emv.Selection.Services;

internal class SelectionProcess : CommandProcessingQueue<Message>
{
    #region Instance Values

    private readonly SelectionStateMachine _SelectionStateMachine;

    #endregion

    #region Constructor

    public SelectionProcess(IEndpointClient endpointClient, TransactionProfiles transactionProfiles, PoiInformation poiInformation) : base(
        new CancellationTokenSource())
    {
        _SelectionStateMachine = new SelectionStateMachine(endpointClient, transactionProfiles, poiInformation);
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Emv.Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="Emv.Exceptions.InvalidSignalRequest"></exception>
    private async Task Handle(ActivateSelectionRequest request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Emv.Exceptions.RequestOutOfSyncException"></exception>
    private async Task Handle(StopSelectionRequest request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Emv.Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    private async Task Handle(ActivatePcdResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Emv.Exceptions.RequestOutOfSyncException"></exception>
    private async Task Handle(SelectApplicationDefinitionFileInfoResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private async Task Handle(SelectProximityPaymentSystemEnvironmentResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private async Task Handle(SendPoiInformationResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    protected override void Handle(Message command)
    {
        if (command is ActivateSelectionRequest activateSelectionRequest)
        {
            Handle(activateSelectionRequest).ConfigureAwait(false);
            return;
        }

        if (command is StopSelectionRequest stopSelectionRequest)
        {
            Handle(stopSelectionRequest).ConfigureAwait(false);
            return;
        }

        if (command is ActivatePcdResponse activatePcdResponse)
        {
            Handle(activatePcdResponse).ConfigureAwait(false);
            return;
        }

        if (command is SelectApplicationDefinitionFileInfoResponse selectApplicationDefinitionFileInfoResponse)
        {
            Handle(selectApplicationDefinitionFileInfoResponse).ConfigureAwait(false);
            return;
        }

        if (command is SelectProximityPaymentSystemEnvironmentResponse selectProximityPaymentSystemEnvironmentResponse)
        {
            Handle(selectProximityPaymentSystemEnvironmentResponse).ConfigureAwait(false);
            return;
        }

        if (command is SendPoiInformationResponse sendPoiInformationResponse)
        {
            Handle(sendPoiInformationResponse).ConfigureAwait(false);
            return;
        }
    }

    #endregion
}