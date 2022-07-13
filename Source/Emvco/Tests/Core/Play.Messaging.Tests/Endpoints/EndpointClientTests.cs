using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Messaging.Tests.Data.Channels;
using Play.Messaging.Tests.Data.Messages;
using Play.Testing.BaseTestClasses;

using Xunit;
using Xunit.Sdk;

namespace Play.Messaging.Tests.Endpoints;

public partial class EndpointClientTests : TestBase
{
    #region Instance Values

    private readonly TestChannel1 _TestChannel1;
    private readonly TestChannel2 _TestChannel2;
    private readonly IEndpointClient _EndpointClient;

    #endregion

    #region Constructor

    public EndpointClientTests()
    {
        MessageBus bus = new();
        _EndpointClient = bus.CreateEndpointClient();
        _TestChannel1 = new TestChannel1(bus);
        _TestChannel2 = new TestChannel2(bus);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void TestChannel1_SendingRequestMessage_ReturnsExpectedValueFromTestChannel()
    {
        int expected = 22;
        TestChannel1RequestMessage message = new(expected);
        _EndpointClient.Send(message);
        int actual = _TestChannel1.GetRequestValue();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestChannel1_SendingMultipleRequestMessages_ReturnsExpectedValues()
    {
        int firstExpected = 22;
        TestChannel1RequestMessage firstMessage = new(firstExpected);
        _EndpointClient.Send(firstMessage);
        int firstActual = _TestChannel1.GetRequestValue();
        Assert.Equal(firstExpected, firstActual);

        int secondExpected = 34;
        TestChannel1RequestMessage secondMessage = new(secondExpected);
        _EndpointClient.Send(secondMessage);
        int secondActual = _TestChannel1.GetRequestValue();
        Assert.Equal(secondExpected, secondActual);
    }

    [Fact]
    public void TestChannel1_SendingMultipleRequestMessages_DisplaysFirstInFirstOutOrder()
    {
        for (int i = 0; i <= 10; i++)
        {
            TestChannel1RequestMessage message = new(i);
            _EndpointClient.Send(message);
        }

        int expected = 10;
        int actual = _TestChannel1.GetRequestValue();
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void TestChannel1_SendingResponseMessage_ReturnsExpectedResult()
    {
        TestChannel1RequestMessage requestMessage = new(10);
        TestChannel1ResponseMessage responseMessage = new(requestMessage.GetCorrelationId(), 11);
        _EndpointClient.Send(requestMessage);
        _EndpointClient.Send(responseMessage);

        Assert.Equal(10, _TestChannel1.GetRequestValue());
        Assert.Equal(11, _TestChannel1.GetResponseValue());
    }

    [Fact]
    public void TestChannel1AndTestChannel2_SendingTestChannel2RequestMessage_CorrectlyRoutesToChannel2()
    {
        int expected = 10;
        TestChannel2RequestMessage requestMessage = new(expected);
        _EndpointClient.Send(requestMessage);
        int actual = _TestChannel2.GetRequestValue();

        Assert.Equal(expected, actual);
    }

    #endregion
}