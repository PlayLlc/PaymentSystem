using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;

using IdentityModel;

using Microsoft.AspNetCore.Identity;

namespace Play.Identity.Api.Identity.Entities
{
    public class ContactInfo
    {
        #region Instance Values

        public int Id { get; set; }

        /// <summary>
        ///     The user's first name
        /// </summary>
        [Required]
        [MinLength(1)]
        [RegularExpression("[\x32-\x7E]{2,26}")]
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        ///     The user's last name
        /// </summary>
        [Required]
        [MinLength(1)]
        [RegularExpression("[\x32-\x7E]{2,26}")]
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        ///     The user's mobile phone number
        /// </summary>
        [Required]
        [MinLength(10)]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        ///     The user's email address
        /// </summary>
        [Required]
        [MinLength(1)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        #endregion

        #region Instance Members

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }

        #endregion
    }

    public class Address
    {
        #region Instance Values

        public int Id { get; set; }

        /// <summary>
        ///     The street address of the user's home
        /// </summary>
        [Required]
        [MinLength(1)]
        public string StreetAddress { get; set; } = string.Empty;

        /// <summary>
        ///     The apartment number of the user's home
        /// </summary>
        public string? ApartmentNumber { get; set; }

        /// <summary>
        ///     The zipcode of the user's home
        /// </summary>
        [Required]
        [MinLength(5)]
        [MaxLength(10)]
        public string Zipcode { get; set; } = string.Empty;

        /// <summary>
        ///     The state the user lives in
        /// </summary>
        [Required]
        [MinLength(1)]
        public string State { get; set; } = string.Empty;

        /// <summary>
        ///     The city where the user lives
        /// </summary>
        [Required]
        [MinLength(1)]
        public string City { get; set; } = string.Empty;

        #endregion

        #region Instance Members

        /// <exception cref="NotSupportedException"></exception>
        public string Normalize()
        {
            return JsonSerializer.Serialize(new
            {
                street_address = StreetAddress,
                locality = City,
                region = State,
                postal_code = Zipcode,
                country = "United States"
            });
        }

        #endregion
    }

    public class PersonalInfo
    {
        #region Instance Values

        public int Id { get; set; }

        /// <summary>
        ///     The last four digits of the user's Social Security Number
        /// </summary>
        [Required]
        [MinLength(4)]
        [MaxLength(4)]
        [RegularExpression("[\\d]{4}")]
        public string LastFourOfSocial { get; set; } = string.Empty;

        /// <summary>
        ///     The user's date of birth
        /// </summary>
        [Required]
        public DateTime DateOfBirth { get; set; }

        #endregion
    }

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
}