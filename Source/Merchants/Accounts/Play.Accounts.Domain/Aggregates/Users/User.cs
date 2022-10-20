using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Services;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

public class User : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;

    private readonly string _MerchantId;
    private readonly string _TerminalId;
    private readonly bool _IsActive;
    private readonly List<UserRole> _Roles;
    private readonly PersonalDetail _PersonalDetail;
    private Address _Address;
    private Contact _Contact;
    private string _HashedPassword;

    #endregion

    #region Constructor

    private User()
    {
        // Entity Framework only
    }

    public User(
        string id, string merchantId, string terminalId, string hashedPassword, Address address, Contact contact, PersonalDetail personalDetail, bool isActive,
        params UserRole[] roles)
    {
        _Id = id;
        _MerchantId = merchantId;
        _TerminalId = terminalId;
        _HashedPassword = hashedPassword;
        _Address = address;
        _Contact = contact;
        _PersonalDetail = personalDetail;
        _IsActive = isActive;
        _Roles = roles.ToList();
    }

    #endregion

    #region Instance Members

    internal string GetMerchantId()
    {
        return _MerchantId;
    }

    public void UpdateAddress(UpdateUserAddressCommand command)
    {
        // TODO: Merchant underwriting for new Address? Verify not prohibited? Verify still in area where we're allowed to operate?
        _Address = new Address(command.Address);

        // TODO: broadcast address change
    }

    public void UpdateContactInfo(UpdateUserContactCommand command)
    {
        _Contact = new Contact(command.Contact);

        // TODO: broadcast Contact Info change
    }

    public void UpdatePassword(IHashPasswords passwordHasher, string password)
    {
        Enforce(new UserPasswordMustBeStrong(password));

        string hashedPassword = passwordHasher.GeneratePasswordHash(password);

        // TODO: Check the last 4 password hashes and make sure this is unique

        _HashedPassword = hashedPassword;

        // TODO: Publish domain event broadcasting the user's password was updated
    }

    public void AddUserRole(UserRole role)
    {
        // Add User Role
        // TODO: Publish Domain event
    }

    public void RemoveUserRole(UserRole role)
    {
        // Remove User Role
        // TODO: Publish Domain Event
    }

    public override string GetId()
    {
        return _Id;
    }

    public override UserDto AsDto()
    {
        return new UserDto
        {
            Id = _Id, /* MerchantId = _MerchantId.Id,*/
            AddressDto = _Address.AsDto(),
            ContactDto = _Contact.AsDto(),
            PersonalDetailDto = _PersonalDetail.AsDto(),
            IsActive = _IsActive,
            HashedPassword = _HashedPassword
        };
    }

    #endregion
}