using System;

using Play.Emv.Kernel.Contracts;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Messaging;
using Play.Messaging.Exceptions;

namespace Play.Emv.Kernel.Services;

public class KernelEndpoint : IMessageChannel, IDisposable
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = KernelChannel.Id;

    #endregion

    #region Instance Values

    public readonly ChannelIdentifier ChannelIdentifier;
    private readonly IEndpointClient _EndpointClient;
    private readonly KernelRetriever _KernelRetriever;

    #endregion

    #region Constructor

    private KernelEndpoint(KernelRetriever kernelRetriever, IEndpointClient endpointClient)
    {
        ChannelIdentifier = new ChannelIdentifier(ChannelTypeId);
        _KernelRetriever = kernelRetriever;
        _EndpointClient = endpointClient;
        _EndpointClient.Subscribe(this);
    }

    #endregion

    #region Instance Members

    public ChannelTypeId GetChannelTypeId() => ChannelTypeId;
    public ChannelIdentifier GetChannelIdentifier() => ChannelIdentifier;

    #region Requests

    /// <summary>
    ///     Request
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="InvalidMessageRoutingException"></exception>
    public void Request(RequestMessage message)
    {
        if (message is ActivateKernelRequest activateKernelRequest)
            Request(activateKernelRequest);
        else if (message is CleanKernelRequest cleanKernelRequest)
            Request(cleanKernelRequest);
        else if (message is QueryKernelRequest queryKernelRequest)
            Request(queryKernelRequest);
        else if (message is StopKernelRequest stopKernelRequest)
            Request(stopKernelRequest);
        else if (message is UpdateKernelRequest updateKernelRequest)
            Request(updateKernelRequest);
        else
            throw new InvalidMessageRoutingException(message, this);
    }

    public void Request(ActivateKernelRequest message) => _KernelRetriever.Enqueue(message);
    public void Request(CleanKernelRequest message) => _KernelRetriever.Enqueue(message);
    public void Request(QueryKernelRequest message) => _KernelRetriever.Enqueue(message);
    public void Request(StopKernelRequest message) => _KernelRetriever.Enqueue(message);
    public void Request(UpdateKernelRequest message) => _KernelRetriever.Enqueue(message);

    #endregion

    #region Callbacks

    /// <summary>
    ///     Handle
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="InvalidMessageRoutingException"></exception>
    public void Handle(ResponseMessage message)
    {
        if (message is QueryPcdResponse outSelectionResponse)
            Handle(outSelectionResponse);
        else if (message is QueryTerminalResponse outKernelResponse)
            Handle(outKernelResponse);
        else
            throw new InvalidMessageRoutingException(message, this);
    }

    public void Handle(QueryPcdResponse message) => _KernelRetriever.Enqueue(message);
    public void Handle(QueryTerminalResponse message) => _KernelRetriever.Enqueue(message);

    #endregion

    public static KernelEndpoint Create(KernelProcess[] kernelProcesses, IEndpointClient endpointClient) =>
        new(new KernelRetriever(kernelProcesses), endpointClient);

    public void Dispose()
    {
        _EndpointClient.Unsubscribe(this);
    }

    #endregion
}