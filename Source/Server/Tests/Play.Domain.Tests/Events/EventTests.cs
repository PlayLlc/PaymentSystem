using Castle.Core.Logging;

using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.Tests.Aggregates;

namespace Play.Domain.Tests.Events;

public class EventTests
{
    #region Instance Members

    [Fact]
    public void AggregateWithBusinessRule_BusinessRuleIsValid_ExceptionIsNotThrown()
    {
        var testValue = new Name("Brian");
        var sut = new TestAggregate();
        sut.UpdateName(testValue);
        Assert.NotNull(sut);
    }

    [Fact]
    public void AggregateWithBusinessRule_BusinessRuleIsInValid_ExceptionIsNotThrown()
    {
        var testValue = new Name("MyNameIsJeff");
        var sut = new TestAggregate();

        Assert.Throws<BusinessRuleValidationException>(() => sut.UpdateName(testValue));
    }

    #endregion
}