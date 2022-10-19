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
    private readonly string _UserRegistrationId;
    private readonly DateTimeUtc _RegistrationDate;
    private readonly Name? _CompanyName;
    private readonly Address? _Address;
    private readonly BusinessType? _BusinessType;
    private readonly MerchantCategoryCode? _MerchantCategoryCode;
    private MerchantRegistrationStatus _Status;

    #endregion

    #region Constructor

    private MerchantRegistration(string id, Name companyName)
    {
        _Id = id;
        _CompanyName = companyName;
    }

    private MerchantRegistration(
        string id, string userRegistrationId, Name companyName, Address address, BusinessType businessType, MerchantCategoryCode merchantCategoryCode,
        DateTimeUtc registrationDate, DateTimeUtc? confirmedDate, MerchantRegistrationStatus status)
    {
        _Id = id;
        _UserRegistrationId = userRegistrationId;
        _CompanyName = companyName;
        _Address = address;
        _BusinessType = businessType;
        _MerchantCategoryCode = merchantCategoryCode;
        _RegistrationDate = registrationDate;
        _Status = status;
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    public static MerchantRegistration CreateNewMerchantRegistration(User user, string companyName)
    {
        MerchantRegistration registration = new MerchantRegistration(user.GetMerchantId(), new Name(companyName));

        // TODO: Publish Merchant Created domain event

        return registration;
    }

    /// <exception cref="InvalidOperationException"></exception>
    public Result<Merchant?> CreateMerchant()
    {
        if (_Status != MerchantRegistrationStatuses.Approved)
            return new Result<Merchant?>(null, $"The {nameof(Merchant)} can't be created because registration has not yet been approved");

        if (_CompanyName is null)
            throw new InvalidOperationException();
        if (_Address is null)
            throw new InvalidOperationException();
        if (_BusinessType is null)
            throw new InvalidOperationException();
        if (_MerchantCategoryCode is null)
            throw new InvalidOperationException();

        return new Result<Merchant?>(new Merchant(_Id, _CompanyName, _Address, _BusinessType, _MerchantCategoryCode));
    }

    public override string GetId()
    {
        return _Id;
    }

    public override MerchantRegistrationDto AsDto()
    {
        return new MerchantRegistrationDto
        {
            Id = _Id,
            UserRegistrationId = _UserRegistrationId,
            AddressDto = _Address?.AsDto() ?? new AddressDto(),
            BusinessType = _BusinessType ?? new(),
            CompanyName = _CompanyName?.Value ?? new(),
            MerchantCategoryCode = _MerchantCategoryCode,
            RegisteredDate = _RegistrationDate,
            RegistrationStatus = _Status
        };
    }

    private bool IsUserRegistrationExpired()
    {
        if (_Status == MerchantRegistrationStatuses.Expired)
            return true;

        Result<IBusinessRule> businessRule = GetEnforcementResult(new MerchantCannotBeCreatedWhenRegistrationHasExpired(_RegistrationDate));

        if (!businessRule.Succeeded)
        {
            _Status = MerchantRegistrationStatuses.Expired;

            return true;
        }

        return false;
    }

    /// <exception cref="InvalidOperationException"></exception>
    public Result VerifyMerchantAccount(IUnderwriteMerchants underwritingService)
    {
        if (IsUserRegistrationExpired())
            return new Result($"The {nameof(MerchantRegistration)} has expired");

        if (_CompanyName is null)
            throw new InvalidOperationException();
        if (_MerchantCategoryCode is null)
            throw new InvalidOperationException();
        if (_Address is null)
            throw new InvalidOperationException();

        if (!GetEnforcementResult(new MerchantIndustryMustNotBeProhibited(_MerchantCategoryCode, underwritingService)).Succeeded)
            return RejectRegistration();
        if (!GetEnforcementResult(new MerchantMustNotBeProhibited(underwritingService, _CompanyName!, _Address!)).Succeeded)
            return RejectRegistration();

        _Status = MerchantRegistrationStatuses.Approved;

        Publish(new MerchantRegistrationConfirmedDomainEvent(_Id, _CompanyName));

        return new Result();
    }

    public Result RejectRegistration()
    {
        _Status = MerchantRegistrationStatuses.Rejected;

        return new Result("Merchant account verification failed");
    }

    #endregion
}