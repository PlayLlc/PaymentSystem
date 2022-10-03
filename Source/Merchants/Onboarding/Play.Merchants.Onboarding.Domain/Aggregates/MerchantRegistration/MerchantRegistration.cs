using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

public class MerchantRegistration : Aggregate<string>
{
    #region Instance Values

    private readonly MerchantRegistrationId _Id;
    private readonly Name _Name;
    private readonly Address _Address;
    private readonly BusinessTypes _BusinessType;
    private readonly MerchantCategoryCodes _MerchantCategoryCode;
    private readonly DateTimeUtc _RegisteredDate;
    private RegistrationStatuses _Status;
    private DateTimeUtc? _ConfirmedDate;

    #endregion

    #region Constructor

    public MerchantRegistration(
        MerchantRegistrationId id, Name name, Address address, BusinessTypes businessType, MerchantCategoryCodes merchantCategoryCode,
        DateTimeUtc registeredDate, DateTimeUtc? confirmedDate, RegistrationStatuses status)
    {
        _Id = id;
        _Name = name;
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
        string name, string streetAddress, string apartmentNumber, string zipcode, StateAbbreviations state, string city, BusinessTypes businessType,
        MerchantCategoryCodes merchantCategoryCode)
    {
        Name companyName = new(name);
        Address address = new Address(AddressId.New(), streetAddress, apartmentNumber, zipcode, state, city);
        MerchantRegistration merchantRegistration = new MerchantRegistration(MerchantRegistrationId.New(), companyName, address, businessType,
            merchantCategoryCode, DateTimeUtc.Now, null, RegistrationStatuses.WaitingForConfirmation);

        // Publish a domain event when a business process has taken place
        merchantRegistration.Raise(new MerchantRegistrationCreated(merchantRegistration._Id, merchantRegistration._Name, merchantRegistration._Address,
            merchantRegistration._BusinessType, merchantRegistration._MerchantCategoryCode, merchantRegistration._RegisteredDate,
            merchantRegistration._Status));

        return merchantRegistration;
    }

    public override MerchantRegistrationId GetId()
    {
        return (MerchantRegistrationId) _Id;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public void Confirm(IUnderwriteMerchants underwritingService)
    {
        Enforce(new MerchantRegistrationCanNotBeConfirmedMoreThanOnce(_Status));
        Enforce(new MerchantRegistrationCanNotBeConfirmedAfterItHasExpired(_Status));

        if (new MerchantIndustryMustNotBeProhibited(_MerchantCategoryCode, underwritingService).IsBroken())
            RejectRegistration();

        if(new MerchantMustNotBeProhibited(_Name, _Address, underwritingService).IsBroken())
            RejectRegistration(); 

        _Status = RegistrationStatuses.Confirmed;

        Raise(new MerchantRegistrationConfirmed(_Id));
    }

    public void RejectRegistration()
    {
        _Status = RegistrationStatuses.Rejected;
        Raise(new MerchantRegistrationRejected(_Id));
    }

    #endregion
}