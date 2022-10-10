using Play.Domain;

using System.ComponentModel.DataAnnotations;

namespace Play.Accounts.Contracts.Dtos
{
    public class UserDto : IDto
    {
        #region Instance Values

        [Required]
        [MinLength(1)]
        public string Id { get; set; } = string.Empty;

        [Required]
        [MinLength(1)]
        public string MerchantId { get; set; } = string.Empty;

        [Required]
        public AddressDto Address { get; set; } = new();

        [Required]
        public ContactInfoDto ContactInfo { get; set; } = new();

        [Required]
        public PersonalInfoDto PersonalInfo { get; set; } = new();

        public bool IsActive { get; set; }

        #endregion
    }
}