﻿using System.Threading;
using System.Threading.Tasks;

using Play.Core.Threads;
using Play.Emv.Configuration;
using Play.Emv.Pcd.Contracts;

namespace Play.Emv.Pcd.Services;

// HACK: Implement Session and State Machine for this process.
internal class ProximityCouplingDeviceProcess : CommandProcessingQueue
{
    #region Instance Values

    private readonly PcdStateMachine _PcdStateMachine;

    #endregion

    #region Constructor

    public ProximityCouplingDeviceProcess(CardClient cardClient, PcdProtocolConfiguration configuration, ISendPcdResponses pcdEndpoint) :
        base(new CancellationTokenSource())
    {
        _PcdStateMachine = new PcdStateMachine(cardClient, configuration, pcdEndpoint);
    }

    #endregion

    #region Instance Members

    internal void Enqueue(ActivatePcdRequest request) => Enqueue((dynamic) request);
    internal void Enqueue(QueryPcdRequest request) => Enqueue((dynamic) request);
    internal void Enqueue(StopPcdRequest request) => Enqueue((dynamic) request);
    protected override async Task Handle(dynamic command) => await Handle(command).ConfigureAwait(false);

    public async Task Handle(ActivatePcdRequest request)
    {
        await Task.Run(() => { _PcdStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    public async Task Handle(QueryPcdRequest request)
    {
        await Task.Run(() => { _PcdStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    #region STOP

    public async Task Handle(StopPcdRequest request)
    {
        await Task.Run(() =>
        {
            _PcdStateMachine.Handle(request, () =>
            {
                Clear();
                Cancel();
            });
        }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    #endregion

    #endregion
}