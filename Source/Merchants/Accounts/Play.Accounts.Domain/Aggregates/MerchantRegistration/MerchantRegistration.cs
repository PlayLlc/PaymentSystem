using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates.Events;
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

using Play.Domain.Repositories;

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
            MerchantRegistrationStatuses.WaitingForRiskAnalysis, DateTimeUtc.Now);
        registration._Status = MerchantRegistrationStatuses.WaitingForRiskAnalysis;

        registration.Publish(new MerchantRegistrationCreated(registration._Id, companyName));

        return registration;
    }

    public override string GetId()
    {
        return _Id;
    }

    private bool IsMerchantRegistrationExpired()
    {
        Result<IBusinessRule> businessRule = GetEnforcementResult(new MerchantRegistrationCannotCompleteIfExpired(_Status, _RegistrationDate));

        if (!businessRule.Succeeded)
        {
            _Status = MerchantRegistrationStatuses.Expired;

            return true;
        }

        return false;
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="AggregateException"></exception>
    public Result VerifyMerchantAccount(
        IUnderwriteMerchants underwritingService, Address address, BusinessType businessType, MerchantCategoryCode merchantCategoryCode)
    {
        if (IsMerchantRegistrationExpired())
            return new Result($"The {nameof(MerchantRegistration)} has expired");

        if (_CompanyName is null)
            throw new InvalidOperationException();

        _Address = address;
        _BusinessType = businessType;
        _MerchantCategoryCode = merchantCategoryCode;

        if (!GetEnforcementResult(new MerchantIndustryMustNotBeProhibited(_MerchantCategoryCode, underwritingService)).Succeeded)
            return RejectRegistration();
        if (!GetEnforcementResult(new MerchantMustNotBeProhibited(underwritingService, _CompanyName!, _Address!)).Succeeded)
            return RejectRegistration();

        _Status = MerchantRegistrationStatuses.Approved;

        Publish(new MerchantRegistrationApproved(_Id, _CompanyName));

        return new Result();
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public Merchant CreateMerchant()
    {
        Enforce(new MerchantCannotBeCreatedWithoutApproval(_Status));

        if (_CompanyName is null)
            throw new InvalidOperationException();
        if (_Address is null)
            throw new InvalidOperationException();
        if (_BusinessType is null)
            throw new InvalidOperationException();
        if (_MerchantCategoryCode is null)
            throw new InvalidOperationException();

        var merchant = new Merchant(_Id, _CompanyName, _Address, _BusinessType, _MerchantCategoryCode);
        Publish(new MerchantHasBeenCreated(_Id));

        return merchant;
    }

    public Result RejectRegistration()
    {
        _Status = MerchantRegistrationStatuses.Rejected;

        return new Result("Merchant account verification failed");
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