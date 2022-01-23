﻿using System;

using Play.Emv.Configuration;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Messaging;
using Play.Messaging.Exceptions;

namespace Play.Emv.Pcd.Services;

public class ProximityCouplingDeviceEndpoint : IMessageChannel, IHandlePcdRequests, ISendPcdResponses, IDisposable
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = ChannelType.ProximityCouplingDevice;

    #endregion

    #region Instance Values

    public readonly ChannelIdentifier ChannelIdentifier;
    private readonly IEndpointClient _EndpointClient;
    private readonly ProximityCouplingDeviceProcess _ProximityCouplingDeviceProcess;

    #endregion

    #region Constructor

    private ProximityCouplingDeviceEndpoint(
        ICreateEndpointClient messageRouter,
        PcdProtocolConfiguration configuration,
        IProximityCouplingDeviceClient pcdClient)
    {
        ChannelIdentifier = new ChannelIdentifier(ChannelTypeId);

        _ProximityCouplingDeviceProcess = new ProximityCouplingDeviceProcess(new CardClient(pcdClient), configuration, this);
        _EndpointClient = messageRouter.CreateEndpointClient(this);
        _EndpointClient.Subscribe();
    }

    #endregion

    #region Instance Members

    public ChannelIdentifier GetChannelIdentifier() => ChannelIdentifier;
    public ChannelTypeId GetChannelTypeId() => ChannelTypeId;

    #region Requests

    public void Request(RequestMessage message)
    {
        if (message is ActivatePcdRequest activatePcdRequest)
            Request(activatePcdRequest);
        else if (message is QueryPcdRequest queryPcdRequest)
            Request(queryPcdRequest);
        else if (message is StopPcdRequest stopPcdRequest)
            Request(stopPcdRequest);
        else
            throw new UnhandledRequestException(message);
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

    #region Responses

    void ISendPcdResponses.Send(ActivatePcdResponse message)
    {
        _EndpointClient.Send(message);
    }

    void ISendPcdResponses.Send(QueryPcdResponse message)
    {
        _EndpointClient.Send(message);
    }

    void ISendPcdResponses.Send(StopPcdAcknowledgedResponse message)
    {
        _EndpointClient.Send(message);
    }

    #endregion

    #region Callbacks

    public void Handle(ResponseMessage message)
    {
        throw new InvalidMessageRoutingException(message, this);
    }

    #endregion

    public static ProximityCouplingDeviceEndpoint Create(
        ICreateEndpointClient messageRouter,
        PcdProtocolConfiguration configuration,
        IProximityCouplingDeviceClient pcdClient) =>
        new(messageRouter, configuration, pcdClient);

    public void Dispose()
    {
        _EndpointClient.Unsubscribe();
    }

    #endregion
}