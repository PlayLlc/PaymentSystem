using Play.Accounts.Contracts.Commands.UserRegistration;
using Play.Accounts.Contracts.Common;
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
    private readonly string? _Password;
    private readonly DateTimeUtc _RegisteredDate;

    private Address? _Address;
    private ContactInfo? _ContactInfo;
    private PersonalInfo? _PersonalInfo;
    private RegistrationStatus _Status;

    private ConfirmationCode? _EmailConfirmation;
    private ConfirmationCode? _SmsConfirmation;

    #endregion

    #region Constructor

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    private UserRegistration(IEnsureUniqueEmails uniqueEmailChecker, string username, string password)
    {
        Enforce(new UsernameMustBeAValidEmail(username));
        Enforce(new UsernameMustBeUnique(uniqueEmailChecker, username));
        Enforce(new PasswordMustBeStrong(password));
        _Id = GenerateSimpleStringId();
        _Username = username;
        _Password = password;
        _RegisteredDate = DateTimeUtc.Now;
        _Status = new RegistrationStatus(RegistrationStatuses.WaitingForEmailVerification);
        Publish(new UserRegistrationCreated(_Id, _Password));
    }

    #endregion

    #region Instance Members

    /// <exception cref="ValueObjectException"></exception>
    private bool IsUserRegistrationExpired()
    {
        if (_Status == RegistrationStatuses.Expired)
            return true;

        if ((DateTimeUtc.Now - _RegisteredDate) > _ValidityPeriod)
        {
            _Status = new RegistrationStatus(RegistrationStatuses.Expired);
            Publish(new UserRegistrationHasExpired(_Id));

            return true;
        }

        return false;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public static UserRegistration CreateNewUserRegistration(IEnsureUniqueEmails uniqueEmailChecker, CreateUserRegistrationCommand command)
    {
        return new UserRegistration(uniqueEmailChecker, command.Email, command.Password);
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<Result> SendEmailAccountVerificationCode(IVerifyEmailAccounts emailAccountVerifier)
    {
        if (IsUserRegistrationExpired())
            return new Result($"The user registration has expired");

        if (_Status.Value == RegistrationStatuses.Rejected)
            return new Result($"The user can not register because they have previously been rejected");

        if (_ContactInfo is null)
            throw new InvalidOperationException();

        _EmailConfirmation = new ConfirmationCode(GenerateSimpleStringId(), DateTimeUtc.Now, Randomize.Integers.UInt(100000, 999999));

        Result result = await emailAccountVerifier.SendVerificationCode(_EmailConfirmation.Code, _ContactInfo!.Email.Value).ConfigureAwait(false);

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

            _PersonalInfo = new PersonalInfo(command.PersonalInfo);
            _Address = new Address(command.AddressDto);
            _ContactInfo = new ContactInfo(command.ContactInfoDto);
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

        if (_ContactInfo is null)
            throw new InvalidOperationException();

        _SmsConfirmation = new ConfirmationCode(GenerateSimpleStringId(), DateTimeUtc.Now, Randomize.Integers.UInt(100000, 999999));

        Result result = await mobilePhoneVerifier.SendVerificationCode(_SmsConfirmation.Code, _ContactInfo!.Phone.Value).ConfigureAwait(false);

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

        if (_PersonalInfo is null)
            throw new InvalidOperationException();
        if (_Address is null)
            throw new InvalidOperationException();
        if (_ContactInfo is null)
            throw new InvalidOperationException();

        Result<IBusinessRule> result =
            GetEnforcementResult(new UserMustNotBeProhibitedFromRegistering(merchantUnderwriter, _PersonalInfo!, _Address!, _ContactInfo!));

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

        if (_PersonalInfo is null)
            throw new InvalidOperationException();
        if (_Address is null)
            throw new InvalidOperationException();
        if (_ContactInfo is null)
            throw new InvalidOperationException();

        return new Result<User?>(new User(_Id, _Address!, _ContactInfo!, _PersonalInfo!, true));
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
            ContactInfo = _ContactInfo?.AsDto() ?? new ContactInfoDto(),
            PersonalInfo = _PersonalInfo?.AsDto(),
            RegisteredDate = _RegisteredDate!,
            RegistrationStatus = _Status
        };
    }

    #endregion
}