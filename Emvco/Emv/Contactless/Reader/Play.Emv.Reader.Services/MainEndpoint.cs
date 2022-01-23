﻿using System;

using Play.Emv.Display.Contracts;
using Play.Emv.Kernel.Contracts.SignalOut;
using Play.Emv.Kernel.Services;
using Play.Emv.Messaging;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Reader.Contracts.SignalIn;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Selection.Contracts;
using Play.Emv.Selection.Contracts.SignalOut;
using Play.Messaging;
using Play.Messaging.Exceptions;

namespace Play.Emv.Reader.Services;

public class MainEndpoint : IMessageChannel, IReaderEndpoint, IHandleResponsesToReader, ISendReaderResponses, IDisposable
{
    #region Static Metadata

    public static readonly ChannelTypeId SelectionSessionId = ChannelType.Reader;

    #endregion

    #region Instance Values

    public readonly ChannelIdentifier ChannelIdentifier;
    private readonly IEndpointClient _EndpointClient;
    private readonly MainProcess _MainProcess;

    #endregion

    #region Constructor

    private MainEndpoint(
        ActivateReaderRequest activateReaderRequest,
        ICreateEndpointClient messageRouter,
        IHandleDisplayRequests displayEndpoint,
        IHandleSelectionRequests selectionEndpoint,
        KernelRetriever kernelRetriever)
    {
        ChannelIdentifier = new ChannelIdentifier(SelectionSessionId);
        _MainProcess = new MainProcess(activateReaderRequest, selectionEndpoint, displayEndpoint, this, kernelRetriever);

        _EndpointClient = messageRouter.CreateEndpointClient(this);
        _EndpointClient.Subscribe();
    }

    #endregion

    #region Instance Members

    public ChannelTypeId GetChannelTypeId()
    {
        return ChannelIdentifier.GetChannelTypeId();
    }

    public ChannelIdentifier GetChannelIdentifier()
    {
        return ChannelIdentifier;
    }

    #region Requests

    public void Request(RequestMessage message)
    {
        throw new InvalidMessageRoutingException(message, this);
    }

    public void Request(AbortReaderRequest message)
    {
        _MainProcess.Enqueue(message);
    }

    public void Request(ActivateReaderRequest message)
    {
        _MainProcess.Enqueue(message);
    }

    public void Request(QueryReaderRequest message)
    {
        _MainProcess.Enqueue(message);
    }

    public void Request(StopReaderRequest message)
    {
        _MainProcess.Enqueue(message);
    }

    public void Request(UpdateReaderRequest message)
    {
        _MainProcess.Enqueue(message);
    }

    #endregion

    #region Responses

    public void Send(OutReaderResponse message)
    {
        _EndpointClient.Send(message);
    }

    public void Send(QueryReaderResponse message)
    {
        _EndpointClient.Send(message);
    }

    public void Send(StopReaderAcknowledgedResponse message)
    {
        _EndpointClient.Send(message);
    }

    #endregion

    #region Callbacks

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

    internal static MainEndpoint Create(
        ActivateReaderRequest activateReaderRequest,
        ICreateEndpointClient messageRouter,
        IHandleDisplayRequests displayEndpoint,
        IHandleSelectionRequests selectionEndpoint,
        KernelRetriever kernelRetriever)
    {
        return new(activateReaderRequest, messageRouter, displayEndpoint, selectionEndpoint, kernelRetriever);
    }

    public void Dispose()
    {
        _EndpointClient.Unsubscribe();
    }

    #endregion
}