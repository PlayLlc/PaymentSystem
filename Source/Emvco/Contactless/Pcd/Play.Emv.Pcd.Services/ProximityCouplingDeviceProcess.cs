using System.Threading;
using System.Threading.Tasks;

using Play.Emv.Pcd.Contracts;
using Play.Messaging;
using Play.Messaging.Threads;

namespace Play.Emv.Pcd.Services;

// HACK: Implement Session and State Machine for this process.
internal class ProximityCouplingDeviceProcess : CommandProcessingQueue<Message>
{
    #region Instance Values

    private readonly PcdStateMachine _PcdStateMachine;

    #endregion

    #region Constructor

    public ProximityCouplingDeviceProcess(CardClient cardClient, PcdConfiguration configuration, IEndpointClient endpointClient) : base(
        new CancellationTokenSource())
    {
        _PcdStateMachine = new PcdStateMachine(cardClient, configuration, endpointClient);
    }

    #endregion

    #region Instance Members

    protected override async Task Handle(Message command)
    {
        if (command is ActivatePcdRequest activatePcdRequest)
        {
            await Handle(activatePcdRequest).ConfigureAwait(false);
            return;
        }

        if (command is QueryPcdRequest queryPcdRequest)
        {
            await Handle(queryPcdRequest).ConfigureAwait(false);
            return;
        }

        if (command is StopPcdRequest stopPcdRequest)
        {
            await Handle(stopPcdRequest).ConfigureAwait(false);
            return;
        }
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Emv.Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="TransmissionError"></exception>
    /// <exception cref="PcdTransmissionException"></exception>
    public async Task Handle(ActivatePcdRequest request)
    {
        await Task.Run(() => { _PcdStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Emv.Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="Play.Messaging.Exceptions.InvalidMessageRoutingException"></exception>
    /// <exception cref="TransmissionError"></exception>
    /// <exception cref="PcdTransmissionException"></exception>
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
    /// <exception cref="Emv.Exceptions.RequestOutOfSyncException"></exception>
    /// <exception cref="Emv.Exceptions.InvalidSignalRequest"></exception>
    /// <exception cref="TransmissionError"></exception>
    /// <exception cref="PcdTransmissionException"></exception>
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