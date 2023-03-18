using Microsoft.Extensions.Logging;

using Moq;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Tests.TestDoubles.Aggregates.Brian;
using Play.Domain.Tests.TestDoubles.EventHandlers;

namespace Play.Domain.Tests.Events;

public class DomainEventHandlerTests
{
    #region Instance Values

    private readonly BrianHandler _BrianHandler;

    #endregion

    #region Constructor

    public DomainEventHandlerTests()
    {
        var a = new Mock<ILogger<BrianHandler>>(MockBehavior.Strict);
        _BrianHandler = new BrianHandler(a.Object);
    }

    #endregion

    #region Instance Members

    [Fact]
    public void ButtStuff()
    {
        var testValue = new Name("MyNameIsJeff");
        var sut = new Brian();
        sut.UpdateName(testValue);
        Assert.NotNull(sut);
    }

    #endregion
}