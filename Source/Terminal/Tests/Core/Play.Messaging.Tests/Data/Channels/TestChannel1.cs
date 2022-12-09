﻿using Play.Messaging.Exceptions;
using Play.Messaging.Tests.Data.Messages;

namespace Play.Messaging.Tests.Data.Channels;

public class TestChannel1 : IMessageChannel
{
    #region Static Metadata

    public static readonly ChannelTypeId ChannelTypeId = TestChannel1Id.Id;

    #endregion

    #region Instance Values

    public readonly ChannelIdentifier ChannelIdentifier;
    private readonly IEndpointClient _EndpointClient;

    #endregion

    #region Instance Values

    private int _RequestValue;
    private int _ResponseValue;

    #endregion

    #region Constructor

    public TestChannel1(ICreateEndpointClient messageRouter)
    {
        _EndpointClient = messageRouter.GetEndpointClient();
        _EndpointClient.Subscribe(this);
        ChannelIdentifier = new ChannelIdentifier(ChannelTypeId);
    }

    #endregion

    #region Instance Members

    public ChannelTypeId GetChannelTypeId() => ChannelTypeId;
    public ChannelIdentifier GetChannelIdentifier() => ChannelIdentifier;

    #region Requests

    public void Request(RequestMessage message)
    {
        if (message is TestChannel1RequestMessage testRequestMessage)
            Request(testRequestMessage);
        else
            throw new InvalidMessageRoutingException(message, this);
    }

    public void Request(TestChannel1RequestMessage message)
    {
        _RequestValue = message.GetValue();
    }

    #endregion

    #region Callbacks

    public void Handle(ResponseMessage message)
    {
        if (message is TestChannel1ResponseMessage testResponseMessage)
            Handle(testResponseMessage);
        else
            throw new InvalidMessageRoutingException(message, this);
    }

    private void Handle(TestChannel1ResponseMessage testResponseMessage)
    {
        _ResponseValue = testResponseMessage.GetValue();
    }

    #endregion

    public int GetRequestValue() => _RequestValue;
    public int GetResponseValue() => _ResponseValue;

    public void Dispose()
    {
        _EndpointClient.Unsubscribe(this);
    }

    #endregion
}