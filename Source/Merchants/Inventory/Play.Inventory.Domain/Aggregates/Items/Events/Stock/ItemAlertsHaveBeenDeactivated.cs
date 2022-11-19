using Play.Domain.Events;
using Play.Inventory.Domain.Entities;

namespace Play.Inventory.Domain;

public record ItemAlertsHaveBeenDeactivated : DomainEvent
{
    #region Instance Values

    public readonly Item Item;
    public readonly string UserId;

    #endregion

    #region Constructor

    public ItemAlertsHaveBeenDeactivated(Item item, string userId) : base(
        $"The {nameof(User)} with the ID: [{userId}] has deactivated {nameof(Alerts)} for the {nameof(Item)} with the ID: [{item.GetId()}];")
    {
        Item = item;
        UserId = userId;
    }

    #endregion
}