using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

public class User : Aggregate<string>
{
    #region Instance Values

    private readonly string _Id;

    private readonly string _HashedPassword;

    private readonly bool _IsActive;
    private readonly Address _Address;
    private readonly Contact _Contact;
    private readonly PersonalDetail _PersonalDetail;
    private readonly List<UserRole> _Roles;

    #endregion

    #region Constructor

    private User()
    {
        // Entity Framework only
    }

    public User(string id, string hashedPassword, Address address, Contact contact, PersonalDetail personalDetail, bool isActive, params UserRole[] roles)
    {
        _Id = id;

        _HashedPassword = hashedPassword;
        _Address = address;
        _Contact = contact;
        _PersonalDetail = personalDetail;
        _IsActive = isActive;
        _Roles = roles.ToList();
    }

    #endregion

    #region Instance Members

    public static User CreateFromUserRegistration(
        string userRegistrationId, string hashedPassword, Address address, Contact contact, PersonalDetail personalDetail, params UserRole[] roles)
    {
        User user = new User(userRegistrationId, hashedPassword, address, contact, personalDetail, true, roles);
        user.Publish(new UserCreated(user.GetId()));

        return user;
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
            IsActive = _IsActive
        };
    }

    #endregion
}