using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Tests.TestDoubles.Aggregates.Brian.DomainEvents;

namespace Play.Domain.Tests.TestDoubles.Aggregates.Brian.Rules;

public class NameMustBeBrian : BusinessRule<Brian>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"The {nameof(Brian)} must not have a {nameof(Name)} that is equal to Brian";

    #endregion

    #region Constructor

    internal NameMustBeBrian(Name name)
    {
        _IsValid = name.Value == "Brian";
    }

    #endregion

    #region Instance Members

    public override NameWasNotBrian CreateBusinessRuleViolationDomainEvent(Brian item) => new(item, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}