using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates.Events;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;
using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.Aggregates;

public class Merchant : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;
    private readonly Name _CompanyName;
    private readonly Address _Address;
    private readonly BusinessType _BusinessType;
    private readonly MerchantCategoryCode _MerchantCategoryCode;

    #endregion

    #region Constructor

    public Merchant(string id, Name companyName, Address address, BusinessType businessType, MerchantCategoryCode merchantCategoryCode)
    {
        _Id = id;
        _CompanyName = companyName;
        _Address = address;
        _BusinessType = businessType;
        _MerchantCategoryCode = merchantCategoryCode;
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
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