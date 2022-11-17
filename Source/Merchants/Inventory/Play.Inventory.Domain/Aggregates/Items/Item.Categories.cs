using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;

namespace Play.Inventory.Domain;

/// <summary>
///     This partial represents the Inventory Item's behavior related to creating and managing Categories associated with
///     the Item
/// </summary>
public partial class Item : Aggregate<SimpleStringId>
{
    #region Category

    /// <exception cref="AggregateException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task AddCategory(IRetrieveUsers userService, ICategoryRepository categoryRepository, UpdateCategory command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));
        Category category = await categoryRepository.GetByIdAsync(new SimpleStringId(command.CategoryId)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Category));

        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));
        Enforce(new CategoryMustHaveTheSameMerchant(category, _MerchantId));

        if (!_Categories.Add(category))
            return;

        Publish(new ItemCategoryAdded(this, category!, user.GetId()));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="AggregateException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task RemoveCategory(IRetrieveUsers userService, UpdateCategory command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));

        if (_Categories.RemoveWhere(a => a.Id == command.CategoryId) == 0)
            return;

        Publish(new ItemCategoryRemoved(this, user.GetId(), command.CategoryId));
    }

    #endregion
}