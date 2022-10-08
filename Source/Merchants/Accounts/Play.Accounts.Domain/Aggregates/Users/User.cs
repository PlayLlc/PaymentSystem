using Play.Accounts.Contracts.Dtos;
using Play.Domain.Aggregates;
using Play.Globalization.Time;
using Play.Merchants.Onboarding.Domain.Common;

namespace Play.Merchants.Onboarding.Domain.Aggregates;

public class User : Aggregate<string>
{
    #region Instance Values

    private readonly UserId _Id;

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

    public User(UserId id, Address address, ContactInfo contactInfo, string lastFourOfSsn, DateTimeUtc dateOfBirth, bool isActive, params UserRole[] roles)
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
        UserRegistrationId userRegistrationId, Address address, ContactInfo contactInfo, string lastFourOfSsn, DateTimeUtc dateOfBirth)
    {
        User user = new User(new UserId(userRegistrationId.Id), address, contactInfo, lastFourOfSsn, dateOfBirth, true, UserRole.Member);
        user.Raise(new UserCreated(user.GetId()));

        return user;
    }

    public void AddRole(UserRole role)
    {
        if (_Roles.Add(role))
            Raise(new UserRoleAdded((UserId) _Id!, role));
    }

    public override UserId GetId()
    {
        return (UserId) _Id;
    }

    public override UserDto AsDto()
    {
        return new UserDto
        {
            Id = _Id.Id, /* MerchantId = _MerchantId.Id,*/ Address = _Address.AsDto(), ContactInfo = _ContactInfo.AsDto(),
            PersonalInfo = new PersonalInfoDto() {DateOfBirth = _DateOfBirth, LastFourOfSocial = _LastFourOfSsn}, IsActive = _IsActive
        };
    }

    #endregion
}