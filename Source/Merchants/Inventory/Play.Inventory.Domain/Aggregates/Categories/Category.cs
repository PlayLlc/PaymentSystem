﻿using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Contracts.Dtos;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;

namespace Play.Inventory.Domain;

public class Category : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;

    private readonly Name _Name;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Category()
    { }

    /// <exception cref="ValueObjectException"></exception>
    internal Category(CategoryDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        _MerchantId = new SimpleStringId(dto.MerchantId);
        _Name = new Name(dto.Name);
    }

    /// <exception cref="ValueObjectException"></exception>
    internal Category(string id, string merchantId, string name)
    {
        Id = new SimpleStringId(id);
        _MerchantId = new SimpleStringId(merchantId);
        _Name = new Name(name);
    }

    #endregion

    #region Instance Members

    public string GetName()
    {
        return _Name;
    }

    /// <exception cref="ValueObjectException"></exception>
    internal bool IsMerchantIdEqual(string merchantId)
    {
        return _MerchantId == new SimpleStringId(merchantId);
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="AggregateException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public static async Task<Category> CreateCategory(
        IRetrieveUsers userService, IRetrieveMerchants merchantService, ICategoryRepository categoryRepository, CreateCategory command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Merchant merchant = await merchantService.GetByIdAsync(command.MerchantId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(Merchant));
        Category category = new Category(GenerateSimpleStringId(), command.MerchantId, command.Name);

        category.Enforce(new MerchantMustBeActiveToCreateAggregate<Category>(merchant));
        category.Enforce(new UserMustBeActiveToUpdateAggregate<Category>(user));
        category.Enforce(new AggregateMustBeUpdatedByKnownUser<Category>(merchant.Id, user));
        category.Enforce(new CategoryMustNotAlreadyExist(categoryRepository, merchant.Id, category._Name));

        category.Publish(new CategoryHasBeenCreated(category, merchant.Id, user.GetId()));

        return category;
    }

    /// <exception cref="NotFoundException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task RemoveCategory(IRetrieveUsers userService, RemoveCategory command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Enforce(new UserMustBeActiveToUpdateAggregate<Category>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Category>(_MerchantId, user));

        Publish(new CategoryHasBeenRemoved(this, _MerchantId, user.Id));
    }

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override CategoryDto AsDto()
    {
        return new CategoryDto
        {
            Id = Id,
            MerchantId = _MerchantId,
            Name = _Name
        };
    }

    #endregion
}