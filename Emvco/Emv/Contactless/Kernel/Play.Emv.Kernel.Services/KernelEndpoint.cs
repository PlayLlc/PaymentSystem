using System;

using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Contracts.SignalIn;
using Play.Emv.Kernel.Contracts.SignalOut;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Messaging;
using Play.Messaging.Exceptions;

namespace Play.Emv.Kernel.Services;

public class KernelEndpoint : IMessageChannel, IHandleKernelRequests, ISendKernelResponses, IHandleResponsesToKernel, IDisposable
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Kernel;

    #endregion

    #region Instance Values

    public readonly ChannelIdentifier ChannelIdentifier;
    private readonly IEndpointClient _EndpointClient;
    private readonly KernelRetriever _KernelRetriever;

    #endregion

    #region Constructor

    private KernelEndpoint(KernelRetriever kernelRetriever, ICreateEndpointClient messageRouter)
    {
        ChannelIdentifier = new ChannelIdentifier(ChannelTypeId);
        _KernelRetriever = kernelRetriever;
        _EndpointClient = messageRouter.CreateEndpointClient(this);
        _EndpointClient.Subscribe();
    }

    #endregion

    #region Instance Members

    public ChannelTypeId GetChannelTypeId() => ChannelTypeId;
    public ChannelIdentifier GetChannelIdentifier() => ChannelIdentifier;

    #region Requests

    public void Request(RequestMessage message)
    {
        if (message is ActivatePcdRequest activatePcdRequest)
            Request(activatePcdRequest);
        else if (message is QueryPcdRequest queryPcdRequest)
            Request(queryPcdRequest);
        else
            throw new UnhandledRequestException(message);
    }

    public void Request(ActivateKernelRequest message) => _KernelRetriever.Enqueue(message);
    public void Request(CleanKernelRequest message) => _KernelRetriever.Enqueue(message);
    public void Request(QueryKernelRequest message) => _KernelRetriever.Enqueue(message);
    public void Request(StopKernelRequest message) => _KernelRetriever.Enqueue(message);
    public void Request(UpdateKernelRequest message) => _KernelRetriever.Enqueue(message);

    #endregion

    #region Responses

    public void Send(OutKernelResponse message)
    {
        _EndpointClient.Send(message);
    }

    public void Send(QueryKernelResponse message)
    {
        _EndpointClient.Send(message);
    }

    #endregion

    #region Callbacks

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

    public static KernelEndpoint Create(KernelRetriever kernelRetriever, ICreateEndpointClient messageRouter) =>
        new(kernelRetriever, messageRouter);

    public void Dispose()
    {
        _EndpointClient.Unsubscribe();
    }

    #endregion
}