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
    public PersonalDetail PersonalDetail;

    private Contact _Contact;

    public Contact Contact
    {
        get => _Contact;
        set
        {
            _Contact = value;
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
        Contact = new Contact(dto.ContactInfoDto);
        PersonalDetail = new PersonalDetail(dto.PersonalInfoDto);
    }

    public UserIdentity(Contact contact, Address address, PersonalDetail personalDetail)
    {
        Contact = contact;
        Address = address;
        PersonalDetail = personalDetail;
    }

    #endregion

    #region Instance Members

    public IEnumerable<Claim> GenerateClaims()
    {
        return new List<Claim>
        {
            new(JwtClaimTypes.Subject, $"{Guid.NewGuid()}_{DateTime.UtcNow.Ticks}"),
            new(JwtClaimTypes.Name, $"{Contact.GetFullName()}"),
            new(JwtClaimTypes.GivenName, Contact.FirstName),
            new(JwtClaimTypes.FamilyName, Contact.LastName),
            new(JwtClaimTypes.Email, Contact.Email),
            new(JwtClaimTypes.BirthDate, PersonalDetail.DateOfBirth.ToShortDateString()),
            new(JwtClaimTypes.Address, Address.Normalize())
        };
    }

    #endregion
}