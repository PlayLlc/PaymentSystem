using System.Security.Claims;

using IdentityModel;

using Microsoft.AspNetCore.Identity;

namespace Play.Identity.Api.Identity.Entities;

public sealed class UserIdentity : IdentityUser
{
    #region Instance Values

    private ContactInfo _ContactInfo = new();

    public Address Address { get; set; } = new();

    public ContactInfo ContactInfo
    {
        get => _ContactInfo;
        set
        {
            UserName = value.Email;
            NormalizedUserName = value.Email.ToLower();
            Email = value.Email;
            NormalizedEmail = value.Email.ToLower();
            PhoneNumber = value.Phone;
            _ContactInfo = value;
        }
    }

    public PersonalInfo PersonalInfo { get; set; } = new();
    public int? EmailConfirmationCode { get; set; }
    public int? MobileConfirmationCode { get; set; }

    #endregion

    #region Constructor

    public UserIdentity()
    { }

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