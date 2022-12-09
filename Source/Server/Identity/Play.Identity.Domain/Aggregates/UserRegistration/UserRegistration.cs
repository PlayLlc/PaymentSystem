﻿using Play.Core;
using Play.Core.Exceptions;
using Play.Domain.Aggregates;
using Play.Domain.Common.Dtos;
using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Identity.Contracts.Commands;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.Aggregates.Rules;
using Play.Identity.Domain.Entities;
using Play.Identity.Domain.Enums;
using Play.Identity.Domain.Services;
using Play.Identity.Domain.ValueObjects;
using Play.Randoms;

namespace Play.Identity.Domain.Aggregates;

public class UserRegistration : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly SimpleStringId _MerchantId;
    private readonly string _Username;
    private readonly string _HashedPassword;
    private readonly DateTimeUtc _RegistrationDate;

    private bool _HasEmailBeenVerified;
    private bool _HasPhoneBeenVerified;
    private UserRegistrationStatus _Status;

    private Address? _Address;
    private Contact? _Contact;
    private PersonalDetail? _PersonalDetail;
    private ConfirmationCode? _EmailConfirmation;
    private ConfirmationCode? _SmsConfirmation;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private UserRegistration()
    { }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    private UserRegistration(CreateUserRegistrationCommand command, IHashPasswords passwordHasher)
    {
        Id = new(GenerateSimpleStringId());
        _MerchantId = new(GenerateSimpleStringId());
        _Username = command.Email;
        _HashedPassword = passwordHasher.GeneratePasswordHash(command.Password);
        _RegistrationDate = DateTimeUtc.Now;
        _Status = UserRegistrationStatuses.WaitingForEmailVerification;
    }

    #endregion

    #region Instance Members

    public string GetMerchantId() => _MerchantId;

    public string GetEmail() => _Username;

    public bool HasEmailBeenVerified() => _HasEmailBeenVerified;

    public bool HasPhoneBeenVerified() => _HasPhoneBeenVerified;

    public bool IsApproved() => _Status == UserRegistrationStatuses.Approved;

    /// <exception cref="AggregateException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public static UserRegistration CreateNewUserRegistration(
        IEnsureUniqueEmails uniqueEmailChecker, IHashPasswords passwordHasher, CreateUserRegistrationCommand command)
    {
        UserRegistration userRegistration = new(command, passwordHasher);

        userRegistration.Enforce(new UserRegistrationUsernameMustBeAValidEmail(command.Email));
        userRegistration.Enforce(new UserRegistrationUsernameMustBeUnique(uniqueEmailChecker, command.Email));
        userRegistration.Enforce(new UserPasswordMustBeStrong(command.Password));
        userRegistration.Publish(new UserRegistrationCreated(userRegistration));

        return userRegistration;
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task<Result> SendEmailVerificationCode(IVerifyEmailAccounts emailAccountVerifier)
    {
        Enforce(new UserRegistrationMustNotExpire(_Status, _RegistrationDate), () => _Status = UserRegistrationStatuses.Expired);
        Enforce(new UserRegistrationMustNotBeRejected(_Status), () => _Status = UserRegistrationStatuses.Rejected);

        if (_Contact is null)
            throw new CommandOutOfSyncException($"The {nameof(Contact)} is required but could not be found");

        _EmailConfirmation = new(GenerateSimpleStringId(), DateTimeUtc.Now, Randomize.Integers.UInt(100000, 999999));

        Result result = await emailAccountVerifier.SendVerificationCode(_EmailConfirmation.Code, _Contact!.Email.Value, _Contact.GetFullName())
            .ConfigureAwait(false);

        if (!result.Succeeded)
        {
            _SmsConfirmation = null;
            Publish(new EmailVerificationCodeFailedToSend(this));

            return result;
        }

        _Status = UserRegistrationStatuses.WaitingForSmsVerification;
        Publish(new EmailVerificationCodeHasBeenSent(this));

        return new();
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public void VerifyEmail(VerifyConfirmationCodeCommand command)
    {
        Enforce(new UserRegistrationMustNotExpire(_Status, _RegistrationDate), () => _Status = UserRegistrationStatuses.Expired);
        Enforce(new UserRegistrationMustNotBeRejected(_Status), () => _Status = UserRegistrationStatuses.Rejected);

        if (_EmailConfirmation is null)
            throw new CommandOutOfSyncException($"The email {nameof(ConfirmationCode)} is required but could not be found");

        ConfirmationCode confirmationCode = new(_EmailConfirmation.AsDto());
        _EmailConfirmation = null;

        Enforce(new EmailVerificationCodeMustNotExpire(confirmationCode)); // TODO: If expired - send another in domain event handler
        Enforce(new EmailVerificationCodeMustBeCorrect(confirmationCode!, command.ConfirmationCode));

        _Status = UserRegistrationStatuses.WaitingForSmsVerification;
        _HasEmailBeenVerified = true;
        Publish(new EmailVerificationWasSuccessful(this));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public void UpdateContactInfo(UpdateContactCommand contact)
    {
        Enforce(new UserRegistrationMustNotExpire(_Status, _RegistrationDate), () => _Status = UserRegistrationStatuses.Expired);
        Enforce(new UserRegistrationMustNotBeRejected(_Status), () => _Status = UserRegistrationStatuses.Rejected);

        contact.Contact.Id = GenerateSimpleStringId();
        _Contact = new(contact.Contact);
        Publish(new UserRegistrationContactInfoUpdated(this));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task SendSmsVerificationCode(IVerifyMobilePhones mobilePhoneVerifier)
    {
        Enforce(new UserRegistrationMustNotExpire(_Status, _RegistrationDate), () => _Status = UserRegistrationStatuses.Expired);
        Enforce(new UserRegistrationMustNotBeRejected(_Status), () => _Status = UserRegistrationStatuses.Rejected);

        if (_Contact is null)
            throw new CommandOutOfSyncException($"The {nameof(Contact)} is required but could not be found");

        _SmsConfirmation = new(GenerateSimpleStringId(), DateTimeUtc.Now, Randomize.Integers.UInt(100000, 999999));
        Result result = await mobilePhoneVerifier.SendVerificationCode(_SmsConfirmation.Code, _Contact!.Phone.Value).ConfigureAwait(false);

        if (!result.Succeeded)
        {
            _SmsConfirmation = null;
            Publish(new SmsVerificationCodeFailedToSend(this));
        }

        _Status = UserRegistrationStatuses.WaitingForSmsVerification;
        Publish(new SmsVerificationCodeHasBeenSent(this));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public void VerifyMobilePhone(VerifyConfirmationCodeCommand command)
    {
        Enforce(new UserRegistrationMustNotExpire(_Status, _RegistrationDate), () => _Status = UserRegistrationStatuses.Expired);
        Enforce(new UserRegistrationMustNotBeRejected(_Status), () => _Status = UserRegistrationStatuses.Rejected);

        if (_SmsConfirmation is null)
            throw new CommandOutOfSyncException($"The SMS {nameof(ConfirmationCode)} is required but could not be found");

        ConfirmationCode confirmationCode = new(_SmsConfirmation.AsDto());
        _EmailConfirmation = null;

        Enforce(new SmsVerificationCodeMustNotBeExpired(confirmationCode)); // TODO: If expired - send another in domain event handler 
        Enforce(new SmsVerificationCodeMustBeCorrect(confirmationCode, command.ConfirmationCode));

        _HasPhoneBeenVerified = true;
        Publish(new UserRegistrationPhoneVerified(this));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public void UpdateUserAddress(UpdateAddressCommand command)
    {
        Enforce(new UserRegistrationMustNotExpire(_Status, _RegistrationDate), () => _Status = UserRegistrationStatuses.Expired);
        Enforce(new UserRegistrationMustNotBeRejected(_Status), () => _Status = UserRegistrationStatuses.Rejected);

        command.Address.Id = GenerateSimpleStringId();
        _Address = new(command.Address);

        if (_Address is not null && _PersonalDetail is not null && _Contact is not null)
            _Status = UserRegistrationStatuses.WaitingForRiskAnalysis;

        Publish(new UserRegistrationAddressUpdated(this));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public void UpdatePersonalDetails(UpdatePersonalDetailCommand command)
    {
        Enforce(new UserRegistrationMustNotExpire(_Status, _RegistrationDate), () => _Status = UserRegistrationStatuses.Expired);
        Enforce(new UserRegistrationMustNotBeRejected(_Status), () => _Status = UserRegistrationStatuses.Rejected);

        command.PersonalDetail.Id = GenerateSimpleStringId();
        _PersonalDetail = new(command.PersonalDetail);

        if (_Address is not null && _PersonalDetail is not null && _Contact is not null)
            _Status = UserRegistrationStatuses.WaitingForRiskAnalysis;

        Publish(new UserRegistrationAddressUpdated(this));
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public void AnalyzeUserRisk(IUnderwriteMerchants merchantUnderwriter)
    {
        Enforce(new UserRegistrationMustNotExpire(_Status, _RegistrationDate), () => _Status = UserRegistrationStatuses.Expired);
        Enforce(new UserRegistrationMustNotBeRejected(_Status), () => _Status = UserRegistrationStatuses.Rejected);

        if (_PersonalDetail is null)
            throw new CommandOutOfSyncException($"The {nameof(PersonalDetail)} is required but could not be found");
        if (_Address is null)
            throw new CommandOutOfSyncException($"The {nameof(Address)} is required but could not be found");
        if (_Contact is null)
            throw new CommandOutOfSyncException($"The {nameof(Contact)} is required but could not be found");

        Enforce(new UserRegistrationMustNotBeProhibited(merchantUnderwriter, _PersonalDetail!, _Address!, _Contact!),
            () => _Status = UserRegistrationStatuses.Rejected);

        _Status = UserRegistrationStatuses.Approved;
        Publish(new UserRegistrationHasBeenApproved(this));
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    public User CreateUser()
    {
        Enforce(new UserCannotBeCreatedWithoutApproval(_Status));

        if (_PersonalDetail is null)
            throw new CommandOutOfSyncException($"The {nameof(PersonalDetail)} is required but could not be found");
        if (_Address is null)
            throw new CommandOutOfSyncException($"The {nameof(Address)} is required but could not be found");
        if (_Contact is null)
            throw new CommandOutOfSyncException($"The {nameof(Contact)} is required but could not be found");

        User user = new(Id, GenerateSimpleStringId(), GenerateSimpleStringId(), new(Id, _HashedPassword, DateTimeUtc.Now), _Address!, _Contact!,
            _PersonalDetail!, true);

        Publish(new UserHasBeenCreated(user));

        return user;
    }

    public override SimpleStringId GetId() => Id;

    /// <exception cref="PlayInternalException"></exception>
    public override UserRegistrationDto AsDto() =>
        new()
        {
            Id = Id,
            MerchantId = _MerchantId,
            Address = _Address?.AsDto(),
            ContactInfo = _Contact?.AsDto() ?? new ContactDto(),
            PersonalInfo = _PersonalDetail?.AsDto(),
            RegisteredDate = _RegistrationDate!,
            RegistrationStatus = _Status
        };

    #endregion
}