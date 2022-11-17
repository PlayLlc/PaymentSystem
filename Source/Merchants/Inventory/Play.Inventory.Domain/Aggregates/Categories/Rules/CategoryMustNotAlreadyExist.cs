using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Domain;

public class CategoryMustNotAlreadyExist : BusinessRule<Category, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"The {nameof(Category)} must not already exist as an object for the specified {nameof(Merchant)};";

    #endregion

    #region Constructor

    /// <exception cref="AggregateException"></exception>
    internal CategoryMustNotAlreadyExist(ICategoryRepository categoryRepository, SimpleStringId merchantId, Name categoryName)
    {
        Task<bool> result = categoryRepository.DoesCategoryAlreadyExist(merchantId, categoryName);
        Task.WhenAll(result);

        _IsValid = result.Result;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    public override CategoryAlreadyExists CreateBusinessRuleViolationDomainEvent(Category aggregate)
    {
        return new CategoryAlreadyExists(aggregate, this);
    }

    #endregion
}