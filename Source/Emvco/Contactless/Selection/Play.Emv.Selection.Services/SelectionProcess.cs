using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Display.Contracts;
using Play.Emv.Pcd.Contracts;
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

    public SelectionProcess(
        IHandlePcdRequests pcdClient, IHandleDisplayRequests displayClient, TransactionProfile[] transactionProfiles, PoiInformation poiInformation,
        ISendSelectionResponses endpointClient) : base(new CancellationTokenSource())
    {
        _SelectionStateMachine = new SelectionStateMachine(pcdClient, displayClient, transactionProfiles, poiInformation, this, endpointClient);
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

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Emv.Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private async Task Handle(SelectProximityPaymentSystemEnvironmentResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Emv.Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    private async Task Handle(SendPoiInformationResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    protected override async Task Handle(Message command)
    {
        if (command is ActivateSelectionRequest activateSelectionRequest)
        {
            await Handle(activateSelectionRequest).ConfigureAwait(false);
            return;
        }

        if (command is StopSelectionRequest stopSelectionRequest)
        {
            await Handle(stopSelectionRequest).ConfigureAwait(false);
            return;
        }

        if (command is ActivatePcdResponse activatePcdResponse)
        {
            await Handle(activatePcdResponse).ConfigureAwait(false);
            return;
        }

        if (command is SelectApplicationDefinitionFileInfoResponse selectApplicationDefinitionFileInfoResponse)
        {
            await Handle(selectApplicationDefinitionFileInfoResponse).ConfigureAwait(false);
            return;
        }

        if (command is SelectProximityPaymentSystemEnvironmentResponse selectProximityPaymentSystemEnvironmentResponse)
        {
            await Handle(selectProximityPaymentSystemEnvironmentResponse).ConfigureAwait(false);
            return;
        }

        if (command is SendPoiInformationResponse sendPoiInformationResponse)
        {
            await Handle(sendPoiInformationResponse).ConfigureAwait(false);
            return;
        }
    }

    #endregion
}