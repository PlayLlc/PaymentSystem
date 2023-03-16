using Microsoft.Toolkit.HighPerformance.Enumerables;

using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Domain.Tests.Aggregates;
using Play.Domain.Tests.Events;
using Play.Globalization.Currency;

namespace Play.Inventory.Domain.Aggregates;

public class NameMustBeBrian : BusinessRule<TestAggregate, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"The {nameof(TestAggregate)} must not have a {nameof(Name)} that is equal to Brian";

    #endregion

    #region Constructor

    internal NameMustBeBrian(Name name)
    {
        _IsValid = name.Value == "Brian";
    }

    #endregion

    #region Instance Members

    public override NameWasNotBrian CreateBusinessRuleViolationDomainEvent(TestAggregate item) => new(item, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}