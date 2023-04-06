﻿using Play.Domain.Aggregates;
using Play.Domain.Common.Dtos;
using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Identity.Contracts.Commands.MerchantRegistration;
using Play.Identity.Contracts.Dtos;
using Play.Registration.Domain.Aggregates.MerchantRegistration.DomainEvents;
using Play.Registration.Domain.Aggregates.MerchantRegistration.DomainEvents.Rules;
using Play.Registration.Domain.Aggregates.MerchantRegistration.Rules;
using Play.Registration.Domain.Entities;
using Play.Registration.Domain.Enums;
using Play.Registration.Domain.Repositories;
using Play.Registration.Domain.Services;
using Play.Registration.Domain.ValueObjects;

namespace Play.Registration.Domain.Aggregates.MerchantRegistration;

public class MerchantRegistration : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly DateTimeUtc _RegistrationDate;
    private readonly Name _CompanyName;
    private Address? _Address;
    private BusinessInfo? _BusinessInfo;
    private MerchantRegistrationStatus _Status;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private MerchantRegistration()
    { }

    /// <exception cref="ValueObjectException"></exception>
    private MerchantRegistration(string id, Name companyName, MerchantRegistrationStatus status, DateTimeUtc registrationDate)
    {
        Id = new SimpleStringId(id);
        _CompanyName = companyName;
        _Status = status;
        _RegistrationDate = registrationDate;
    }

    #endregion

    #region Instance Members

    public bool IsApproved() => _Status == MerchantRegistrationStatuses.Approved;

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public static MerchantRegistration CreateNewMerchantRegistration(
        IUserRegistrationRepository userRegistrationRepository, CreateMerchantRegistrationCommand command)
    {
        UserRegistration.UserRegistration? userRegistration = userRegistrationRepository.GetById(new SimpleStringId(command.UserId))
                                                              ?? throw new NotFoundException(typeof(UserRegistration.UserRegistration));

        MerchantRegistration registration = new(userRegistration.GetMerchantId(), new Name(command.CompanyName),
            MerchantRegistrationStatuses.WaitingForRiskAnalysis, DateTimeUtc.Now) {_Status = MerchantRegistrationStatuses.WaitingForRiskAnalysis};

        registration.Publish(new MerchantRegistrationHasBeenCreated(registration));

        return registration;
    }

    public override SimpleStringId GetId() => Id;

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
    public SimpleStringId CreateMerchant() => throw new NotImplementedException();

    //Enforce(new MerchantRegistrationMustNotExpire(_Status, _RegistrationDate), () => _Status = MerchantRegistrationStatuses.Expired);
    //Enforce(new MerchantRegistrationCannotBeCreatedWithoutApproval(_Status));
    //if (_CompanyName is null)
    //    throw new CommandOutOfSyncException($"The {nameof(Name)} of the Merchant is required but could not be found");
    //if (_Address is null)
    //    throw new CommandOutOfSyncException($"The {nameof(Address)} of the Merchant is required but could not be found");
    //if (_BusinessInfo is null)
    //    throw new CommandOutOfSyncException($"The {nameof(BusinessInfo)} of the Merchant is required but could not be found");
    //Merchant merchant = new(Id, _CompanyName, _Address, _BusinessInfo, true);
    //Publish(new MerchantHasBeenCreated(merchant));
    //return merchant;
    public override MerchantRegistrationDto AsDto() =>
        new()
        {
            Id = Id,
            AddressDto = _Address?.AsDto() ?? new AddressDto(),
            BusinessInfo = _BusinessInfo?.AsDto() ?? new BusinessInfoDto(),
            CompanyName = _CompanyName!.Value,
            RegisteredDate = _RegistrationDate,
            RegistrationStatus = _Status
        };

    #endregion
}