using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;

namespace Play.Inventory.Domain;

public record ItemVariationAlreadyExists : BrokenRuleOrPolicyDomainEvent<Item, SimpleStringId>
{
    #region Instance Values

    public readonly Item Item;

    #endregion

    #region Constructor

    public ItemVariationAlreadyExists(Item item, IBusinessRule rule) : base(item, rule)
    {
        Item = item;
    }

    #endregion
}

public record StockItemAlreadyExists : BrokenRuleOrPolicyDomainEvent<Inventory, SimpleStringId>
{
    #region Instance Values

    public readonly Inventory Inventory;

    #endregion

    #region Constructor

    public StockItemAlreadyExists(Inventory inventory, IBusinessRule rule) : base(inventory, rule)
    {
        Inventory = inventory;
    }

    #endregion
}