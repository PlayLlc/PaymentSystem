using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

/// <summary>
///     Update a category that an item is associated with
/// </summary>
public record UpdateItemCategories
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public List<string> CategoryIds { get; set; } = new List<string>();

    #endregion
}