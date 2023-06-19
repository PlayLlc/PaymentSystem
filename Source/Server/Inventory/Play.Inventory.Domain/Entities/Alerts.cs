using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Dtos;

namespace Play.Inventory.Domain.Entities;

public class Alerts : Entity<SimpleStringId>
{
    #region Instance Values

    private bool _IsActive;
    private ushort _LowInventoryThreshold;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Alerts()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public Alerts(AlertsDto dto)
    {
        Id = new(dto.Id!);
        _IsActive = dto.IsActive;
        _LowInventoryThreshold = dto.LowInventoryThreshold;
    }

    /// <exception cref="ValueObjectException"></exception>
    public Alerts(string id, bool isActive, ushort lowInventoryThreshold)
    {
        Id = new(id);
        _IsActive = isActive;
        _LowInventoryThreshold = lowInventoryThreshold;
    }

    #endregion

    #region Instance Members
    public ushort GetLowInventoryThreshold() => _LowInventoryThreshold;;
    public bool IsActive() => _IsActive;
    public bool IsLowInventoryAlertRequired(int quantity, out IEnumerable<User>? subscribers)
    {
        subscribers = null;

        if (!_IsActive)
            return false;

        return quantity <= _LowInventoryThreshold;
    }

    public bool IsOutOfStockAlertRequired(int quantity, out IEnumerable<User>? subscribers)
    {
        subscribers = null;

        if (!_IsActive)
            return false;

        return quantity <= 0;
    }

    public void ActivateAlerts()
    {
        _IsActive = true;
    }

    public void DeactivateAlerts()
    {
        _IsActive = false;
    }

    public void UpdateLowInventoryThreshold(ushort quantity)
    {
        _LowInventoryThreshold = quantity;
    }

    public override SimpleStringId GetId() => Id;

    public override AlertsDto AsDto() =>
        new()
        {
            Id = Id,
            IsActive = _IsActive,
            LowInventoryThreshold = _LowInventoryThreshold
        };

    #endregion
}