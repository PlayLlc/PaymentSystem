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
    private readonly Address _Address;
    private readonly Contact _Contact;
    private readonly PersonalDetail _PersonalDetail;
    private string _HashedPassword;

    #endregion

    #region Constructor

    private User()
    {
        // Entity Framework only
    }

    private User(
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

    public static User CreateFromUserRegistration(
        string userRegistrationId, string hashedPassword, Address address, Contact contact, PersonalDetail personalDetail, params UserRole[] roles)
    {
        User user = new User(userRegistrationId, hashedPassword, address, contact, personalDetail, true, roles);
        user.Publish(new UserCreated(user.GetId()));

        return user;
    }

    public void UpdatePassword(IHashPasswords passwordHasher, string password)
    {
        Enforce(new UserPasswordMustBeStrong(password));

        string hashedPassword = passwordHasher.GeneratePasswordHash(password);

        // TODO: Check the last 4 password hashes and make sure this is unique

        _HashedPassword = hashedPassword;

        // TODO: Publish domain event broadcasting the user's password was updated
    }

    public void UpdateAddress(Address address)
    {
        // TODO: broadcast address change
    }

    public void UpdateContactInfo(Contact address)
    {
        // TODO: broadcast Contact Info change
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
            ContactInfoDto = _Contact.AsDto(),
            PersonalInfoDto = _PersonalDetail.AsDto(),
            IsActive = _IsActive,
            HashedPassword = _HashedPassword
        };
    }

    #endregion
}