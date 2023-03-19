using AutoFixture.AutoMoq;
using AutoFixture;

using Microsoft.Extensions.Logging;

using Moq;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Tests.TestDoubles.Aggregates.Brian;
using Play.Domain.Tests.TestDoubles.EventHandlers;
using Play.Domain.Events;
using Play.Domain.Tests.TestDoubles.Aggregates.Brian.DomainEvents;
using Play.Domain.Exceptions;
using Play.Domain.Tests.TestDoubles.Aggregates.Greg;

namespace Play.Domain.Tests.Events;

public class DomainEventHandlerTests
{
    #region Instance Values

    private readonly IFixture _Fixture;

    private readonly ILogger<BrianHandler> _BrianLogger;
    private readonly ILogger<GregHandler> _GregLogger;

    #endregion

    #region Constructor

    public DomainEventHandlerTests()
    {
        //_BrianLogger = new Mock<ILogger<BrianHandler>>(MockBehavior.Strict).Object;
        //_GregLogger = new Mock<ILogger<GregHandler>>(MockBehavior.Strict).Object;

        _Fixture = new Fixture().Customize(new AutoMoqCustomization());
    }

    #endregion

    #region Instance Members

    [Fact]
    public void Brian_BusinessRuleIsInValid_PublishMethodIsInvoked()
    {
        BrianHandler handler = _Fixture.Create<BrianHandler>();

        var testValue = new Name("MyNameIsNotBrian");
        var sut = new Brian();

        Assert.Throws<BusinessRuleValidationException>(() => sut.UpdateName(testValue));
        Assert.True(handler.WasNameWasNotBrianCalled);
    }

    [Fact]
    public void Brian_BusinessRuleIsValid_PublishMethodIsNotInvoked()
    {
        BrianHandler handler = _Fixture.Create<BrianHandler>();

        var testValue = new Name("Brian");
        var sut = new Brian();
        sut.UpdateName(testValue);
        Assert.False(handler.WasNameWasNotBrianCalled);
    }

    [Fact]
    public void Greg_NameIsNotGreg_PublishMethodIsInvoked()
    {
        GregHandler handler = _Fixture.Create<GregHandler>();

        var testValue = new Name("MyNameIsNotGreg");
        var sut = new Greg();

        Assert.Throws<BusinessRuleValidationException>(() => sut.UpdateName(testValue));
        Assert.True(handler.WasNameWasNotGregCalled);
    }

    [Fact]
    public void Greg_NameIsNotCapitalized_PublishMethodIsInvoked()
    {
        GregHandler handler = _Fixture.Create<GregHandler>();

        var testValue = new Name("greg");
        var sut = new Greg();

        Assert.Throws<BusinessRuleValidationException>(() => sut.UpdateName(testValue));
        Assert.True(handler.WasFirstCharacterWasNotCapitalized);
    }

    [Fact]
    public void Greg_NameIsNotCapitalizedAndNameIsNotGreg_PublishMethodIsInvoked()
    {
        GregHandler handler = _Fixture.Create<GregHandler>();

        var testValue = new Name("jeff");
        var sut = new Greg();
        sut.UpdateName(testValue);
        Assert.False(handler.WasNameWasNotGregCalled);
        Assert.False(handler.WasFirstCharacterWasNotCapitalized);
    }

    [Fact]
    public void GregHandler2()
    {
        GregHandler2 handler = _Fixture.Create<GregHandler2>();

        var testValue = new Name("MyNameIsNotGreg");
        var sut = new Greg();

        Assert.Throws<BusinessRuleValidationException>(() => sut.UpdateName(testValue));
        Assert.True(handler.WasNameWasNotGregCalled);
    }

    #endregion
}