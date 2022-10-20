using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.ValueObjects;
using Play.Core;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;

using Play.Accounts.Contracts.Commands;
using Play.Domain.Repositories;
using Play.Domain.Exceptions;

namespace Play.Accounts.Domain.Aggregates;

public class MerchantRegistration : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;
    private readonly DateTimeUtc _RegistrationDate;
    private readonly Name? _CompanyName;
    private Address? _Address;
    private BusinessType? _BusinessType;
    private MerchantCategoryCode? _MerchantCategoryCode;
    private MerchantRegistrationStatus _Status;

    #endregion

    #region Constructor

    private MerchantRegistration(string id, Name companyName, MerchantRegistrationStatus status, DateTimeUtc registrationDate)
    {
        _Id = id;
        _CompanyName = companyName;
        _Status = status;
        _RegistrationDate = registrationDate;
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    public static MerchantRegistration CreateNewMerchantRegistration(User user, string companyName)
    {
        MerchantRegistration registration = new MerchantRegistration(user.GetMerchantId(), new Name(companyName),
            MerchantRegistrationStatuses.WaitingForRiskAnalysis, DateTimeUtc.Now) {_Status = MerchantRegistrationStatuses.WaitingForRiskAnalysis};

        registration.Publish(new MerchantRegistrationCreated(registration._Id, companyName));

        return registration;
    }

    public override string GetId()
    {
        return _Id;
    }

    /// <exception cref="AggregateException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public void VerifyMerchantAccount(IUnderwriteMerchants underwritingService, UpdateMerchantRegistrationCommand command)
    {
        Enforce(new MerchantRegistrationMustNotExpire(_Status, _RegistrationDate), () => _Status = MerchantRegistrationStatuses.Expired);
        Enforce(new MerchantRegistrationMustNotBeRejected(_Status, _RegistrationDate), () => _Status = MerchantRegistrationStatuses.Rejected);

        if (_CompanyName is null)
            throw new CommandOutOfSyncException($"The {nameof(Name)} of the Merchant is required but could not be found");

        _Address = new Address(command.Address);
        _BusinessType = new BusinessType(command.BusinessType);
        _MerchantCategoryCode = new MerchantCategoryCode(command.MerchantCategoryCode);

        Enforce(new MerchantIndustryMustNotBeProhibited(_MerchantCategoryCode, underwritingService), () => _Status = MerchantRegistrationStatuses.Rejected);
        Enforce(new MerchantMustNotBeProhibited(underwritingService, _CompanyName!, _Address!), () => _Status = MerchantRegistrationStatuses.Rejected);

        _Status = MerchantRegistrationStatuses.Approved;
        Publish(new MerchantRegistrationApproved(_Id, _CompanyName));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    public Merchant CreateMerchant()
    {
        Enforce(new MerchantRegistrationMustNotExpire(_Status, _RegistrationDate), () => _Status = MerchantRegistrationStatuses.Expired);
        Enforce(new MerchantCannotBeCreatedWithoutApproval(_Status));

        if (_CompanyName is null)
            throw new CommandOutOfSyncException($"The {nameof(Name)} of the Merchant is required but could not be found");
        if (_Address is null)
            throw new CommandOutOfSyncException($"The {nameof(Address)} of the Merchant is required but could not be found");
        if (_BusinessType is null)
            throw new CommandOutOfSyncException($"The {nameof(BusinessType)} of the Merchant is required but could not be found");
        if (_MerchantCategoryCode is null)
            throw new CommandOutOfSyncException($"The {nameof(MerchantCategoryCode)} of the Merchant is required but could not be found");

        var merchant = new Merchant(_Id, _CompanyName, _Address, _BusinessType, _MerchantCategoryCode);
        Publish(new MerchantHasBeenCreated(_Id));

        return merchant;
    }

    public override MerchantRegistrationDto AsDto()
    {
        return new MerchantRegistrationDto
        {
            Id = _Id,
            AddressDto = _Address?.AsDto() ?? new AddressDto(),
            BusinessType = _BusinessType?.Value,
            CompanyName = _CompanyName?.Value,
            MerchantCategoryCode = _MerchantCategoryCode?.Value,
            RegisteredDate = _RegistrationDate,
            RegistrationStatus = _Status
        };
    }

    #endregion
}