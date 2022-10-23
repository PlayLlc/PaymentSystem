using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Commands.User;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;
using Play.Domain;
using Play.Domain.Aggregates;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;

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

    private User()
    {
        // Entity Framework only
    }

    public User(
        string id, string merchantId, string terminalId, Password password, Address address, Contact contact, PersonalDetail personalDetail, bool isActive)
    {
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

    internal string GetMerchantId()
    {
        return _MerchantId;
    }

    // HACK: What's going on here?
    /// <exception cref="BusinessRuleValidationException"></exception>
    public void LoginValidation()
    {
        Enforce(new UserMustBeActive(_IsActive));
        Enforce(new UserMustUpdatePasswordEvery90Days(_Password));
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
        _Password = new Password(GenerateSimpleStringId(), hashedPassword, DateTime.UtcNow);
        Publish(new UserPasswordHasBeenUpdated(this));
    }

    public override string GetId()
    {
        return _Id;
    }

    public override UserDto AsDto()
    {
        return new UserDto
        {
            Id = _Id,
            MerchantId = _MerchantId,
            TerminalId = _TerminalId,
            AddressDto = _Address.AsDto(),
            ContactDto = _Contact.AsDto(),
            PersonalDetailDto = _PersonalDetail.AsDto(),
            IsActive = _IsActive,
            Password = _Password.AsDto()
        };
    }

    #endregion
}