using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Loyalty.Contracts.Dtos;

namespace Play.Loyalty.Domain.Aggregates._External;

public class Merchant : Entity<SimpleStringId>, IExternalAggregate
{
    #region Instance Values

    public readonly bool IsActive;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public Merchant(MerchantDto dto)
    {
        Id = new SimpleStringId(dto.Id);
        IsActive = dto.IsActive;
    }

    /// <exception cref="ValueObjectException"></exception>
    public Merchant(string id, bool isActive)
    {
        Id = new SimpleStringId(id);
        IsActive = isActive;
    }

    // Constructor for Entity Framework
    private Merchant()
    { }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId() => Id;

    public override MerchantDto AsDto() =>
        new()
        {
            Id = Id,
            IsActive = IsActive
        };

    #endregion
}