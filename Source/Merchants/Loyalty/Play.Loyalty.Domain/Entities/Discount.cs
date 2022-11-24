using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Domain;
using Play.Loyalty.Contracts.Dtos;

namespace Play.Loyalty.Domain.Entities;

public class Discount : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _ItemId;
    private readonly SimpleStringId _VariationId;
    private MoneyValueObject _MoneyValueObject;

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
        _MoneyValueObject = dto.Price.AsMoney();
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Discount(string id, string itemId, string variationId, Money discountMoneyValueObject)
    {
        Id = new SimpleStringId(id);
        _ItemId = new SimpleStringId(itemId);
        _VariationId = new SimpleStringId(variationId);
        _MoneyValueObject = discountMoneyValueObject;
    }

    #endregion

    #region Instance Members

    internal bool IsDiscountedItem(string itemId, string variationId) => (_ItemId == itemId) && (_VariationId == variationId);

    internal Money GetDiscountPrice() => _MoneyValueObject;

    /// <exception cref="ValueObjectException"></exception>
    public void UpdateDiscountPrice(Money price)
    {
        if (!price.IsCommonCurrency(_MoneyValueObject))
            throw new ValueObjectException(
                $"{nameof(Discount)} has the {nameof(NumericCurrencyCode)} {_MoneyValueObject} but an attempt was made to update the amount with a different {nameof(NumericCurrencyCode)} {price.GetNumericCurrencyCode()}");

        _MoneyValueObject = price;
    }

    public override SimpleStringId GetId() => Id;

    public override DiscountDto AsDto() =>
        new()
        {
            Id = Id,
            VariationId = _VariationId,
            Price = _MoneyValueObject.AsDto()
        };

    #endregion
}