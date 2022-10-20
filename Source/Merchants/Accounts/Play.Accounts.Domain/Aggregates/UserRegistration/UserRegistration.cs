using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.Services;
using Play.Core;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Accounts.Contracts.Commands;
using Play.Accounts.Domain.ValueObjects;
using Play.Core.Exceptions;
using Play.Randoms;
using Play.Domain.Exceptions;

namespace Play.Accounts.Domain.Aggregates;

public class UserRegistration : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;
    private readonly string _Username;
    private readonly string _HashedHashedPassword;
    private readonly DateTimeUtc _RegistrationDate;

    private Address? _Address;
    private Contact? _Contact;
    private PersonalDetail? _PersonalDetail;
    private UserRegistrationStatus _Status;

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
        _HashedHashedPassword = hashedPassword;
        _RegistrationDate = DateTimeUtc.Now;
        _Status = UserRegistrationStatuses.WaitingForEmailVerification;
    }

    #endregion

    #region Instance Members

    public string GetEmail()
    {
        return _Username;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public static UserRegistration CreateNewUserRegistration(
        IEnsureUniqueEmails uniqueEmailChecker, IHashPasswords passwordHasher, CreateUserRegistrationCommand command)
    {
        var userRegistration = new UserRegistration(GenerateSimpleStringId(), command.Email, passwordHasher.GeneratePasswordHash(command.Password));

        userRegistration.Enforce(new UsernameMustBeAValidEmail(command.Email));
        userRegistration.Enforce(new UsernameMustBeUnique(uniqueEmailChecker, command.Email));
        userRegistration.Enforce(new UserPasswordMustBeStrong(command.Password));

        userRegistration.Publish(new UserRegistrationCreated(userRegistration.GetId(), command.Email));

        return userRegistration;
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    public async Task<Result> SendEmailAccountVerificationCode(IVerifyEmailAccounts emailAccountVerifier)
    {
        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        if (_Status.Value == UserRegistrationStatuses.Rejected)
            return new Result($"The user can not register because they have previously been rejected");

        if (_Contact is null)
            throw new CommandOutOfSyncException($"The {nameof(Contact)} is required but could not be found");

        _EmailConfirmation = new ConfirmationCode(GenerateSimpleStringId(), DateTimeUtc.Now, Randomize.Integers.UInt(100000, 999999));

        Result result = await emailAccountVerifier.SendVerificationCode(_EmailConfirmation.Code, _Contact!.Email.Value).ConfigureAwait(false);

        if (!result.Succeeded)
        {
            _SmsConfirmation = null;
            Publish(new EmailVerificationCodeFailedToSend(_Id));

            return result;
        }

        _Status = UserRegistrationStatuses.WaitingForSmsVerification;

        return new Result();
    }

    /// <exception cref="ValueObjectException"></exception>
    public Result VerifyEmail(VerifyConfirmationCodeCommand command)
    {
        if (_EmailConfirmation is null)
            throw new CommandOutOfSyncException($"The email {nameof(ConfirmationCode)} is required but could not be found");

        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        ConfirmationCode confirmationCode = new ConfirmationCode(_EmailConfirmation.AsDto());
        _EmailConfirmation = null;

        Result<IBusinessRule> emailConfirmationCodeMustNotBeExpired = GetEnforcementResult(new EmailConfirmationCodeMustNotExpire(confirmationCode));
        Result<IBusinessRule> emailConfirmationCodeMustBeVerified =
            GetEnforcementResult(new EmailConfirmationCodeMustBeCorrect(confirmationCode!, command.ConfirmationCode));

        if (!emailConfirmationCodeMustNotBeExpired.Succeeded)
            return emailConfirmationCodeMustNotBeExpired;

        if (!emailConfirmationCodeMustBeVerified.Succeeded)
            return emailConfirmationCodeMustBeVerified;

        _Status = UserRegistrationStatuses.WaitingForSmsVerification;

        // Todo: Publish Domain Event so that we persist the status update

        return new Result();
    }

    /// <exception cref="ValueObjectException"></exception>
    public Result UpdateUserRegistrationDetails(UpdateUserRegistrationDetailsCommand command)
    {
        if (_Status.Value == UserRegistrationStatuses.Rejected)
            return new Result($"The user can not register because they have previously been rejected");
        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        try
        {
            command.AddressDto.Id = GenerateSimpleStringId();
            command.ContactDto.Id = GenerateSimpleStringId();
            command.PersonalInfo.Id = GenerateSimpleStringId();

            _PersonalDetail = new PersonalDetail(command.PersonalInfo);
            _Address = new Address(command.AddressDto);
            _Contact = new Contact(command.ContactDto);
        }
        catch (ValueObjectException e)
        {
            return new Result(e.Message);
        }

        return new Result();
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    public async Task<Result> SendSmsVerificationCode(IVerifyMobilePhones mobilePhoneVerifier)
    {
        if (_Status.Value == UserRegistrationStatuses.Rejected)
            return new Result($"The user can not register because they have previously been rejected");
        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        if (_Contact is null)
            throw new CommandOutOfSyncException($"The {nameof(Contact)} is required but could not be found");

        _SmsConfirmation = new ConfirmationCode(GenerateSimpleStringId(), DateTimeUtc.Now, Randomize.Integers.UInt(100000, 999999));
        Result result = await mobilePhoneVerifier.SendVerificationCode(_SmsConfirmation.Code, _Contact!.Phone.Value).ConfigureAwait(false);

        if (!result.Succeeded)
        {
            _SmsConfirmation = null;

            Publish(new SmsVerificationCodeFailedToSend(_Id));

            return result;
        }

        _Status = UserRegistrationStatuses.WaitingForSmsVerification;

        return new Result();
    }

    /// <exception cref="ValueObjectException"></exception>
    public Result VerifyMobilePhone(VerifyConfirmationCodeCommand command)
    {
        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        if (_SmsConfirmation is null)
            throw new CommandOutOfSyncException($"The SMS {nameof(ConfirmationCode)} is required but could not be found");

        ConfirmationCode confirmationCode = new ConfirmationCode(_SmsConfirmation.AsDto());
        _EmailConfirmation = null;

        Result<IBusinessRule> smsConfirmationCodeMustNotBeExpired = GetEnforcementResult(new SmsConfirmationCodeMustNotBeExpired(confirmationCode));
        Result<IBusinessRule> smsConfirmationCodeMustBeVerified =
            GetEnforcementResult(new SmsConfirmationCodeMustBeCorrect(confirmationCode!, command.ConfirmationCode));

        if (!smsConfirmationCodeMustNotBeExpired.Succeeded)
            return smsConfirmationCodeMustNotBeExpired;

        if (!smsConfirmationCodeMustBeVerified.Succeeded)
            return smsConfirmationCodeMustBeVerified;

        _Status = UserRegistrationStatuses.WaitingForRiskAnalysis;

        return new Result();
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    public Result AnalyzeUserRisk(IUnderwriteMerchants merchantUnderwriter)
    {
        if (_Status.Value == UserRegistrationStatuses.Rejected)
            return new Result($"The user can not register because they have previously been rejected");
        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        if (_PersonalDetail is null)
            throw new CommandOutOfSyncException($"The {nameof(PersonalDetail)} is required but could not be found");
        if (_Address is null)
            throw new CommandOutOfSyncException($"The {nameof(Address)} is required but could not be found");
        if (_Contact is null)
            throw new CommandOutOfSyncException($"The {nameof(Contact)} is required but could not be found");

        Result<IBusinessRule> result =
            GetEnforcementResult(new UserRegistrationMustNotBeProhibited(merchantUnderwriter, _PersonalDetail!, _Address!, _Contact!));

        if (!result.Succeeded)
        {
            _Status = UserRegistrationStatuses.Rejected;
            Publish(((UserRegistrationMustNotBeProhibited) result.Value).CreateBusinessRuleViolationDomainEvent(this));

            return result;
        }

        _Status = UserRegistrationStatuses.Approved;
        Publish(new UserRegistrationRiskAnalysisApproved(_Id, _Username));

        return result;
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

        var user = new User(_Id, GenerateSimpleStringId(), GenerateSimpleStringId(), _HashedHashedPassword!, _Address!, _Contact!, _PersonalDetail!, true);

        // TODO: Handle this domain event and persist the new user that has been created
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

    private bool IsUserRegistrationExpired()
    {
        Result<IBusinessRule> businessRule = GetEnforcementResult(new UserRegistrationMustNotExpire(_Status, _RegistrationDate));

        if (!businessRule.Succeeded)
        {
            _Status = UserRegistrationStatuses.Expired;

            return true;
        }

        return false;
    }

    #endregion
}