using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.Services;
using Play.Core;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

using System.Net.NetworkInformation;

using Play.Accounts.Contracts.Commands;
using Play.Accounts.Domain.ValueObjects;
using Play.Core.Exceptions;
using Play.Randoms;

namespace Play.Accounts.Domain.Aggregates;

public class UserRegistration : Aggregate<string>
{
    #region Static Metadata

    private static readonly TimeSpan _ValidityPeriod = new(7, 0, 0, 0);

    #endregion

    #region Instance Values

    private readonly string _Id;
    private readonly string _Username;
    private readonly string _HashedPassword;
    private readonly DateTimeUtc _RegistrationDate;

    private Address? _Address;
    private Contact? _Contact;
    private PersonalDetail? _PersonalDetail;
    private RegistrationStatus _Status;

    private ConfirmationCode? _EmailConfirmation;
    private ConfirmationCode? _SmsConfirmation;

    #endregion

    #region Constructor

    private UserRegistration(
        string id, string username, string hashedPassword, DateTimeUtc registrationDate, Address address, Contact contact, PersonalDetail personalDetail,
        RegistrationStatus status, ConfirmationCode? emailConfirmation = null, ConfirmationCode? smsConfirmation = null)
    {
        _Id = id;
        _Username = username;
        _HashedPassword = hashedPassword;
        _RegistrationDate = registrationDate;
        _Address = address;
        _Contact = contact;
        _PersonalDetail = personalDetail;
        _Status = status;
        _EmailConfirmation = emailConfirmation;
        _SmsConfirmation = smsConfirmation;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    private UserRegistration(IEnsureUniqueEmails uniqueEmailChecker, IHashPasswords passwordHasher, string username, string password)
    {
        Enforce(new UsernameMustBeAValidEmail(username));
        Enforce(new UsernameMustBeUnique(uniqueEmailChecker, username));
        Enforce(new PasswordMustBeStrong(password));
        _Id = GenerateSimpleStringId();
        _Username = username;
        _HashedPassword = passwordHasher.GeneratePasswordHash(password);
        _RegistrationDate = DateTimeUtc.Now;
        _Status = new RegistrationStatus(RegistrationStatuses.WaitingForEmailVerification);
        Publish(new UserRegistrationCreated(_Id, _HashedPassword));
    }

    #endregion

    #region Instance Members

    public string GetEmail()
    {
        return _Username;
    }

    /// <exception cref="ValueObjectException"></exception>
    private bool IsUserRegistrationExpired()
    {
        if (_Status == RegistrationStatuses.Expired)
            return true;

        if ((DateTimeUtc.Now - _RegistrationDate) > _ValidityPeriod)
        {
            _Status = new RegistrationStatus(RegistrationStatuses.Expired);
            Publish(new UserRegistrationHasExpired(_Id));

            return true;
        }

        return false;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public static UserRegistration CreateNewUserRegistration(
        IEnsureUniqueEmails uniqueEmailChecker, IHashPasswords passwordHasher, CreateUserRegistrationCommand command)
    {
        return new UserRegistration(uniqueEmailChecker, passwordHasher, command.Email, command.Password);
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<Result> SendEmailAccountVerificationCode(IVerifyEmailAccounts emailAccountVerifier)
    {
        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        if (_Status.Value == RegistrationStatuses.Rejected)
            return new Result($"The user can not register because they have previously been rejected");

        if (_Contact is null)
            throw new InvalidOperationException();

        _EmailConfirmation = new ConfirmationCode(GenerateSimpleStringId(), DateTimeUtc.Now, Randomize.Integers.UInt(100000, 999999));

        Result result = await emailAccountVerifier.SendVerificationCode(_EmailConfirmation.Code, _Contact!.Email.Value).ConfigureAwait(false);

        if (!result.Succeeded)
        {
            _SmsConfirmation = null;
            Publish(new EmailVerificationCodeFailedToSend(_Id));

            return result;
        }

        _Status = new RegistrationStatus(RegistrationStatuses.WaitingForSmsVerification);

        return new Result();
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public Result VerifyEmail(VerifyUserRegistrationConfirmationCodeCommand command)
    {
        if (_EmailConfirmation is null)
            throw new InvalidOperationException();

        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        ConfirmationCode confirmationCode = new ConfirmationCode(_EmailConfirmation.AsDto());
        _EmailConfirmation = null;

        Result<IBusinessRule> emailConfirmationCodeMustNotBeExpired = GetEnforcementResult(new EmailConfirmationCodeMustNotBeExpired(confirmationCode));

        if (!emailConfirmationCodeMustNotBeExpired.Succeeded)
            return emailConfirmationCodeMustNotBeExpired;

        var emailConfirmationCodeMustBeVerified = GetEnforcementResult(new EmailConfirmationCodeMustBeVerified(confirmationCode!, command.ConfirmationCode));

        if (!emailConfirmationCodeMustBeVerified.Succeeded)
            return emailConfirmationCodeMustBeVerified;

        _Status = new RegistrationStatus(RegistrationStatuses.WaitingForSmsVerification);

        return new Result();
    }

    /// <exception cref="ValueObjectException"></exception>
    public Result UpdateUserRegistrationDetails(UpdateUserRegistrationDetailsCommand command)
    {
        if (_Status.Value == RegistrationStatuses.Rejected)
            return new Result($"The user can not register because they have previously been rejected");
        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        try
        {
            command.AddressDto.Id = GenerateSimpleStringId();
            command.ContactInfoDto.Id = GenerateSimpleStringId();
            command.PersonalInfo.Id = GenerateSimpleStringId();

            _PersonalDetail = new PersonalDetail(command.PersonalInfo);
            _Address = new Address(command.AddressDto);
            _Contact = new Contact(command.ContactInfoDto);
        }
        catch (ValueObjectException e)
        {
            return new Result(e.Message);
        }

        return new Result();
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<Result> SendSmsVerificationCode(IVerifyMobilePhones mobilePhoneVerifier)
    {
        if (_Status.Value == RegistrationStatuses.Rejected)
            return new Result($"The user can not register because they have previously been rejected");
        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        if (_Contact is null)
            throw new InvalidOperationException();

        _SmsConfirmation = new ConfirmationCode(GenerateSimpleStringId(), DateTimeUtc.Now, Randomize.Integers.UInt(100000, 999999));

        Result result = await mobilePhoneVerifier.SendVerificationCode(_SmsConfirmation.Code, _Contact!.Phone.Value).ConfigureAwait(false);

        if (!result.Succeeded)
        {
            _SmsConfirmation = null;

            Publish(new SmsVerificationCodeFailedToSend(_Id));

            return result;
        }

        _Status = new RegistrationStatus(RegistrationStatuses.WaitingForSmsVerification);

        return new Result();
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public Result VerifyMobilePhone(VerifyUserRegistrationConfirmationCodeCommand command)
    {
        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        if (_SmsConfirmation is null)
            throw new InvalidOperationException();

        ConfirmationCode confirmationCode = new ConfirmationCode(_SmsConfirmation.AsDto());
        _EmailConfirmation = null;

        Result<IBusinessRule> smsConfirmationCodeMustNotBeExpired = GetEnforcementResult(new SmsConfirmationCodeMustNotBeExpired(confirmationCode));

        if (!smsConfirmationCodeMustNotBeExpired.Succeeded)
            return smsConfirmationCodeMustNotBeExpired;

        var smsConfirmationCodeMustBeVerified = GetEnforcementResult(new SmsConfirmationCodeMustBeVerified(confirmationCode!, command.ConfirmationCode));

        if (!smsConfirmationCodeMustBeVerified.Succeeded)
            return smsConfirmationCodeMustBeVerified;

        _Status = new RegistrationStatus(RegistrationStatuses.WaitingForRiskAnalysis);

        return new Result();
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public Result AnalyzeUserRisk(IUnderwriteMerchants merchantUnderwriter)
    {
        if (_Status.Value == RegistrationStatuses.Rejected)
            return new Result($"The user can not register because they have previously been rejected");
        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        if (_PersonalDetail is null)
            throw new InvalidOperationException();
        if (_Address is null)
            throw new InvalidOperationException();
        if (_Contact is null)
            throw new InvalidOperationException();

        Result<IBusinessRule> result =
            GetEnforcementResult(new UserMustNotBeProhibitedFromRegistering(merchantUnderwriter, _PersonalDetail!, _Address!, _Contact!));

        if (!result.Succeeded)
        {
            _Status = new RegistrationStatus(RegistrationStatuses.Rejected);
            Publish(((UserMustNotBeProhibitedFromRegistering) result.Value).CreateBusinessRuleViolationDomainEvent(this));

            return result;
        }

        _Status = new RegistrationStatus(RegistrationStatuses.Approved);
        Publish(new UserRegistrationRiskAnalysisApproved(_Id, _Username));

        return result;
    }

    /// <exception cref="InvalidOperationException"></exception>
    public Result<User?> CreateUser()
    {
        if (_Status != RegistrationStatuses.Approved)
            return new Result<User?>(null, $"The user can't be created because registration has not yet been approved");

        if (_PersonalDetail is null)
            throw new InvalidOperationException();
        if (_Address is null)
            throw new InvalidOperationException();
        if (_Contact is null)
            throw new InvalidOperationException();

        return new Result<User?>(new User(_Id, _HashedPassword!, _Address!, _Contact!, _PersonalDetail!, true));
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
            ContactInfo = _Contact?.AsDto() ?? new ContactInfoDto(),
            PersonalInfo = _PersonalDetail?.AsDto(),
            RegisteredDate = _RegistrationDate!,
            RegistrationStatus = _Status
        };
    }

    #endregion
}