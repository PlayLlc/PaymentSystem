using System;

using Play.Emv.Pcd.Contracts;
using Play.Messaging;
using Play.Messaging.Exceptions;

namespace Play.Emv.Pcd.Services;

public class ProximityCouplingDeviceEndpoint : IMessageChannel, IDisposable
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = ProximityCouplingDeviceChannel.Id;

    #endregion

    #region Instance Values

    public readonly ChannelIdentifier ChannelIdentifier;
    private readonly IEndpointClient _EndpointClient;
    private readonly ProximityCouplingDeviceProcess _ProximityCouplingDeviceProcess;

    #endregion

    #region Constructor

    private ProximityCouplingDeviceEndpoint(PcdConfiguration configuration, IProximityCouplingDeviceClient pcdClient, IEndpointClient endpointClient)
    {
        ChannelIdentifier = new ChannelIdentifier(ChannelTypeId);
        _EndpointClient = endpointClient;
        _EndpointClient.Subscribe(this);
        _ProximityCouplingDeviceProcess = new ProximityCouplingDeviceProcess(new CardClient(pcdClient), configuration, _EndpointClient);
    }

    #endregion

    #region Instance Members

    public ChannelIdentifier GetChannelIdentifier() => ChannelIdentifier;
    public ChannelTypeId GetChannelTypeId() => ChannelTypeId;

    #region Requests

    /// <summary>
    ///     Request
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="InvalidMessageRoutingException"></exception>
    public void Request(RequestMessage message)
    {
        if (message is ActivatePcdRequest activatePcdRequest)
            Request(activatePcdRequest);
        else if (message is QueryPcdRequest queryPcdRequest)
            Request(queryPcdRequest);
        else if (message is StopPcdRequest stopPcdRequest)
            Request(stopPcdRequest);
        else
            throw new InvalidMessageRoutingException(message, this);
    }

    public void Request(ActivatePcdRequest message)
    {
        _ProximityCouplingDeviceProcess.Enqueue(message);
    }

    public void Request(QueryPcdRequest message)
    {
        _ProximityCouplingDeviceProcess.Enqueue(message);
    }

    public void Request(StopPcdRequest message)
    {
        _ProximityCouplingDeviceProcess.Enqueue(message);
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
        throw new InvalidMessageRoutingException(message, this);
    }

    #endregion

    public static ProximityCouplingDeviceEndpoint Create(
        PcdConfiguration configuration, IProximityCouplingDeviceClient pcdClient, IEndpointClient endpointClient) =>
        new(configuration, pcdClient, endpointClient);

    public void Dispose()
    {
        _EndpointClient.Unsubscribe(this);
    }

    #endregion
}