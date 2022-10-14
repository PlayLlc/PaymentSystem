using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

using Address = Play.Accounts.Domain.Entities.Address;

namespace Play.Accounts.Domain.Aggregates.Merchants;

public class Merchant : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;
    private readonly Name _CompanyName;
    private readonly Address _Address;
    private readonly BusinessTypes _BusinessType;
    private readonly MerchantCategoryCodes _MerchantCategoryCode;

    #endregion

    #region Constructor

    public Merchant(string id, Name companyName, Address address, BusinessTypes businessType, MerchantCategoryCodes merchantCategoryCode)
    {
        _Id = id;
        _CompanyName = companyName;
        _Address = address;
        _BusinessType = businessType;
        _MerchantCategoryCode = merchantCategoryCode;
    }

    #endregion

    #region Instance Members

    public static Merchant CreateFromMerchantRegistration(Name name, Address address, BusinessTypes businessType, MerchantCategoryCodes merchantCategoryCode)
    {
        return new Merchant(GenerateSimpleStringId(), name, address, businessType, merchantCategoryCode);
    }

    public override string GetId()
    {
        return _Id;
    }

    public override MerchantDto AsDto()
    {
        return new MerchantDto
        {
            Id = _Id,
            AddressDto = _Address.AsDto(),
            BusinessType = _BusinessType,
            CompanyName = _CompanyName.Value,
            MerchantCategoryCode = $"{_MerchantCategoryCode}"
        };
    }

    #endregion
}