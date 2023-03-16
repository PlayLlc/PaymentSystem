using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Domain.Entities;

namespace Play.Loyalty.Domain.Aggregates;

public class DiscountPriceMustBeLowerThanItemPrice : BusinessRule<Programs>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Discount)} price must be lower than the regular listed item price";

    #endregion

    #region Constructor

    internal DiscountPriceMustBeLowerThanItemPrice(InventoryItem inventoryItem, Money discountPrice)
    {
        _IsValid = inventoryItem.IsLowerThanInventoryPrice(discountPrice);
    }

    #endregion

    #region Instance Members

    public override bool IsBroken() => !_IsValid;

    public override DiscountedPriceIsTooHigh CreateBusinessRuleViolationDomainEvent(Programs aggregate) => new(aggregate, this);

    #endregion
}