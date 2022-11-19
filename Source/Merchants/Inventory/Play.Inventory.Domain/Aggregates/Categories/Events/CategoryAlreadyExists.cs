using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Inventory.Domain.Aggregates;

public record CategoryAlreadyExists : BrokenRuleOrPolicyDomainEvent<Category, SimpleStringId>
{
    #region Instance Values

    public readonly Category Category;

    #endregion

    #region Constructor

    public CategoryAlreadyExists(Category category, IBusinessRule rule) : base(category, rule)
    {
        Category = category;
    }

    #endregion
}