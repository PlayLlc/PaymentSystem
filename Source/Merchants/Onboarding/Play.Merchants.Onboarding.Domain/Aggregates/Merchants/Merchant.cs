using Play.Domain.Aggregates;
using Play.Globalization.Time;
using Play.Merchants.Onboarding.Domain.Common;

namespace Play.Merchants.Onboarding.Domain.Aggregates;

public class Merchant : Aggregate<string>
{
    #region Instance Values

    private readonly UserId _Id;

    private Address _Address;
    private ContactInfo _ContactInfo;

    #endregion

    #region Constructor

    private Merchant()
    {
        // Entity Framework only
    }

    public Merchant(MerchantId id, Address address, ContactInfo contactInfo)
    {
        _Id = id;
        _Address = address;
        _ContactInfo = contactInfo;
    }

    #endregion

    #region Instance Members

    public static Merchant CreateFromUserRegistration(
        UserRegistrationId userRegistrationId, Address address, ContactInfo contactInfo, string lastFourOfSsn, DateTimeUtc dateOfBirth)
    {
        return new Merchant(new UserId(userRegistrationId.Id), address, contactInfo, lastFourOfSsn, dateOfBirth, true, UserRole.Member);
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