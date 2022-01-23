using System;

using Play.Emv.Kernel.Contracts.SignalOut;
using Play.Emv.Messaging;
using Play.Emv.Reader.Contracts.SignalOut;
using Play.Emv.Terminal.Configuration;
using Play.Emv.Terminal.Contracts;
using Play.Emv.Terminal.Contracts.SignalIn;
using Play.Emv.Terminal.Contracts.SignalOut;
using Play.Messaging;
using Play.Messaging.Exceptions;

namespace Play.Emv.Terminal.Services;

public class TerminalEndpoint : IMessageChannel, IHandleTerminalRequests, ISendTerminalResponses, IHandleResponsesToTerminal, IDisposable
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = ChannelType.Terminal;

    #endregion

    #region Instance Values

    public readonly ChannelIdentifier ChannelIdentifier;
    private readonly IEndpointClient _EndpointClient;
    private readonly TerminalProcess _TerminalProcess;

    #endregion

    #region Constructor

    private TerminalEndpoint(ITerminalConfigurationRepository terminalConfigurationRepository, ICreateEndpointClient messageRouter)
    {
        ChannelIdentifier = new ChannelIdentifier(ChannelTypeId);
        _TerminalProcess = new TerminalProcess(terminalConfigurationRepository);
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
        if (message is ActivateTerminalRequest activatePcdRequest)
            Request(activatePcdRequest);
        else if (message is QueryTerminalRequest queryPcdRequest)
            Request(queryPcdRequest);
        else
            throw new UnhandledRequestException(message);
    }

    public void Request(ActivateTerminalRequest message)
    {
        _TerminalProcess.Enqueue(message);
    }

    public void Request(QueryTerminalRequest message)
    {
        _TerminalProcess.Enqueue(message);
    }

    #endregion

    #region Responses

    public void Send(QueryTerminalResponse message)
    {
        _EndpointClient.Send(message);
    }

    #endregion

    #region Callbacks

    public void Handle(ResponseMessage message)
    {
        if (message is OutReaderResponse outReaderResponse)
            Handle(outReaderResponse);
        if (message is QueryKernelResponse queryKernelResponse)
            Handle(queryKernelResponse);

        if (message is StopReaderAcknowledgedResponse stopReaderAcknowledgedResponse)
            Handle(stopReaderAcknowledgedResponse);
        else
            throw new InvalidMessageRoutingException(message, this);
    }

    void IHandleResponsesToTerminal.Handle(OutReaderResponse message)
    {
        _TerminalProcess.Enqueue(message);
    }

    void IHandleResponsesToTerminal.Handle(QueryKernelResponse message)
    {
        _TerminalProcess.Enqueue(message);
    }

    void IHandleResponsesToTerminal.Handle(StopReaderAcknowledgedResponse message)
    {
        _TerminalProcess.Enqueue(message);
    }

    #endregion

    public static TerminalEndpoint Create(
        ITerminalConfigurationRepository terminalConfigurationRepository,
        ICreateEndpointClient messageRouter) =>
        new TerminalEndpoint(terminalConfigurationRepository, messageRouter);

    public void Dispose()
    {
        _EndpointClient.Unsubscribe();
    }

    #endregion
}