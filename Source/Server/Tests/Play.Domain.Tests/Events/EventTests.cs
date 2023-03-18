using Castle.Core.Logging;

using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.Tests.TestDoubles.Aggregates.Brian;

namespace Play.Domain.Tests.Events;

public class EventTests
{
    #region Instance Members

    [Fact]
    public void BrianAggregate_BusinessRuleIsValid_ExceptionIsNotThrown()
    {
        var testValue = new Name("Brian");
        var sut = new Brian();
        sut.UpdateName(testValue);
        Assert.NotNull(sut);
    }

    [Fact]
    public void BrianAggregate_BusinessRuleIsInValid_ExceptionIsThrown()
    {
        var testValue = new Name("MyNameIsJeff");
        var sut = new Brian();

        Assert.Throws<BusinessRuleValidationException>(() => sut.UpdateName(testValue));
    }

    [Fact]
    public void GregAggregate_BothBusinessRulesAreValid_ExceptionIsNotThrown()
    {
        var testValue = new Name("Greg");
        var sut = new Brian();
        sut.UpdateName(testValue);
        Assert.NotNull(sut);
    }

    [Fact]
    public void GregAggregate_BusinessRuleIsInValid_ExceptionIsThrown()
    {
        var testValue = new Name("MyNameIsJeff");
        var sut = new Brian();

        Assert.Throws<BusinessRuleValidationException>(() => sut.UpdateName(testValue));
    }

    [Fact]
    public void GregAggregate_SharedBusinessRuleIsInValid_ExceptionIsThrown()
    {
        var testValue = new Name("greg");
        var sut = new Brian();

        Assert.Throws<BusinessRuleValidationException>(() => sut.UpdateName(testValue));
    }

    #endregion
}