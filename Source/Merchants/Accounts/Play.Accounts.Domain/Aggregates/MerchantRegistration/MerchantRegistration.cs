using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Domain.Exceptions;
using Play.Accounts.Contracts.Commands.MerchantRegistration;

namespace Play.Accounts.Domain.Aggregates;

public class MerchantRegistration : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;
    private readonly DateTimeUtc _RegistrationDate;
    private readonly Name? _CompanyName;
    private Address? _Address;
    private BusinessInfo? _BusinessInfo;
    private MerchantRegistrationStatus _Status;

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private MerchantRegistration()
    { }

    private MerchantRegistration(string id, Name companyName, MerchantRegistrationStatus status, DateTimeUtc registrationDate)
    {
        _Id = id;
        _CompanyName = companyName;
        _Status = status;
        _RegistrationDate = registrationDate;
    }

    #endregion

    #region Instance Members

    public bool IsApproved()
    {
        return _Status == MerchantRegistrationStatuses.Approved;
    }

    /// <exception cref="ValueObjectException"></exception>
    public static MerchantRegistration CreateNewMerchantRegistration(CreateMerchantRegistrationCommand command)
    {
        MerchantRegistration registration = new MerchantRegistration(command.User.MerchantId, new Name(command.Name),
            MerchantRegistrationStatuses.WaitingForRiskAnalysis, DateTimeUtc.Now) {_Status = MerchantRegistrationStatuses.WaitingForRiskAnalysis};

        registration.Publish(new MerchantRegistrationCreated(registration));

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
        Enforce(new MerchantRegistrationMustNotBeRejected(_Status), () => _Status = MerchantRegistrationStatuses.Rejected);

        if (_CompanyName is null)
            throw new CommandOutOfSyncException($"The {nameof(Name)} of the Merchant is required but could not be found");

        _Address = new Address(command.Address);
        _BusinessInfo = new BusinessInfo(command.BusinessInfo);

        Enforce(new MerchantRegistrationIndustryMustNotBeProhibited(_BusinessInfo.MerchantCategoryCode, underwritingService),
            () => _Status = MerchantRegistrationStatuses.Rejected);
        Enforce(new MerchantMustNotBeProhibited(underwritingService, _CompanyName!, _Address!), () => _Status = MerchantRegistrationStatuses.Rejected);

        _Status = MerchantRegistrationStatuses.Approved;
        Publish(new MerchantRegistrationApproved(this));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    public Merchant CreateMerchant()
    {
        Enforce(new MerchantRegistrationMustNotExpire(_Status, _RegistrationDate), () => _Status = MerchantRegistrationStatuses.Expired);
        Enforce(new MerchantRegistrationCannotBeCreatedWithoutApproval(_Status));

        if (_CompanyName is null)
            throw new CommandOutOfSyncException($"The {nameof(Name)} of the Merchant is required but could not be found");
        if (_Address is null)
            throw new CommandOutOfSyncException($"The {nameof(Address)} of the Merchant is required but could not be found");
        if (_BusinessInfo is null)
            throw new CommandOutOfSyncException($"The {nameof(BusinessInfo)} of the Merchant is required but could not be found");

        Merchant merchant = new Merchant(_Id, _CompanyName, _Address, _BusinessInfo, true);
        Publish(new MerchantHasBeenCreated(merchant));

        return merchant;
    }

    public override MerchantRegistrationDto AsDto()
    {
        return new MerchantRegistrationDto
        {
            Id = _Id,
            AddressDto = _Address?.AsDto() ?? new AddressDto(),
            BusinessInfo = _BusinessInfo?.AsDto() ?? new BusinessInfoDto(),
            CompanyName = _CompanyName!.Value,
            RegisteredDate = _RegistrationDate,
            RegistrationStatus = _Status
        };
    }

    #endregion
}