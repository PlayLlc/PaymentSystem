using Microsoft.Extensions.Logging;

using Moq;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Tests.Aggregates;
using Play.Domain.Tests.TestDoubles.EventHandlers;

namespace Play.Domain.Tests.Events;

public class DomainEventHandlerTests
{
    #region Instance Values

    private readonly TestAggregateHandler _TestAggregateHandler;

    #endregion

    #region Constructor

    public DomainEventHandlerTests()
    {
        var a = new Mock<ILogger<TestAggregateHandler>>(MockBehavior.Strict);
        _TestAggregateHandler = new TestAggregateHandler(a.Object);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void ButtStuff()
    {
        var testValue = new Name("MyNameIsJeff");
        var sut = new TestAggregate();
        sut.UpdateName(testValue);
        Assert.NotNull(sut);
    }

    #endregion
}