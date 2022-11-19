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
        Id = new SimpleStringId(dto.Id);
        CompanyName = new Name(dto.CompanyName);
        IsActive = dto.IsActive;
    }

    /// <exception cref="ValueObjectException"></exception>
    public Merchant(string id, string companyName, bool isActive)
    {
        Id = new SimpleStringId(id);
        CompanyName = new Name(companyName);
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
            CompanyName = CompanyName,
            IsActive = IsActive
        };
    }

    #endregion

    //private readonly HashSet<Store> _Stores;
}