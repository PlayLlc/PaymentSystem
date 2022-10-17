using System.Security.Claims;

using IdentityModel;

using Microsoft.AspNetCore.Identity;

using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;

namespace Play.Accounts.Persistence.Sql.Entities;

public sealed class UserIdentity : IdentityUser
{
    #region Instance Values

    public Address Address;
    public PersonalInfo PersonalInfo;

    private ContactInfo _ContactInfo;

    public ContactInfo ContactInfo
    {
        get => _ContactInfo;
        set
        {
            _ContactInfo = value;
            UserName = value.Email;

            //NormalizedUserName = value.Email.Value.ToUpper();
            Email = value.Email;

            //NormalizedEmail = Email.ToUpper();
            PhoneNumber = value.Phone;
        }
    }

    public bool IsActive { get; set; }
    public int? EmailConfirmationCode { get; set; }
    public int? MobileConfirmationCode { get; set; }

    #endregion

    #region Constructor

    private UserIdentity()
    { }

    public UserIdentity(UserDto dto)
    {
        Address = new Address(dto.AddressDto);
        ContactInfo = new ContactInfo(dto.ContactInfoDto);
        PersonalInfo = new PersonalInfo(dto.PersonalInfoDto);
    }

    public UserIdentity(ContactInfo contactInfo, Address address, PersonalInfo personalInfo)
    {
        ContactInfo = contactInfo;
        Address = address;
        PersonalInfo = personalInfo;
    }

    #endregion

    #region Instance Members

    public IEnumerable<Claim> GenerateClaims()
    {
        return new List<Claim>
        {
            new(JwtClaimTypes.Subject, $"{Guid.NewGuid()}_{DateTime.UtcNow.Ticks}"),
            new(JwtClaimTypes.Name, $"{ContactInfo.GetFullName()}"),
            new(JwtClaimTypes.GivenName, ContactInfo.FirstName),
            new(JwtClaimTypes.FamilyName, ContactInfo.LastName),
            new(JwtClaimTypes.Email, ContactInfo.Email),
            new(JwtClaimTypes.BirthDate, PersonalInfo.DateOfBirth.ToShortDateString()),
            new(JwtClaimTypes.Address, Address.Normalize())
        };
    }

    #endregion
}