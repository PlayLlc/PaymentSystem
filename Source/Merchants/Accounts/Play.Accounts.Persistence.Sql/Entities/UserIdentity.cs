using System.Security.Claims;

using IdentityModel;

using Microsoft.AspNetCore.Identity;

using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Entities;
using Play.Globalization.Time;

namespace Play.Accounts.Persistence.Sql.Entities;

public sealed class UserIdentity : IdentityUser
{
    #region Instance Values

    public string MerchantId;
    public string TerminalId;
    public Password Password;
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

    #endregion

    #region Constructor

    private UserIdentity()
    { }

    public UserIdentity(UserDto dto)
    {
        Id = dto.Id;
        MerchantId = dto.MerchantId;
        TerminalId = dto.TerminalId;
        Password = new Password(dto.Password);
        PasswordHash = dto.Password.HashedPassword;
        Address = new Address(dto.AddressDto);
        Contact = new Contact(dto.ContactDto);
        PersonalDetail = new PersonalDetail(dto.PersonalDetailDto);
    }

    public UserIdentity(string id, string merchantId, string terminalId, Password password, Contact contact, Address address, PersonalDetail personalDetail)
    {
        Id = id;
        MerchantId = merchantId;
        TerminalId = terminalId;
        Password = password;
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
            new(JwtClaimTypes.Subject, $"{Guid.NewGuid()}_{DateTimeUtc.Now.Ticks}"),
            new(JwtClaimTypes.Name, $"{Contact.GetFullName()}"),
            new(JwtClaimTypes.GivenName, Contact.FirstName),
            new(JwtClaimTypes.FamilyName, Contact.LastName),
            new(JwtClaimTypes.Email, Contact.Email),
            new(JwtClaimTypes.BirthDate, PersonalDetail.DateOfBirth.ToShortDateFormat()),
            new(JwtClaimTypes.Address, Address.Normalize())
        };
    }

    #endregion
}