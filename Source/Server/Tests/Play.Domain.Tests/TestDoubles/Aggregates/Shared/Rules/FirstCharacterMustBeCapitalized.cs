using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Tests.TestDoubles.Aggregates.Shared.DomainEvents;

namespace Play.Domain.Tests.TestDoubles.Aggregates.Shared.Rules;

public class FirstCharacterMustBeCapitalized<_Aggregate> : BusinessRule<_Aggregate> where _Aggregate : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {typeof(_Aggregate).Name} must have a {nameof(Name)} with a capitalized first character;";

    #endregion

    #region Constructor

    internal FirstCharacterMustBeCapitalized(Name name)
    {
        if (name.Value.Length == 0)
        {
            _IsValid = false;

            return;
        }

        if (!char.IsUpper(name.Value[0]))
        {
            _IsValid = false;

            return;
        }

        _IsValid = true;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override FirstCharacterWasNotCapitalized<_Aggregate> CreateBusinessRuleViolationDomainEvent(_Aggregate aggregate) => new(aggregate, this);

    #endregion
}