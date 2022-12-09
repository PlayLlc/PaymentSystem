using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Inventory.Domain.Aggregates;

public record AttemptedCreatingDuplicateStockItem : BrokenRuleOrPolicyDomainEvent<Inventory, SimpleStringId>
{
    #region Instance Values

    public readonly Inventory Inventory;

    #endregion

    #region Constructor

    public AttemptedCreatingDuplicateStockItem(Inventory inventory, IBusinessRule rule) : base(inventory, rule)
    {
        Inventory = inventory;
    }

    #endregion
}