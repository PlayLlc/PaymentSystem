using Play.Emv.Ber.DataElements;
using Play.Emv.Pcd.Contracts;
using Play.Emv.Selection.Contracts;
using Play.Messaging;
using Play.Messaging.Exceptions;

namespace Play.Emv.Selection.Services;

public class SelectionEndpoint : IMessageChannel, IDisposable
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = SelectionChannel.Id;

    #endregion

    #region Instance Values

    public readonly ChannelIdentifier ChannelIdentifier;
    private readonly IEndpointClient _EndpointClient;
    private readonly SelectionProcess _SelectionProcess;

    #endregion

    #region Constructor

    private SelectionEndpoint(ICreateEndpointClient messageBus, TransactionProfile[] transactionProfiles, PoiInformation poiInformation)
    {
        _EndpointClient = messageBus.CreateEndpointClient();
        _EndpointClient.Subscribe(this);
        ChannelIdentifier = new ChannelIdentifier(ChannelTypeId);
        _SelectionProcess = new SelectionProcess(_EndpointClient, transactionProfiles, poiInformation);
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
        if (message is ActivateSelectionRequest activateSelectionRequest)
            Request(activateSelectionRequest);
        else if (message is EmptyCombinationSelectionRequest emptyCombinationSelectionRequest)
            Request(emptyCombinationSelectionRequest);
        else if (message is StopSelectionRequest stopSelectionRequest)
            Request(stopSelectionRequest);
        else
            throw new InvalidMessageRoutingException(message, this);
    }

    public void Request(ActivateSelectionRequest message)
    {
        _SelectionProcess.Enqueue(message);
    }

    public void Request(StopSelectionRequest message)
    {
        _SelectionProcess.Enqueue(message);
    }

    public void Request(EmptyCombinationSelectionRequest message)
    {
        _SelectionProcess.Enqueue(message);
    }

    #endregion

    #region Responses

    private void Send(OutSelectionResponse message)
    {
        _EndpointClient.Send(message);
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
        if (message is SelectApplicationDefinitionFileInfoResponse appletFci)
            Handle(appletFci);
        else
            throw new InvalidMessageRoutingException(message, this);
    }

    public void Handle(SelectApplicationDefinitionFileInfoResponse response)
    { }

    #endregion

    public static SelectionEndpoint Create(ICreateEndpointClient messageRouter, TransactionProfile[] transactionProfiles, PoiInformation poiInformation) =>
        new(messageRouter, transactionProfiles, poiInformation);

    public void Dispose()
    {
        _EndpointClient.Unsubscribe(this);
    }

    #endregion
}