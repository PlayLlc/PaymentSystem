﻿using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Loyalty.Contracts.Dtos;

namespace Play.Loyalty.Domain.Entities;

public class DiscountProgram : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly HashSet<Discount> _Discounts;

    private bool _IsActive;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for EF only
    private DiscountProgram()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal DiscountProgram(DiscountsProgramDto dto)
    {
        Id = new(dto.Id);
        _IsActive = dto.IsActive;
        _Discounts = dto.Discounts.Select(a => new Discount(a)).ToHashSet();
    }

    /// <exception cref="ValueObjectException"></exception>
    internal DiscountProgram(string id, bool isActive, IEnumerable<Discount> discounts)
    {
        Id = new(id);
        _IsActive = isActive;
        _Discounts = discounts.ToHashSet();
    }

    #endregion

    #region Instance Members

    internal bool IsActive() => _IsActive;

    internal bool DoesDiscountExist(string discountId) => _Discounts.Any(a => a.Id == discountId);
    internal bool DoesDiscountExist(string itemId, string variationId) => _Discounts.Any(a => a.IsDiscountedItem(itemId, variationId));
    internal void Activate(bool value) => _IsActive = value;

    /// <exception cref="ValueObjectException"></exception>
    public void UpdateDiscountPrice(string discountId, Money price)
    {
        _Discounts.First().UpdateDiscountPrice(price);
    }

    /// <exception cref="ValueObjectException"></exception>
    public bool Add(Discount discount) => _Discounts.Add(discount);

    public bool Remove(string discountId) => _Discounts.RemoveWhere(a => a.Id == discountId) > 0;

    public override SimpleStringId GetId() => Id;

    public override DiscountsProgramDto AsDto()
    {
        return new()
        {
            Id = Id,
            Discounts = _Discounts.Select(a => a.AsDto()),
            IsActive = _IsActive
        };
    }

    #endregion
}