using System.Security.Claims;

using IdentityModel;

using Microsoft.AspNetCore.Identity;

using Play.Domain.Common.Dtos;
using Play.Domain.Common.Entities;
using Play.Domain.Entities;
using Play.Globalization.Time;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.Entities;

namespace Play.Identity.Persistence.Sql.Entities;

public sealed class UserIdentity : IdentityUser, IEntity
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
        Address = new Address(dto.Address);
        Contact = new Contact(dto.Contact);
        PersonalDetail = new PersonalDetail(dto.PersonalDetail);
    }

    public UserIdentity(string id, string merchantId, string terminalId, Password password, Contact contact, Address address, PersonalDetail personalDetail)
    {
        Id = id;
        MerchantId = merchantId;
        TerminalId = terminalId;
        Password = password;
        PasswordHash = password.HashedPassword;
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

    public UserDto AsDto()
    {
        AddressDto address = Address.AsDto();
        ContactDto contact = Contact.AsDto();
        PersonalDetailDto personal = PersonalDetail.AsDto();
        PasswordDto password = Password.AsDto();

        return new UserDto
        {
            Id = Id,
            MerchantId = MerchantId,
            TerminalId = TerminalId,
            Password = password,
            Address = address,
            Contact = contact,
            PersonalDetail = personal,
            IsActive = IsActive
        };
    }

    #endregion
}