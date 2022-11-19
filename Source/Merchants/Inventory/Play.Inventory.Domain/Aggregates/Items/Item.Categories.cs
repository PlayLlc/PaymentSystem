using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Commands;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;
using Play.Inventory.Domain.Services;

namespace Play.Inventory.Domain.Aggregates;

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
    public async Task AddCategories(IRetrieveUsers userService, ICategoryRepository categoryRepository, UpdateItemCategories command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));

        List<Category> categoriesAdded = new List<Category>();

        foreach (var categoryId in command.CategoryIds)
        {
            Category category = await categoryRepository.GetByIdAsync(new SimpleStringId(categoryId)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(Category));

            Enforce(new CategoryMustHaveTheSameMerchant(category, _MerchantId));

            if (!_Categories.Add(category))
                continue;

            categoriesAdded.Add(category);
        }

        if (categoriesAdded.Count == 0)
            return;

        Publish(new ItemCategoriesAdded(this, command.UserId, categoriesAdded.ToArray()));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="AggregateException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task RemoveCategories(IRetrieveUsers userService, UpdateItemCategories command)
    {
        User user = await userService.GetByIdAsync(command.UserId).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

        Enforce(new UserMustBeActiveToUpdateAggregate<Item>(user));
        Enforce(new AggregateMustBeUpdatedByKnownUser<Item>(_MerchantId, user));

        List<Category> categoriesRemoved = new List<Category>();

        foreach (var categoryId in command.CategoryIds)
        {
            Category? category = _Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category is null)
                continue;

            categoriesRemoved.Add(category);
            _Categories.RemoveWhere(a => a.Id == categoryId);
        }

        if (categoriesRemoved.Count == 0)
            return;

        Publish(new ItemCategoriesRemoved(this, user.GetId(), categoriesRemoved.ToArray()));
    }

    #endregion
}