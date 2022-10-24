using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Commands.Merchant;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.ValueObjects;

namespace Play.Accounts.Domain.Aggregates;

public class Merchant : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;
    private Name _CompanyName;
    private Address _Address;
    private BusinessInfo _BusinessInfo;
    private bool _IsActive;

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Merchant()
    { }

    public Merchant(string id, Name companyName, Address address, BusinessInfo businessInfo, bool isActive)
    {
        _Id = id;
        _CompanyName = companyName;
        _Address = address;
        _BusinessInfo = businessInfo;
        _IsActive = isActive;
    }

    #endregion

    #region Instance Members

    public bool IsActive()
    {
        return _IsActive;
    }

    /// <exception cref="NotSupportedException"></exception>
    /// <exception cref="AggregateException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public void Update(IUnderwriteMerchants merchantUnderwriter, UpdateMerchantBusinessInfo command)
    {
        _BusinessInfo = new BusinessInfo(command.BusinessInfo);
        Enforce(new MerchantCategoryCodeMustNotBeProhibited(merchantUnderwriter, _BusinessInfo.MerchantCategoryCode), () => _IsActive = false);

        Publish(new MerchantBusinessInfoHasBeenUpdated(this));
    }

    /// <exception cref="NotSupportedException"></exception>
    /// <exception cref="AggregateException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public void Update(IUnderwriteMerchants merchantUnderwriter, UpdateAddressCommand command)
    {
        _Address = new Address(command.Address);
        Enforce(new MerchantMustNotBeProhibited(merchantUnderwriter, _CompanyName, _Address), () => _IsActive = false);
        Publish(new MerchantAddressHasBeenUpdated(this));
    }

    /// <exception cref="NotSupportedException"></exception>
    /// <exception cref="AggregateException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public void Update(IUnderwriteMerchants merchantUnderwriter, UpdateMerchantCompanyName command)
    {
        _CompanyName = new Name(command.CompanyName);
        Enforce(new MerchantMustNotBeProhibited(merchantUnderwriter, _CompanyName, _Address), () => _IsActive = false);
        Publish(new MerchantCompanyNameBeenUpdated(this));
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
            BusinessInfo = _BusinessInfo.AsDto(),
            CompanyName = _CompanyName.Value,
            IsActive = true
        };
    }

    #endregion
}