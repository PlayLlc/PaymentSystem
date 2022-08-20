using System.Threading;
using System.Threading.Tasks;

using Play.Core;
using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Reader.Contracts.SignalIn;
using Play.Emv.Selection.Contracts;
using Play.Messaging;

namespace Play.Emv.Reader.Services;

internal class MainProcess : CommandProcessingQueue
{
    #region Instance Values

    private readonly MainStateMachine _MainStateMachine;

    #endregion

    #region Constructor

    public MainProcess(ReaderConfiguration readerConfiguration, IEndpointClient endpointClient) : base(new CancellationTokenSource())
    {
        _MainStateMachine = new MainStateMachine(readerConfiguration, endpointClient);
    }

    #endregion

    #region Instance Members

    internal void Enqueue(OutSelectionResponse message) => Enqueue((dynamic) message);
    internal void Enqueue(OutKernelResponse message) => Enqueue((dynamic) message);
    internal void Enqueue(AbortReaderRequest message) => Enqueue((dynamic) message);
    internal void Enqueue(ActivateReaderRequest message) => Enqueue((dynamic) message);
    internal void Enqueue(QueryReaderRequest message) => Enqueue((dynamic) message);
    internal void Enqueue(StopReaderRequest message) => Enqueue((dynamic) message);
    internal void Enqueue(UpdateReaderRequest message) => Enqueue((dynamic) message);
    internal void Enqueue(StopPcdAcknowledgedResponse message) => Enqueue((dynamic) message);
    protected override async Task Handle(dynamic command) => await Handle(command).ConfigureAwait(false);

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(ActivateReaderRequest request)
    {
        await Task.Run(() => { _MainStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(OutSelectionResponse request)
    {
        await Task.Run(() => { _MainStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(OutKernelResponse request)
    {
        await Task.Run(() => { _MainStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="RequestOutOfSyncException"></exception>
    private async Task Handle(StopReaderRequest request)
    {
        await Task.Run(() => { _MainStateMachine.Handle(request); }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    private async Task Handle(StopPcdAcknowledgedResponse request)
    {
        await Task.Run(() =>
        {
            // HACK: Figure out what you're supposed to do here
        }, _CancellationTokenSource.Token).ConfigureAwait(false);
    }

    #endregion
}