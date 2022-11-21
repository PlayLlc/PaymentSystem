using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.Common.Entitiesd;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Domain;

namespace Play.Loyalty.Domain.Entities;

public class Discount : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _ItemId;
    private readonly SimpleStringId _VariationId;
    private readonly NumericCurrencyCode _NumericCurrencyCode;
    private ulong _Price;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private Discount()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Discount(DiscountDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _VariationId = new SimpleStringId(dto.VariationId);
        _ItemId = new SimpleStringId(dto.ItemId);
        _Price = dto.Price.GetAmount();
        _NumericCurrencyCode = dto.Price.GetNumericCurrencyCode();
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Discount(string id, string itemId, string variationId, Money discountPrice)
    {
        Id = new SimpleStringId(id);
        _ItemId = new SimpleStringId(itemId);
        _VariationId = new SimpleStringId(variationId);
        _Price = discountPrice.GetAmount();
        _NumericCurrencyCode = discountPrice.GetNumericCurrencyCode();
    }

    #endregion

    #region Instance Members

    internal bool IsItemDiscounted(string itemId, string variationId)
    {
        return (_ItemId == itemId) && (_VariationId == variationId);
    }

    internal Money GetDiscountPrice()
    {
        return new Money(_Price, _NumericCurrencyCode);
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public void UpdateDiscountPrice(Money price)
    {
        if (price.GetNumericCurrencyCode() != _NumericCurrencyCode)
            throw new BusinessRuleValidationException(
                $"{nameof(Discount)} has the {nameof(NumericCurrencyCode)} {_NumericCurrencyCode} but an attempt was made to update the amount with a different {nameof(NumericCurrencyCode)} {price.GetNumericCurrencyCode()}");

        _Price = price.GetAmount();
    }

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override DiscountDto AsDto()
    {
        return new DiscountDto()
        {
            Id = Id,
            VariationId = _VariationId,
            Price = new Money(_Price, _NumericCurrencyCode)
        };
    }

    #endregion
}