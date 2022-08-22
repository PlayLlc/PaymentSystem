using System;

using Play.Emv.Display.Configuration;
using Play.Emv.Display.Contracts;
using Play.Emv.Reader.Configuration;
using Play.Messaging;
using Play.Messaging.Exceptions;

namespace Play.Emv.Display.Services;

public class DisplayEndpoint : IMessageChannel, IDisposable
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = DisplayChannel.Id;

    #endregion

    #region Instance Values

    public readonly ChannelIdentifier ChannelIdentifier = new(ChannelTypeId);
    private readonly IEndpointClient _EndpointClient;
    private readonly DisplayProcess _DisplayProcess;

    #endregion

    #region Constructor

    private DisplayEndpoint(
        DisplayConfigurations displayConfiguration, IDisplayMessages displayService, IDisplayLed ledDisplayService, IFormatDisplayMessages messageFormatter,
        IEndpointClient endpointClient)
    {
        _DisplayProcess = new DisplayProcess(displayConfiguration, messageFormatter, displayService, ledDisplayService);
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
        if (message is DisplayMessageRequest activatePcdRequest)
            Request(activatePcdRequest);
        else if (message is StopDisplayRequest queryPcdRequest)
            Request(queryPcdRequest);
        else
            throw new InvalidMessageRoutingException(message, this);
    }

    public void Request(DisplayMessageRequest message)
    {
        _DisplayProcess.Enqueue(message);
    }

    public void Request(StopDisplayRequest message)
    {
        _DisplayProcess.Enqueue(message);
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

    public static DisplayEndpoint Create(
        DisplayConfigurations displayConfiguration, IDisplayMessages displayService, IDisplayLed ledDisplayService, IEndpointClient endpointClient)
    {
        DisplayFormatter formatter = new(displayConfiguration);

        return new DisplayEndpoint(displayConfiguration, displayService, ledDisplayService, formatter, endpointClient);
    }

    public void Dispose()
    {
        _EndpointClient.Unsubscribe(this);
    }

    #endregion
}