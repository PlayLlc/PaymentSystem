using Play.Core;
using Play.Domain.Aggregates;
using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Globalization.Time;
using Play.Identity.Contracts.Commands;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.Aggregates.Rules;
using Play.Identity.Domain.Entities;
using Play.Identity.Domain.Repositories;
using Play.Identity.Domain.Services;

namespace Play.Identity.Domain.Aggregates;

public class User : Aggregate<SimpleStringId>
{
    #region Instance Values

    private readonly string _MerchantId;
    private readonly string _TerminalId;
    private bool _IsActive;
    private PersonalDetail _PersonalDetail;
    private Address _Address;
    private Contact _Contact;
    private Password _Password;

    public override SimpleStringId Id { get; }

    #endregion

    #region Constructor

    // Constructor for Entity Framework
    private User()
    { }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public User(UserDto dto)
    {
        if (dto.Id != dto.Password.Id)
            throw new ArgumentException($"The {nameof(Password)} ID must match the {nameof(User)} ID");

        Id = new SimpleStringId(dto.Id);
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

        Id = new SimpleStringId(id);
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

    public bool IsActive() => _IsActive;
    public bool DoesUserBelongToMerchant(string merchantId) => _MerchantId == merchantId;

    internal string GetHashedPassword() => _Password.HashedPassword;

    public Result LoginValidation(IHashPasswords passwordHasher, string clearTextPassword)
    {
        // TODO: User account must be temporarily locked after 6 login attempts for a minimum of 30..

        Result<IBusinessRule> userMustBeActive = IsEnforced(new UserMustBeActive(_IsActive));

        if (!userMustBeActive.Succeeded)
            return new Result(userMustBeActive.Errors);

        Result<IBusinessRule> expiredPassword = IsEnforced(new UserMustUpdatePasswordEvery90Days(_Password));

        if (!expiredPassword.Succeeded)
            return new Result(expiredPassword.Errors);

        Result<IBusinessRule> passwordValidation = IsEnforced(new PasswordMustBeCorrectToLogin(passwordHasher, _Password.HashedPassword, clearTextPassword));

        if (!passwordValidation.Succeeded)
            return new Result(passwordValidation.Errors);

        return new Result();
    }

    /// <exception cref="AggregateException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public void Update(IUnderwriteMerchants merchantUnderwriter, UpdateAddressCommand command)
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
    public void Update(IUnderwriteMerchants merchantUnderwriter, UpdateContactCommand command)
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
    public void Update(IUnderwriteMerchants merchantUnderwriter, UpdatePersonalDetailCommand command)
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

        userRepository.UpdateUserRoles(Id, command.Roles.Select(a => new UserRole(a.Name)).ToArray());

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
        Enforce(new LastFourUserPasswordsMustBeUnique(uniquePasswordChecker, Id, hashedPassword));
        _Password = new Password(GenerateSimpleStringId(), hashedPassword, DateTimeUtc.Now);
        Publish(new UserPasswordHasBeenUpdated(this));
    }

    public override SimpleStringId GetId() => Id;

    public string GetEmail() => _Contact.Email.Value;

    public override UserDto AsDto() =>
        new()
        {
            Id = Id,
            MerchantId = _MerchantId,
            TerminalId = _TerminalId,
            Address = _Address.AsDto(),
            Contact = _Contact.AsDto(),
            PersonalDetail = _PersonalDetail.AsDto(),
            IsActive = _IsActive,
            Password = _Password.AsDto()
        };

    #endregion
}