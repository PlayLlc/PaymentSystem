using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.Services;
using Play.Core;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Accounts.Domain.ValueObjects;
using Play.Core.Exceptions;
using Play.Randoms;
using Play.Domain.Exceptions;
using Play.Accounts.Contracts.Commands.User;
using Play.Accounts.Contracts.Commands;

namespace Play.Accounts.Domain.Aggregates;

public class UserRegistration : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;
    private readonly string _Username;
    private readonly string _HashedPassword;
    private readonly DateTimeUtc _RegistrationDate;

    private bool _HasEmailBeenVerified = false;
    private bool _HasPhoneBeenVerified = false;
    private UserRegistrationStatus _Status;

    private Address? _Address;
    private Contact? _Contact;
    private PersonalDetail? _PersonalDetail;
    private ConfirmationCode? _EmailConfirmation;
    private ConfirmationCode? _SmsConfirmation;

    #endregion

    #region Constructor

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    private UserRegistration(string id, string username, string hashedPassword)
    {
        _Id = id;
        _Username = username;
        _HashedPassword = hashedPassword;
        _RegistrationDate = DateTimeUtc.Now;
        _Status = UserRegistrationStatuses.WaitingForEmailVerification;
    }

    #endregion

    #region Instance Members

    public string GetEmail()
    {
        return _Username;
    }

    public bool HasEmailBeenVerified()
    {
        return _HasEmailBeenVerified;
    }

    public bool HasPhoneBeenVerified()
    {
        return _HasPhoneBeenVerified;
    }

    public bool IsApproved()
    {
        return _Status == UserRegistrationStatuses.Approved;
    }

    /// <exception cref="AggregateException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public static UserRegistration CreateNewUserRegistration(
        IEnsureUniqueEmails uniqueEmailChecker, IHashPasswords passwordHasher, CreateUserRegistrationCommand command)
    {
        UserRegistration userRegistration =
            new UserRegistration(GenerateSimpleStringId(), command.Email, passwordHasher.GeneratePasswordHash(command.Password));

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

        _EmailConfirmation = new ConfirmationCode(GenerateSimpleStringId(), DateTimeUtc.Now, Randomize.Integers.UInt(100000, 999999));

        Result result = await emailAccountVerifier.SendVerificationCode(_EmailConfirmation.Code, _Contact!.Email.Value).ConfigureAwait(false);

        if (!result.Succeeded)
        {
            _SmsConfirmation = null;
            Publish(new EmailVerificationCodeFailedToSend(this));

            return result;
        }

        _Status = UserRegistrationStatuses.WaitingForSmsVerification;
        Publish(new EmailVerificationCodeHasBeenSent(this));

        return new Result();
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

        ConfirmationCode confirmationCode = new ConfirmationCode(_EmailConfirmation.AsDto());
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
        _Contact = new Contact(contact.Contact);
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

        _SmsConfirmation = new ConfirmationCode(GenerateSimpleStringId(), DateTimeUtc.Now, Randomize.Integers.UInt(100000, 999999));
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

        ConfirmationCode confirmationCode = new ConfirmationCode(_SmsConfirmation.AsDto());
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
        _Address = new Address(command.Address);

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
        _PersonalDetail = new PersonalDetail(command.PersonalDetail);

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
    public User CreateUser(IHashPasswords passwordHasher)
    {
        Enforce(new UserCannotBeCreatedWithoutApproval(_Status));

        if (_PersonalDetail is null)
            throw new CommandOutOfSyncException($"The {nameof(PersonalDetail)} is required but could not be found");
        if (_Address is null)
            throw new CommandOutOfSyncException($"The {nameof(Address)} is required but could not be found");
        if (_Contact is null)
            throw new CommandOutOfSyncException($"The {nameof(Contact)} is required but could not be found");

        User user = new User(_Id, GenerateSimpleStringId(), GenerateSimpleStringId(),
            new Password(_Id, passwordHasher.GeneratePasswordHash(_HashedPassword), DateTimeUtc.Now), _Address!, _Contact!, _PersonalDetail!, true);

        Publish(new UserHasBeenCreated(user));

        return user;
    }

    public override string GetId()
    {
        return _Id;
    }

    /// <exception cref="PlayInternalException"></exception>
    public override UserRegistrationDto AsDto()
    {
        return new UserRegistrationDto
        {
            Id = _Id,
            Address = _Address?.AsDto(),
            ContactInfo = _Contact?.AsDto() ?? new ContactDto(),
            PersonalInfo = _PersonalDetail?.AsDto(),
            RegisteredDate = _RegistrationDate!,
            RegistrationStatus = _Status
        };
    }

    #endregion
}