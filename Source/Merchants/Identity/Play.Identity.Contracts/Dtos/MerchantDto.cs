using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Dtos;

namespace Play.Identity.Contracts.Dtos;

public class MerchantDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    public AddressDto Address { get; set; } = new AddressDto();

    [Required]
    public BusinessInfoDto BusinessInfo { get; set; } = new BusinessInfoDto();

    [Required]
    public bool IsActive { get; set; }

    #endregion
}