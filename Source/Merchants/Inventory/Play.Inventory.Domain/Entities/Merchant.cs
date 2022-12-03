using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Inventory.Contracts.Dtos;

namespace Play.Inventory.Domain.Entities;

public class Merchant : Entity<SimpleStringId>
{
    #region Instance Values

    public readonly bool IsActive;
    public readonly Name CompanyName;
    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    /// <exception cref="ValueObjectException"></exception>
    public Merchant(MerchantDto dto)
    {
        Id = new(dto.Id);
        CompanyName = new(dto.CompanyName);
        IsActive = dto.IsActive;
    }

    /// <exception cref="ValueObjectException"></exception>
    public Merchant(string id, string companyName, bool isActive)
    {
        Id = new(id);
        CompanyName = new(companyName);
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
            CompanyName = CompanyName,
            IsActive = IsActive
        };

    #endregion

    //private readonly HashSet<Store> _Stores;
}