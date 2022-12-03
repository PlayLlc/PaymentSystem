using Play.Domain.Common.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Domain.Entities;

public class BusinessInfo : Entity<SimpleStringId>
{
    #region Instance Values

    public BusinessType BusinessType;
    public MerchantCategoryCode MerchantCategoryCode;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private BusinessInfo()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public BusinessInfo(BusinessInfoDto dto)
    {
        Id = new(dto.Id!);
        BusinessType = new(dto.BusinessType);
        MerchantCategoryCode = new(dto.MerchantCategoryCode);
    }

    /// <exception cref="ValueObjectException"></exception>
    public BusinessInfo(string id, string businessType, ushort merchantCategoryCode)
    {
        Id = new(id);
        BusinessType = new(businessType);
        MerchantCategoryCode = new(merchantCategoryCode);
    }

    #endregion

    #region Instance Members

    public override SimpleStringId GetId() => Id;

    public override BusinessInfoDto AsDto() =>
        new()
        {
            Id = Id,
            BusinessType = BusinessType,
            MerchantCategoryCode = MerchantCategoryCode
        };

    #endregion
}