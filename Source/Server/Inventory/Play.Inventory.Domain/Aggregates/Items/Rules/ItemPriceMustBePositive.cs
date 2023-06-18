using Play.Domain.Aggregates;
using Play.Globalization.Currency;
using Play.Inventory.Domain.Services;

namespace Play.Inventory.Domain.Aggregates;

public class ItemPriceMustBePositive : BusinessRule<Item>
{
    #region Instance Values

    private readonly bool _IsValid;
    private readonly Money _Price;
    public override string Message => $"The {nameof(Item)}'s Price must be a positive nonzero number";

    #endregion

    #region Constructor

    internal ItemPriceMustBePositive(Money price)
    {
        _Price = price;
        _IsValid = price.IsPositiveNonZeroAmount();
    }

    #endregion

    #region Instance Members

    public override ItemPriceWasNotPositive CreateBusinessRuleViolationDomainEvent(Item item) => new(item, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}