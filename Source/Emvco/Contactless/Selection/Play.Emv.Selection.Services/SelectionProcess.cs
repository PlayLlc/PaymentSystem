using Play.Core;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.Exceptions;
using Play.Emv.Exceptions;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Reader.Configuration;
using Play.Emv.Selection.Configuration;
using Play.Emv.Selection.Contracts;
using Play.Messaging;

namespace Play.Emv.Selection.Services;

internal class SelectionProcess : CommandProcessingQueue
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

    internal void Enqueue(ActivateSelectionRequest request) => Enqueue((dynamic) request);
    internal void Enqueue(EmptyCombinationSelectionRequest request) => Enqueue((dynamic) request);
    internal void Enqueue(StopSelectionRequest request) => Enqueue((dynamic) request);
    internal void Enqueue(SelectApplicationDefinitionFileInfoResponse request) => Enqueue((dynamic) request);
    internal void Enqueue(SelectProximityPaymentSystemEnvironmentResponse request) => Enqueue((dynamic) request);
    internal void Enqueue(SendPoiInformationResponse request) => Enqueue((dynamic) request);

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="InvalidSignalRequest"></exception>
    public async Task Handle(ActivateSelectionRequest request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    public async Task Handle(EmptyCombinationSelectionRequest request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    public async Task Handle(StopSelectionRequest request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    public async Task Handle(SelectApplicationDefinitionFileInfoResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public async Task Handle(SelectProximityPaymentSystemEnvironmentResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <exception cref="RequestOutOfSyncException"></exception>
    /// <exception cref="DataElementParsingException"></exception>
    public async Task Handle(SendPoiInformationResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    protected override async Task Handle(dynamic command) => await Handle(command).ConfigureAwait(false);

    #endregion
}