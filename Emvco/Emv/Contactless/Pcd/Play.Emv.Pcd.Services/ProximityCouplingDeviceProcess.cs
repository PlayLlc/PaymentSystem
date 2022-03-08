using System.Threading;
using System.Threading.Tasks;

using Play.Core.Threads;
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

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Play.Emv.Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="Play.Emv.Pcd.Exceptions.CardReadException"></exception>
    public async Task Handle(ActivatePcdRequest request)
    {
        await Task.Run(() => { _PcdStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Play.Emv.Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="Play.Messaging.Exceptions.InvalidMessageRoutingException"></exception>
    /// <exception cref="Play.Emv.Pcd.Exceptions.CardReadException"></exception>
    public async Task Handle(QueryPcdRequest request)
    {
        await Task.Run(() => { _PcdStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    #region STOP

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Play.Emv.Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="Play.Emv.Exceptions.InvalidSignalRequest"></exception>
    /// <exception cref="Play.Emv.Pcd.Exceptions.CardReadException"></exception>
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

    #region Blocking

    internal async Task<GetDataBatchResponse> Transceive(GetDataBatchRequest message) =>
        await _PcdStateMachine.Transceive(message).ConfigureAwait(false);

    internal async Task<ReadApplicationDataResponse> Transceive(ReadApplicationDataRequest message) =>
        await _PcdStateMachine.Transceive(message).ConfigureAwait(false);

    #endregion
}