﻿using Play.Accounts.Contracts.Dtos;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Globalization.Time;
using Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration;
using Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration.Events;
using Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration.Rules;
using Play.Merchants.Onboarding.Domain.Common;
using Play.Merchants.Onboarding.Domain.Entities;
using Play.Merchants.Onboarding.Domain.Enums;
using Play.Merchants.Onboarding.Domain.Services;
using Play.Merchants.Onboarding.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.Aggregates.CompanyRegistration;

public class MerchantRegistration : Aggregate<MerchantRegistrationId>
{
    #region Instance Values

    private readonly MerchantRegistrationId _Id;
    private readonly UserRegistrationId _UserRegistrationId;
    private readonly Name _CompanyName;
    private readonly Address _Address;
    private readonly BusinessTypes _BusinessType;
    private readonly MerchantCategoryCodes _MerchantCategoryCode;
    private readonly DateTimeUtc _RegisteredDate;
    private readonly DateTimeUtc? _ConfirmedDate;
    private RegistrationStatuses _Status;

    #endregion

    #region Constructor

    public MerchantRegistration(
        MerchantRegistrationId id, UserRegistrationId userRegistrationId, Name companyName, Address address, BusinessTypes businessType,
        MerchantCategoryCodes merchantCategoryCode, DateTimeUtc registeredDate, DateTimeUtc? confirmedDate, RegistrationStatuses status)
    {
        _Id = id;
        _UserRegistrationId = userRegistrationId;
        _CompanyName = companyName;
        _Address = address;
        _BusinessType = businessType;
        _MerchantCategoryCode = merchantCategoryCode;
        _RegisteredDate = registeredDate;
        _ConfirmedDate = confirmedDate;
        _Status = status;
    }

    #endregion

    #region Instance Members

    /// <exception cref="Play.Domain.ValueObjects.ValueObjectException"></exception>
    public static MerchantRegistration CreateNewMerchantRegistration(
        UserRegistrationId userRegistrationId, string name, string streetAddress, string apartmentNumber, string zipcode, StateAbbreviations state, string city,
        BusinessTypes businessType, MerchantCategoryCodes merchantCategoryCode)
    {
        Name companyName = new(name);
        Address address = new Address(AddressId.New(), streetAddress, apartmentNumber, zipcode, state, city);
        MerchantRegistration merchantRegistration = new MerchantRegistration(MerchantRegistrationId.New(), userRegistrationId, companyName, address,
            businessType, merchantCategoryCode, DateTimeUtc.Now, null, RegistrationStatuses.WaitingForConfirmation);

        // Publish a domain event when a business process has taken place
        merchantRegistration.Raise(new MerchantRegistrationCreatedDomainEvent(merchantRegistration._Id, merchantRegistration._CompanyName,
            merchantRegistration._Address, merchantRegistration._BusinessType, merchantRegistration._MerchantCategoryCode, merchantRegistration._RegisteredDate,
            merchantRegistration._Status));

        return merchantRegistration;
    }

    public override MerchantRegistrationId GetId()
    {
        return (MerchantRegistrationId) _Id;
    }

    public override MerchantRegistrationDto AsDto()
    {
        return new MerchantRegistrationDto
        {
            Id = _Id.Id, UserRegistrationId = _UserRegistrationId.Id, Address = _Address.AsDto(), BusinessType = _BusinessType,
            CompanyName = _CompanyName.Value, ConfirmedDate = _ConfirmedDate, MerchantCategoryCode = $"{_MerchantCategoryCode}",
            RegisteredDate = _RegisteredDate, RegistrationStatus = _Status
        };
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public void Confirm(IUnderwriteMerchants underwritingService)
    {
        Enforce(new MerchantRegistrationCanNotBeConfirmedMoreThanOnce(_Status));
        Enforce(new MerchantRegistrationCanNotBeConfirmedAfterItHasExpired(_Status));

        if (new MerchantIndustryMustNotBeProhibited(_MerchantCategoryCode, underwritingService).IsBroken())
            RejectRegistration();

        if (new MerchantMustNotBeProhibited(_CompanyName, _Address, underwritingService).IsBroken())
            RejectRegistration();

        _Status = RegistrationStatuses.Confirmed;

        Raise(new MerchantRegistrationConfirmedDomainEvent(_Id));
    }

    public void RejectRegistration()
    {
        _Status = RegistrationStatuses.Rejected;
        Raise(new MerchantRegistrationRejectedDomainEvent(_Id));
    }

    #endregion
}