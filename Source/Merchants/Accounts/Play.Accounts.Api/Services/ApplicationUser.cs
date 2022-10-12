using Microsoft.AspNetCore.Identity;

using Play.Accounts.Contracts.Common;
using Play.Accounts.Contracts.Dtos;
using Play.Globalization.Time;

namespace Play.Accounts.Api.Services
{
    public class ApplicationUser : IdentityUser
    {
        #region Instance Values

        public string MerchantId { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string StreetAddress { get; set; } = string.Empty;

        public string? ApartmentNumber { get; set; }

        public string Zipcode { get; set; } = string.Empty;

        public string StateAbbreviation { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string LastFourOfSocial { get; set; } = string.Empty;

        public DateTimeUtc DateOfBirth { get; set; }

        public DateTimeUtc LastPasswordUpdate { get; set; }

        public bool IsActive { get; set; }

        #endregion

        #region Instance Members

        public bool IsPasswordExpired()
        {
            return (DateTimeUtc.Now - LastPasswordUpdate) < new TimeSpan(90);
        }

        public UserDto AsDto()
        {
            AddressDto addressDto = new AddressDto()
            {
                StreetAddress = StreetAddress,
                ApartmentNumber = ApartmentNumber,
                City = City,
                StateAbbreviation = StateAbbreviation,
                Zipcode = Zipcode
            };
            ContactInfoDto contactInfoDto = new ContactInfoDto()
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Phone = PhoneNumber
            };

            PersonalInfoDto personalInfoDto = new PersonalInfoDto()
            {
                DateOfBirth = new DateTimeUtc(DateOfBirth),
                LastFourOfSocial = LastFourOfSocial
            };

            return new UserDto
            {
                Id = Id,
                AddressDto = addressDto,
                ContactInfoDto = contactInfoDto,
                PersonalInfoDto = personalInfoDto,
                IsActive = IsActive
            };
        }

        #endregion
    }
}