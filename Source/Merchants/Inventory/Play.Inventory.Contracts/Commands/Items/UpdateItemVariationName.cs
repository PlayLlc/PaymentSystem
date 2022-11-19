using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

public record UpdateItemVariationName
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    #endregion
}