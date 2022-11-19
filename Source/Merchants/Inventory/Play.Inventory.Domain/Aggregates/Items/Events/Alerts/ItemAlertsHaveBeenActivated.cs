using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain.Aggregates;

public record ItemAlertsHaveBeenActivated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;

    #endregion

    #region Constructor

    public ItemAlertsHaveBeenActivated(Item item, string userId) : base(
        $"The {nameof(User)} with the ID: [{userId}] has activated {nameof(Alerts)} for the {nameof(Item)} with the ID: [{item.GetId()}];")
    {
        Item = item;
        UserId = userId;
    }

    #endregion
}