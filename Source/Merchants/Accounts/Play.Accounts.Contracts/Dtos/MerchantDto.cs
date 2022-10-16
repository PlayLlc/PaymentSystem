using System.ComponentModel.DataAnnotations;

using Play.Accounts.Contracts.Common;
using Play.Domain;

namespace Play.Accounts.Contracts.Dtos;

public class MerchantDto : IDto
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    public AddressDto AddressDto { get; set; } = new();

    [Required]
    public string BusinessType { get; set; } = string.Empty;

    [Required]
    [MinLength(4)]
    [MaxLength(4)]
    public string MerchantCategoryCode { get; set; } = string.Empty;

    #endregion
}