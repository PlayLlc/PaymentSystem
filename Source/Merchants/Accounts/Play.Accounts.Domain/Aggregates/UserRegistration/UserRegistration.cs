using Play.Accounts.Contracts.Commands.UserRegistration;
using Play.Accounts.Contracts.Common;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.ValueObjects;
using Play.Core;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

using System.Net.NetworkInformation;

using Play.Core.Exceptions;

namespace Play.Accounts.Domain.Aggregates;

public class UserRegistration : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;
    private readonly string _Username;
    private readonly string? _Password;
    private readonly DateTimeUtc _RegisteredDate;

    private Address? _Address;
    private ContactInfo? _ContactInfo;
    private PersonalInfo? _PersonalInfo;
    private RegistrationStatus _Status;

    private uint? _EmailConfirmationCode;
    private uint? _SmsConfirmationCode;

    private DateTimeUtc? _ConfirmedDate;

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

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public static UserRegistration CreateNewUserRegistration(IEnsureUniqueEmails uniqueEmailChecker, string username, string password)
    {
        return new UserRegistration(uniqueEmailChecker, username, password);
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task SendEmailAccountVerificationCode(IVerifyEmailAccounts emailAccountVerifier)
    {
        if (_ContactInfo is null)
            throw new InvalidOperationException();

        // TODO: We need to handle when the email client doesn't correctly send an email
        _EmailConfirmationCode = await emailAccountVerifier.SendVerificationCode(_ContactInfo!.Email).ConfigureAwait(false);
        _Status = new RegistrationStatus(RegistrationStatuses.WaitingForEmailVerification);
    }

    /// <exception cref="ValueObjectException"></exception>
    public bool ValidateEmailVerificationCode(ushort verificationCode)
    {
        // TODO: We need to make sure the verification code hasn't expired.  Create BusinessRules and the matching BusinessRuleViolationDomainEvent 

        bool isValid = _EmailConfirmationCode == verificationCode;
        _EmailConfirmationCode = null;

        if (isValid)
            _Status = new RegistrationStatus(RegistrationStatuses.WaitingForSmsVerification);

        return isValid;
    }

    public Result UpdateUserInfo(PersonalInfo personalInfo, Address address, ContactInfo contactInfo)
    {
        if (_Status.Value == RegistrationStatuses.Rejected)
            return new Result($"The user can not register because they have previously been rejected");

        _PersonalInfo = personalInfo;
        _Address = address;
        _ContactInfo = contactInfo;

        return new Result();
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task SendSmsVerificationCode(IVerifyMobilePhones mobilePhoneVerifier)
    {
        // TODO: We need to make sure the verification code hasn't expired

        if (_ContactInfo is null)
            throw new InvalidOperationException();

        // TODO: We need to handle when the email client doesn't correctly send an email
        _SmsConfirmationCode = await mobilePhoneVerifier.SendVerificationCode(_ContactInfo!.Phone).ConfigureAwait(false);
        _Status = new RegistrationStatus(RegistrationStatuses.WaitingForSmsVerification);
    }

    /// <exception cref="ValueObjectException"></exception>
    public bool ValidateSmsVerificationCode(ushort verificationCode)
    {
        // TODO: We need to make sure the verification code hasn't expired.  Create BusinessRules and the matching BusinessRuleViolationDomainEvent 
        bool isValid = _SmsConfirmationCode == verificationCode;
        _SmsConfirmationCode = null;

        if (isValid)
            _Status = new RegistrationStatus(RegistrationStatuses.WaitingForRiskAnalysis);

        return isValid;
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public Result AnalyzeUserRisk(IUnderwriteMerchants merchantUnderwriter)
    {
        if (_Status.Value == RegistrationStatuses.Rejected)
            return new Result($"The user can not register because they have previously been rejected");

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
            ConfirmedDate = _ConfirmedDate,
            RegisteredDate = _RegisteredDate!,
            RegistrationStatus = _Status
        };
    }

    #endregion
}