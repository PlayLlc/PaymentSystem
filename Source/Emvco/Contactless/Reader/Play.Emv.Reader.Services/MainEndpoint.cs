using System;

using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Services;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Reader.Contracts;
using Play.Emv.Reader.Contracts.SignalIn;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Selection.Contracts;
using Play.Messaging;
using Play.Messaging.Exceptions;

namespace Play.Emv.Reader.Services;

public class MainEndpoint : IMessageChannel, IDisposable
{
    #region Static Metadata

    public static readonly ChannelTypeId SelectionSessionId = ReaderChannel.Id;

    #endregion

    #region Instance Values

    public readonly ChannelIdentifier ChannelIdentifier;
    private readonly IEndpointClient _EndpointClient;
    private readonly MainProcess _MainProcess;

    #endregion

    #region Constructor

    private MainEndpoint(ReaderConfiguration configuration, ICreateEndpointClient messageBus)
    {
        ChannelIdentifier = new ChannelIdentifier(SelectionSessionId);
        _EndpointClient = messageBus.GetEndpointClient();
        _EndpointClient.Subscribe(this);
        _MainProcess = new MainProcess(configuration, _EndpointClient);
    }

    #endregion

    #region Instance Members

    public ChannelTypeId GetChannelTypeId() => ChannelIdentifier.GetChannelTypeId();
    public ChannelIdentifier GetChannelIdentifier() => ChannelIdentifier;

    #region Requests

    /// <summary>
    ///     Request
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="InvalidMessageRoutingException"></exception>
    public void Request(RequestMessage message)
    {
        if (message is ActivateReaderRequest activateReaderRequest)
            Request(activateReaderRequest);
        else if (message is QueryReaderRequest queryReaderRequest)
            Request(queryReaderRequest);
        else if (message is UpdateReaderRequest updateReaderRequest)
            Request(updateReaderRequest);
        else if (message is StopReaderRequest stopReaderRequest)
            Request(stopReaderRequest);
        else if (message is AbortReaderRequest abortReaderRequest)
            Request(abortReaderRequest);
        else
            throw new InvalidMessageRoutingException(message, this);
    }

    public void Request(ActivateReaderRequest message)
    {
        _MainProcess.Enqueue(message);
    }

    public void Request(QueryReaderRequest message)
    {
        _MainProcess.Enqueue(message);
    }

    public void Request(UpdateReaderRequest message)
    {
        _MainProcess.Enqueue(message);
    }

    public void Request(StopReaderRequest message)
    {
        _MainProcess.Enqueue(message);
    }

    public void Request(AbortReaderRequest message)
    {
        _MainProcess.Enqueue(message);
    }

    #endregion

    #region Callbacks

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="InvalidMessageRoutingException"></exception>
    public void Handle(ResponseMessage message)
    {
        if (message is OutSelectionResponse outSelectionResponse)
            Handle(outSelectionResponse);
        else if (message is OutKernelResponse outKernelResponse)
            Handle(outKernelResponse);
        else if (message is StopPcdAcknowledgedResponse stopPcdAcknowledgedResponse)
            Handle(stopPcdAcknowledgedResponse);
        else
            throw new InvalidMessageRoutingException(message, this);
    }

    public void Handle(OutSelectionResponse message)
    {
        _MainProcess.Enqueue(message);
    }

    public void Handle(OutKernelResponse message)
    {
        _MainProcess.Enqueue(message);
    }

    public void Handle(StopPcdAcknowledgedResponse message)
    {
        _MainProcess.Enqueue(message);
    }

    #endregion

    public static MainEndpoint Create(ReaderConfiguration readerConfiguration, ICreateEndpointClient messageRouter) => new(readerConfiguration, messageRouter);

    public void Dispose()
    {
        _EndpointClient.Unsubscribe(this);
    }

    #endregion
}