using Play.Domain.Aggregates;
using Play.Globalization.Time;
using Play.Merchants.Onboarding.Domain.Common;

namespace Play.Merchants.Onboarding.Domain.Aggregates;

public class User : Aggregate<string>
{
    #region Instance Values

    private readonly UserId _Id;
    private readonly HashSet<UserRole> _Roles;

    private Address _Address;
    private ContactInfo _ContactInfo;
    private DateTimeUtc _DateOfBirth;
    private string _LastFourOfSsn;
    private bool _IsActive;

    #endregion

    #region Constructor

    private User()
    {
        // Entity Framework only
    }

    public User(UserId id, Address address, ContactInfo contactInfo, string lastFourOfSsn, DateTimeUtc dateOfBirth, bool isActive, params UserRole[] roles)
    {
        _Id = id;
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

    #endregion
}