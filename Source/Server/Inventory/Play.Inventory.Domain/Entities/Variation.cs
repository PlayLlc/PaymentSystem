﻿using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.ValueObjects;

namespace Play.Inventory.Domain.Entities;

/// <summary>
///     A <see cref="Variation" /> is a distinct type of an <see cref="Item" />. For example, an <see cref="Item" /> can
///     have <see cref="Variation" /> objects for XSmall, Small, Medium to distinguish an <see cref="Item" /> from another
/// </summary>
public class Variation : Entity<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _ItemId;

    private MoneyValueObject _Price;

    /// <summary>
    ///     The name of this <see cref="Variation" />. For example, XSmall, Small, Medium or Red, Blue, Green
    /// </summary>
    private Name _Name;

    private Sku? _Sku;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Variation()
    { }

    internal Variation(SimpleStringId id, SimpleStringId itemId, Name name, MoneyValueObject price, Sku? sku = null)
    {
        Id = id;
        _ItemId = itemId;
        _Name = name;
        _Price = price;
        _Sku = sku;
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Variation(VariationDto dto)
    {
        Id = new(dto.Id);
        _ItemId = new(dto.ItemId);
        _Name = new(dto.Name);
        _Price = new(dto.Price);
        _Sku = string.IsNullOrEmpty(dto.Sku) ? null : new Sku(dto.Sku);
    }

    #endregion

    #region Instance Members

    public string GetName() => _Name;

    /// <exception cref="ValueObjectException"></exception>
    internal void UpdatePrice(Money price)
    {
        _Price = price;
    }

    /// <exception cref="ValueObjectException"></exception>
    internal void UpdateSku(string sku)
    {
        _Sku = new(sku);
    }

    internal void UpdateName(string name)
    {
        _Name = new(name);
    }

    public override SimpleStringId GetId() => Id;

    public override VariationDto AsDto() =>
        new()
        {
            Id = Id,
            Name = _Name,
            Price = _Price.AsDto()
        };

    #endregion
}