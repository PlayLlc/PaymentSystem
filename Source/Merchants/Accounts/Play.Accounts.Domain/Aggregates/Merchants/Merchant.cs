using Play.Accounts.Contracts.Dtos;
using Play.Domain.Aggregates;
using Play.Merchants.Onboarding.Domain.Common;
using Play.Merchants.Onboarding.Domain.Enums;
using Play.Merchants.Onboarding.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.Aggregates;

public class Merchant : Aggregate<string>
{
    #region Instance Values

    private readonly MerchantId _Id;
    private readonly Name _CompanyName;
    private readonly Address _Address;
    private readonly BusinessTypes _BusinessType;
    private readonly MerchantCategoryCodes _MerchantCategoryCode;

    #endregion

    #region Constructor

    public Merchant(MerchantId id, Name companyName, Address address, BusinessTypes businessType, MerchantCategoryCodes merchantCategoryCode)
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
        return new Merchant(MerchantId.New(), name, address, businessType, merchantCategoryCode);
    }

    public override MerchantId GetId()
    {
        return _Id;
    }

    public override MerchantDto AsDto()
    {
        return new MerchantDto
        {
            Id = _Id.Id, Address = _Address.AsDto(), BusinessType = _BusinessType, CompanyName = _CompanyName.Value,
            MerchantCategoryCode = $"{_MerchantCategoryCode}"
        };
    }

    #endregion
}