using Play.Accounts.Contracts.Common;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates.UserRegistration;
using Play.Accounts.Domain.Entities;
using Play.Domain.Aggregates;
using Play.Globalization.Time;

using Address = Play.Accounts.Domain.Entities.Address;
using ContactInfo = Play.Accounts.Domain.Entities.ContactInfo;

namespace Play.Accounts.Domain.Aggregates.Users;

public class User : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;

    // private readonly MerchantId _MerchantId;
    private readonly HashSet<UserRole> _Roles;

    private readonly Address _Address;
    private readonly ContactInfo _ContactInfo;
    private readonly DateTimeUtc _DateOfBirth;
    private readonly string _LastFourOfSsn;
    private readonly bool _IsActive;

    #endregion

    #region Constructor

    private User()
    {
        // Entity Framework only
    }

    public User(string id, Address address, ContactInfo contactInfo, string lastFourOfSsn, DateTimeUtc dateOfBirth, bool isActive, params UserRole[] roles)
    {
        _Id = id;

        //_MerchantId = merchantId;
        _Address = address;
        _ContactInfo = contactInfo;
        _LastFourOfSsn = lastFourOfSsn;
        _DateOfBirth = dateOfBirth;
        _IsActive = isActive;
        _Roles = roles.ToHashSet();
    }

    #endregion

    #region Instance Members

    public static User CreateFromUserRegistration(
        string userRegistrationId, Address address, ContactInfo contactInfo, string lastFourOfSsn, DateTimeUtc dateOfBirth)
    {
        User user = new User(userRegistrationId, address, contactInfo, lastFourOfSsn, dateOfBirth, true, UserRole.Member);
        user.Publish(new UserCreated(user.GetId()));

        return user;
    }

    public void AddRole(UserRole role)
    {
        if (_Roles.Add(role))
            Publish(new UserRoleAdded(_Id!, role));
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
            ContactInfoDto = _ContactInfo.AsDto(),
            PersonalInfoDto = new PersonalInfoDto()
            {
                DateOfBirth = _DateOfBirth,
                LastFourOfSocial = _LastFourOfSsn
            },
            IsActive = _IsActive
        };
    }

    #endregion
}