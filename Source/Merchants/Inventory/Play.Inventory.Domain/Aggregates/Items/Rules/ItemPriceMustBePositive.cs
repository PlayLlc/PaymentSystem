﻿using Play.Domain.Aggregates;
using Play.Globalization.Currency;

namespace Play.Inventory.Domain;

public class ItemPriceMustBePositive : BusinessRule<Item, int>
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

    public override ItemPriceWasNotPositive CreateBusinessRuleViolationDomainEvent(Item item)
    {
        return new ItemPriceWasNotPositive(item, _Price, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}