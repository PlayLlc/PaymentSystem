using Play.Core;
using Play.Core.Exceptions;
using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Globalization.Currency;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Entities;
using Play.Loyalty.Domain.Entitiesddd;
using Play.Loyalty.Domain.Entitiessss;

namespace Play.Inventory.Domain.Aggregatesd;

public class PercentageMustBeValid : BusinessRule<LoyaltyProgram, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(PercentageOff)} must be a number between 0 and 100;";

    #endregion

    #region Constructor

    /// <exception cref="AggregateException"></exception>
    internal PercentageMustBeValid(byte percentage)
    {
        try
        {
            _ = new Probability(percentage);
            _IsValid = true;
        }
        catch (PlayInternalException)
        {
            _IsValid = false;
        }
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    public override PercentageWasInvalid CreateBusinessRuleViolationDomainEvent(LoyaltyProgram aggregate)
    {
        return new PercentageWasInvalid(aggregate, this);
    }

    #endregion
}

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

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    public override PercentageWasInvalid CreateBusinessRuleViolationDomainEvent(LoyaltyProgram aggregate)
    {
        return new PercentageWasInvalid(aggregate, this);
    }

    #endregion
}