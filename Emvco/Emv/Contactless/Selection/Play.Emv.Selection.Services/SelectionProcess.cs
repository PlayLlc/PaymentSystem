﻿using Play.Core.Threads;
using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts.SignalIn;

namespace Play.Emv.Selection.Services;

internal class SelectionProcess : CommandProcessingQueue
{
    #region Instance Values

    private readonly SelectionStateMachine _SelectionStateMachine;

    #endregion

    #region Constructor

    public SelectionProcess(
        IHandlePcdRequests pcdClient,
        IHandleDisplayRequests displayClient,
        TransactionProfile[] transactionProfiles,
        PoiInformation poiInformation,
        ISendSelectionResponses endpointClient) : base(new CancellationTokenSource())
    {
        _SelectionStateMachine =
            new SelectionStateMachine(pcdClient, displayClient, transactionProfiles, poiInformation, this, endpointClient);
    }

    #endregion

    #region Instance Members

    internal void Enqueue(ActivateSelectionRequest request) => Enqueue((dynamic) request);
    internal void Enqueue(StopSelectionRequest request) => Enqueue((dynamic) request);
    internal void Enqueue(ActivatePcdRequest response) => Enqueue((dynamic) response);
    internal void Enqueue(SelectApplicationDefinitionFileInfoResponse request) => Enqueue((dynamic) request);
    internal void Enqueue(SelectProximityPaymentSystemEnvironmentResponse request) => Enqueue((dynamic) request);
    internal void Enqueue(SendPoiInformationResponse request) => Enqueue((dynamic) request);

    public async Task Handle(ActivateSelectionRequest request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    public async Task Handle(StopSelectionRequest request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    public async Task Handle(ActivatePcdResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    public async Task Handle(SelectApplicationDefinitionFileInfoResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    public async Task Handle(SelectProximityPaymentSystemEnvironmentResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    public async Task Handle(SendPoiInformationResponse request)
    {
        await Task.Run(() => { _SelectionStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    protected override async Task Handle(dynamic command) => await Handle(command).ConfigureAwait(false);

    #endregion
}