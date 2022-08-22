using System.Threading;
using System.Threading.Tasks;

using Play.Emv.Exceptions;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Reader.Contracts.SignalIn;
using Play.Emv.Selection.Contracts;
using Play.Messaging;
using Play.Messaging.Threads;

namespace Play.Emv.Reader.Services;

internal class MainProcess : CommandProcessingQueue<Message>
{
    #region Instance Values

    private readonly MainStateMachine _MainStateMachine;

    #endregion

    #region Constructor

    public MainProcess(ReaderDatabase readerDatabase, IEndpointClient endpointClient) : base(new CancellationTokenSource())
    {
        _MainStateMachine = new MainStateMachine(readerDatabase, endpointClient);
    }

    #endregion

    #region Instance Members

    protected override async Task Handle(Message command)
    {
        if (command is ActivateReaderRequest activateReaderRequest)
        {
            await Handle(activateReaderRequest).ConfigureAwait(false);
            return;
        }

        if (command is OutSelectionResponse outSelectionResponse)
        {
            await Handle(outSelectionResponse).ConfigureAwait(false);
            return;
        }

        if (command is OutKernelResponse outKernelResponse)
        {
            await Handle(outKernelResponse).ConfigureAwait(false);
            return;
        }

        if (command is StopReaderRequest stopReaderRequest)
        {
            await Handle(stopReaderRequest).ConfigureAwait(false);
            return;
        }

        if (command is StopPcdAcknowledgedResponse stopPcdAcknowledgedResponse)
        {
            await Handle(stopPcdAcknowledgedResponse).ConfigureAwait(false);
            return;
        }
    }

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