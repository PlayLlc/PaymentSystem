System.ComponentModel.DataAnnotationsDtos;

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
        public AddressDto AddressDto { get; set; } = new();

        [Required]
        public ContactInfoDto ContactInfoDto { get; set; } = new();

        [Required]
        public PersonalInfoDto PersonalInfoDto { get; set; } = new();

        public bool IsActive { get; set; }

        #endregion
    }
}