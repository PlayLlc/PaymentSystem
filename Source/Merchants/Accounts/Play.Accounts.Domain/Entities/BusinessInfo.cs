using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Entities;
using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.Entities;

public class BusinessInfo : Entity<string>
{
    #region Instance Values

    public BusinessType BusinessType;
    public MerchantCategoryCode MerchantCategoryCode;

    public string Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private BusinessInfo()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public BusinessInfo(BusinessInfoDto dto)
    {
        Id = dto.Id!;
        BusinessType = new BusinessType(dto.BusinessType);
        MerchantCategoryCode = new MerchantCategoryCode(dto.MerchantCategoryCode);
    }

    /// <exception cref="ValueObjectException"></exception>
    public BusinessInfo(string id, string businessType, ushort merchantCategoryCode)
    {
        Id = id;
        BusinessType = new BusinessType(businessType);
        MerchantCategoryCode = new MerchantCategoryCode(merchantCategoryCode);
    }

    #endregion

    #region Instance Members

    public override string GetId()
    {
        return Id;
    }

    public override BusinessInfoDto AsDto()
    {
        return new BusinessInfoDto
        {
            Id = Id,
            BusinessType = BusinessType,
            MerchantCategoryCode = MerchantCategoryCode
        };
    }

    #endregion
}