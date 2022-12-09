﻿using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Dtos;

namespace Play.Loyalty.Domain.Entities;

public class Discount : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _ItemId;
    private readonly SimpleStringId _VariationId;
    private MoneyValueObject _Price;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private Discount()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Discount(DiscountDto dto)
    {
        Id = new(dto.Id);
        _VariationId = new(dto.VariationId);
        _ItemId = new(dto.ItemId);
        _Price = dto.Price.AsMoney();
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Discount(string id, string itemId, string variationId, Money discountPrice)
    {
        Id = new(id);
        _ItemId = new(itemId);
        _VariationId = new(variationId);
        _Price = discountPrice;
    }

    #endregion

    #region Instance Members

    internal bool IsDiscountedItem(string itemId, string variationId) => (_ItemId == itemId) && (_VariationId == variationId);

    internal Money GetDiscountPrice() => _Price;

    /// <exception cref="ValueObjectException"></exception>
    public void UpdateDiscountPrice(Money price)
    {
        if (!price.IsCommonCurrency(_Price))
            throw new ValueObjectException(
                $"{nameof(Discount)} has the {nameof(NumericCurrencyCode)} {_Price} but an attempt was made to update the amount with a different {nameof(NumericCurrencyCode)} {price.GetNumericCurrencyCode()}");

        _Price = price;
    }

    public override SimpleStringId GetId() => Id;

    public override DiscountDto AsDto() =>
        new()
        {
            Id = Id,
            VariationId = _VariationId,
            Price = _Price.AsDto()
        };

    #endregion
}