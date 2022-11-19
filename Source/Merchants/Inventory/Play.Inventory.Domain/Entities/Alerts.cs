using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Dtos;

namespace Play.Inventory.Domain.Entities;

public class Alerts : Entity<SimpleStringId>
{
    #region Instance Values

    public bool IsActive;
    public ushort LowInventoryThreshold;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Alerts()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public Alerts(AlertsDto dto)
    {
        Id = new SimpleStringId(dto.Id!);
        IsActive = dto.IsActive;
        LowInventoryThreshold = dto.LowInventoryThreshold;
    }

    /// <exception cref="ValueObjectException"></exception>
    public Alerts(string id, bool isActive, ushort lowInventoryThreshold)
    {
        Id = new SimpleStringId(id);
        IsActive = isActive;
        LowInventoryThreshold = lowInventoryThreshold;
    }

    #endregion

    #region Instance Members

    public bool IsLowInventoryAlertRequired(int quantity, out IEnumerable<User>? subscribers)
    {
        subscribers = null;

        if (!IsActive)
            return false;

        return quantity <= LowInventoryThreshold;
    }

    public bool IsOutOfStockAlertRequired(int quantity, out IEnumerable<User>? subscribers)
    {
        subscribers = null;

        if (!IsActive)
            return false;

        return quantity <= 0;
    }

    public void ActivateAlerts()
    {
        IsActive = true;
    }

    public void DeactivateAlerts()
    {
        IsActive = false;
    }

    public void UpdateLowInventoryThreshold(ushort quantity)
    {
        LowInventoryThreshold = quantity;
    }

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override AlertsDto AsDto()
    {
        return new AlertsDto
        {
            Id = Id,
            IsActive = IsActive,
            LowInventoryThreshold = LowInventoryThreshold
        };
    }

    #endregion
}