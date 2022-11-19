using Play.Domain;

using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Dtos;

public record CategoryDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    #endregion
}