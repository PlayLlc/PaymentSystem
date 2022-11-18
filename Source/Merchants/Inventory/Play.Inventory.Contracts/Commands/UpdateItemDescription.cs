using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

public record UpdateItemDescription
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Description { get; set; } = string.Empty;

    #endregion
}