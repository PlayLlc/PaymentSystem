using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Tests.TestDoubles.Aggregates.Brian.DomainEvents;
using Play.Domain.Tests.TestDoubles.Aggregates.Greg.DomainEvents;

namespace Play.Domain.Tests.TestDoubles.Aggregates.Greg.Rules;

public class NameMustBeGreg : BusinessRule<Greg>
{
    #region Instance Values

    private readonly bool _IsValid;
    public override string Message => $"The {nameof(Greg)} must not have a {nameof(Name)} that is equal to Brian";

    #endregion

    #region Constructor

    internal NameMustBeGreg(Name name)
    {
        _IsValid = name.Value == "Greg";
    }

    #endregion

    #region Instance Members

    public override NameWasNotGreg CreateBusinessRuleViolationDomainEvent(Greg item) => new(item, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}