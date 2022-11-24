using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Globalization.Currency;

namespace Play.Loyalty.Domain.Aggregates;

public class CurrencyMustBeValid : BusinessRule<LoyaltyProgram, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message =>
        $"The {nameof(LoyaltyProgram)} has attempted to updated a money value with an incorrect {nameof(NumericCurrencyCode)}. Money updates must be in the same currency to ensure equality in comparison operations";

    #endregion

    #region Constructor

    internal CurrencyMustBeValid(NumericCurrencyCode currentCurrencyCode, Money newAmount)
    {
        _IsValid = newAmount.GetMajorCurrencyAmount() == currentCurrencyCode;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override CurrencyWasInvalid CreateBusinessRuleViolationDomainEvent(LoyaltyProgram aggregate) => new CurrencyWasInvalid(aggregate, this);

    #endregion
}