﻿using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Dtos;

namespace Play.Inventory.Domain.Entities;

public class Store : Entity<SimpleStringId>
{
    #region Instance Values

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public Store(StoreDto dto)
    {
        Id = new(dto.Id);
    }

    /// <exception cref="ValueObjectException"></exception>
    public Store(string id)
    {
        Id = new(id);
    }

    // Constructor for Entity Framework
    private Store()
    { }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId() => Id;

    public override StoreDto AsDto() => new() {Id = Id};

    #endregion
}