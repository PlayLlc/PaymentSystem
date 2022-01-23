﻿using Play.Emv.Configuration;
using Play.Emv.DataElements;
using Play.Emv.Display.Contracts;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts;
using Play.Emv.Selection.Contracts.SignalIn;
using Play.Emv.Selection.Contracts.SignalOut;
using Play.Messaging;
using Play.Messaging.Exceptions;

namespace Play.Emv.Selection.Services;

public class SelectionEndpoint : IMessageChannel, IHandleSelectionRequests, ISendSelectionResponses, IDisposable
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Selection;

    #endregion

    #region Instance Values

    public readonly ChannelIdentifier ChannelIdentifier;
    private readonly IEndpointClient _EndpointClient;
    private readonly SelectionProcess _SelectionProcess;

    #endregion

    #region Constructor

    private SelectionEndpoint(
        ICreateEndpointClient messageRouter,
        IHandlePcdRequests pcdClient,
        IHandleDisplayRequests displayClient,
        TransactionProfile[] transactionProfiles,
        PoiInformation poiInformation)
    {
        ChannelIdentifier = new ChannelIdentifier(ChannelTypeId);
        _SelectionProcess = new SelectionProcess(pcdClient, displayClient, transactionProfiles, poiInformation, this);
        _EndpointClient = messageRouter.CreateEndpointClient(this);
        _EndpointClient.Subscribe();
    }

    #endregion

    #region Instance Members

    public ChannelTypeId GetChannelTypeId() => ChannelType.Selection;
    public ChannelIdentifier GetChannelIdentifier() => ChannelIdentifier;

    #region Requests

    public void Request(RequestMessage message)
    {
        if (message is ActivatePcdRequest activatePcdRequest)
            Request((RequestMessage) activatePcdRequest);
        else if (message is QueryPcdRequest queryPcdRequest)
            Request((RequestMessage) queryPcdRequest);
        else if (message is StopPcdRequest stopPcdRequest)
            Request((RequestMessage) stopPcdRequest);
        else
            throw new UnhandledRequestException(message);
    }

    public void Request(ActivateSelectionRequest message)
    {
        _SelectionProcess.Enqueue(message);
    }

    public void Request(StopSelectionRequest message)
    {
        _SelectionProcess.Enqueue(message);
    }

    #endregion

    #region Responses

    void ISendSelectionResponses.Send(OutSelectionResponse message)
    {
        _EndpointClient.Send(message);
    }

    #endregion

    #region Callbacks

    public void Handle(ResponseMessage message)
    {
        if (message is SelectApplicationDefinitionFileInfoResponse appletFci)
            Handle(appletFci);
        else
            throw new InvalidMessageRoutingException(message, this);
    }

    public void Handle(SelectApplicationDefinitionFileInfoResponse response)
    { }

    #endregion

    public static SelectionEndpoint Create(
        ICreateEndpointClient messageRouter,
        IHandlePcdRequests pcdClient,
        IHandleDisplayRequests displayClient,
        TransactionProfile[] transactionProfiles,
        PoiInformation poiInformation) =>
        new(messageRouter, pcdClient, displayClient, transactionProfiles, poiInformation);

    public void Dispose()
    {
        _EndpointClient.Unsubscribe();
    }

    #endregion
}