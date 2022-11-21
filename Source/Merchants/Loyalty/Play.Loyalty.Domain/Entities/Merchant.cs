using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Loyalty.Contracts.Dtos;

namespace Play.Loyalty.Domain.Entities;

public class Merchant : Entity<SimpleStringId>
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

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override MerchantDto AsDto()
    {
        return new MerchantDto()
        {
            Id = Id,
            IsActive = IsActive
        };
    }

    #endregion
}