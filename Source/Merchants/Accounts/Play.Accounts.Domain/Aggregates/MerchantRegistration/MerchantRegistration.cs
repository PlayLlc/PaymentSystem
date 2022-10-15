﻿using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates.UserRegistration;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Globalization.Time;

using Address = Play.Accounts.Domain.Entities.Address;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

public class MerchantRegistration : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;
    private readonly string _UserRegistrationId;
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
        string id, string userRegistrationId, Name companyName, Address address, BusinessTypes businessType, MerchantCategoryCodes merchantCategoryCode,
        DateTimeUtc registeredDate, DateTimeUtc? confirmedDate, RegistrationStatuses status)
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
        string userRegistrationId, string name, string streetAddress, string apartmentNumber, string zipcode, StateAbbreviations state, string city,
        BusinessTypes businessType, MerchantCategoryCodes merchantCategoryCode)
    {
        Name companyName = new(name);
        Address address = new Address(GenerateSimpleStringId(), streetAddress, apartmentNumber, zipcode, state, city);
        MerchantRegistration merchantRegistration = new MerchantRegistration(GenerateSimpleStringId(), userRegistrationId, companyName, address, businessType,
            merchantCategoryCode, DateTimeUtc.Now, null, RegistrationStatuses.WaitingForConfirmation);

        // Publish a domain event when a business process has taken place
        merchantRegistration.Publish(new MerchantRegistrationCreatedDomainEvent(merchantRegistration._Id, merchantRegistration._CompanyName,
            merchantRegistration._Address, merchantRegistration._BusinessType, merchantRegistration._MerchantCategoryCode, merchantRegistration._RegisteredDate,
            merchantRegistration._Status));

        return merchantRegistration;
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
            AddressDto = _Address.AsDto(),
            BusinessType = _BusinessType,
            CompanyName = _CompanyName.Value,
            ConfirmedDate = _ConfirmedDate,
            MerchantCategoryCode = $"{_MerchantCategoryCode}",
            RegisteredDate = _RegisteredDate,
            RegistrationStatus = _Status
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

        Publish(new MerchantRegistrationConfirmedDomainEvent(_Id, _CompanyName));
    }

    public void RejectRegistration()
    {
        _Status = RegistrationStatuses.Rejected;
        Publish(new MerchantRegistrationRejectedDomainEvent(_Id));
    }

    #endregion
}