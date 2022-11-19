using Play.Domain.Aggregates;
using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Identity.Contracts.Commands;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.Entities;
using Play.Identity.Domain.Services;

namespace Play.Identity.Domain.Aggregates;

public class Merchant : Aggregate<SimpleStringId>
{
    #region Instance Values

    private Name _CompanyName;
    private Address _Address;
    private BusinessInfo _BusinessInfo;
    private bool _IsActive;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private Merchant()
    { }

    /// <exception cref="ValueObjectException"></exception>
    public Merchant(string id, Name companyName, Address address, BusinessInfo businessInfo, bool isActive)
    {
        Id = new SimpleStringId(id);
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

    public override SimpleStringId GetId()
    {
        return Id;
    }

    public override MerchantDto AsDto()
    {
        return new MerchantDto
        {
            Id = Id,
            Address = _Address.AsDto(),
            BusinessInfo = _BusinessInfo.AsDto(),
            CompanyName = _CompanyName.Value,
            IsActive = true
        };
    }

    #endregion
}