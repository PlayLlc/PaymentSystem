using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Inventory.Contracts.Dtos;

public record MerchantDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; }

    #endregion
}