using Play.Domain.Common.ValueObjects;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Common.Entities;
using Play.Domain.Entities;
using Play.Domain.Common.Entitiesd;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;

namespace Play.Loyalty.Domain.Entities;

public class Discount : Entity<SimpleStringId>
{
    #region Instance Values

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
        _Price = dto.Price.GetAmount();
        _NumericCurrencyCode = dto.Price.GetNumericCurrencyCode();
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Discount(string id, string variationId, Money discountPrice)
    {
        Id = new SimpleStringId(id);
        _VariationId = new SimpleStringId(variationId);
        _Price = discountPrice.GetAmount();
        _NumericCurrencyCode = discountPrice.GetNumericCurrencyCode();
    }

    #endregion

    #region Instance Members

    /// <exception cref="BusinessRuleValidationException"></exception>
    public void UpdateDiscountPrice(Money price)
    {
        if (price.GetMajorCurrencyAmount() != _NumericCurrencyCode)
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