using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;
using Play.Core;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates;

public class User : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;

    private readonly string _MerchantId;
    private readonly string _TerminalId;
    private bool _IsActive;
    private PersonalDetail _PersonalDetail;
    private Address _Address;
    private Contact _Contact;
    private Password _Password;

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private User()
    { }

    /// <exception cref="ArgumentException"></exception>
    public User(UserDto dto)
    {
        if (dto.Id != dto.Password.Id)
            throw new ArgumentException($"The {nameof(Password)} ID must match the {nameof(User)} ID");

        _Id = dto.Id;
        _MerchantId = dto.MerchantId;
        _TerminalId = dto.TerminalId;
        _Password = new Password(dto.Password);
        _Address = new Address(dto.Address);
        _Contact = new Contact(dto.Contact);
        _PersonalDetail = new PersonalDetail(dto.PersonalDetail);
        _IsActive = dto.IsActive;
    }

    /// <exception cref="ArgumentException"></exception>
    public User(
        string id, string merchantId, string terminalId, Password password, Address address, Contact contact, PersonalDetail personalDetail, bool isActive)
    {
        if (password.Id != id)
            throw new ArgumentException($"The {nameof(Password)} ID must match the {nameof(User)} ID");

        _Id = id;
        _MerchantId = merchantId;
        _TerminalId = terminalId;
        _Password = password;
        _Address = address;
        _Contact = contact;
        _PersonalDetail = personalDetail;
        _IsActive = isActive;
    }

    #endregion

    #region Instance Members

    internal string GetHashedPassword()
    {
        return _Password.HashedPassword;
    }

    public Result LoginValidation(IHashPasswords passwordHasher, string clearTextPassword)
    {
        // TODO: User account must be temporarily locked after 6 login attempts for a minimum of 30..

        Result<IBusinessRule> userMustBeActive = GetEnforcementResult(new UserMustBeActive(_IsActive));

        if (!userMustBeActive.Succeeded)
            return new Result(userMustBeActive.Errors);

        Result<IBusinessRule> expiredPassword = GetEnforcementResult(new UserMustUpdatePasswordEvery90Days(_Password));

        if (!expiredPassword.Succeeded)
            return new Result(expiredPassword.Errors);

        Result<IBusinessRule> passwordValidation =
            GetEnforcementResult(new PasswordMustBeCorrectToLogin(passwordHasher, _Password.HashedPassword, clearTextPassword));

        if (!passwordValidation.Succeeded)
            return new Result(passwordValidation.Errors);

        return new Result();
    }

    /// <exception cref="AggregateException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public void UpdateAddress(IUnderwriteMerchants merchantUnderwriter, UpdateAddressCommand command)
    {
        Enforce(new UserMustBeActive(_IsActive));

        if (_PersonalDetail is null)
            throw new CommandOutOfSyncException($"The {nameof(PersonalDetail)} is required but could not be found");
        if (_Contact is null)
            throw new CommandOutOfSyncException($"The {nameof(Contact)} is required but could not be found");

        _Address = new Address(command.Address);
        Enforce(new UserMustNotBeProhibited(merchantUnderwriter, _PersonalDetail, _Address, _Contact), () => _IsActive = false);
        Publish(new UserAddressHasBeenUpdated(this));
    }

    /// <exception cref="AggregateException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public void UpdateContactInfo(IUnderwriteMerchants merchantUnderwriter, UpdateContactCommand command)
    {
        Enforce(new UserMustBeActive(_IsActive));

        if (_PersonalDetail is null)
            throw new CommandOutOfSyncException($"The {nameof(PersonalDetail)} is required but could not be found");
        if (_Address is null)
            throw new CommandOutOfSyncException($"The {nameof(Address)} is required but could not be found");

        _Contact = new Contact(command.Contact);
        Enforce(new UserMustNotBeProhibited(merchantUnderwriter, _PersonalDetail, _Address, _Contact), () => _IsActive = false);
        Publish(new UserContactInfoHasBeenUpdated(this));
    }

    /// <exception cref="AggregateException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public void UpdatePersonalDetails(IUnderwriteMerchants merchantUnderwriter, UpdatePersonalDetailCommand command)
    {
        Enforce(new UserMustBeActive(_IsActive));

        if (_Contact is null)
            throw new CommandOutOfSyncException($"The {nameof(Contact)} is required but could not be found");
        if (_Address is null)
            throw new CommandOutOfSyncException($"The {nameof(Address)} is required but could not be found");

        _PersonalDetail = new PersonalDetail(command.PersonalDetail);
        Enforce(new UserMustNotBeProhibited(merchantUnderwriter, _PersonalDetail, _Address, _Contact), () => _IsActive = false);
        Publish(new UserContactInfoHasBeenUpdated(this));
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    public void UpdateUserRoles(IUserRepository userRepository, UpdateUserRolesCommand command)
    {
        Enforce(new UserMustBeActive(_IsActive));

        userRepository.UpdateUserRoles(_Id, command.Roles.Select(a => new UserRole(a.Name)).ToArray());

        Publish(new UserRolesHaveBeenUpdated(this));
    }

    /// <exception cref="AggregateException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public void UpdatePassword(IEnsureUniquePasswordHistory uniquePasswordChecker, IHashPasswords passwordHasher, string password)
    {
        Enforce(new UserMustBeActive(_IsActive));
        Enforce(new UserPasswordMustBeStrong(password));
        string hashedPassword = passwordHasher.GeneratePasswordHash(password);
        Enforce(new LastFourUserPasswordsMustBeUnique(uniquePasswordChecker, _Id, hashedPassword));
        _Password = new Password(GenerateSimpleStringId(), hashedPassword, DateTimeUtc.Now);
        Publish(new UserPasswordHasBeenUpdated(this));
    }

    public override string GetId()
    {
        return _Id;
    }

    public string GetEmail()
    {
        return _Contact.Email.Value;
    }

    public override UserDto AsDto()
    {
        return new UserDto
        {
            Id = _Id,
            MerchantId = _MerchantId,
            TerminalId = _TerminalId,
            Address = _Address.AsDto(),
            Contact = _Contact.AsDto(),
            PersonalDetail = _PersonalDetail.AsDto(),
            IsActive = _IsActive,
            Password = _Password.AsDto()
        };
    }

    #endregion
}