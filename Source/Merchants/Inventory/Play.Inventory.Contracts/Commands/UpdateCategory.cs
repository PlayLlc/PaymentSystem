using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

/// <summary>
///     Update a category that an item is associated with
/// </summary>
public record UpdateCategory
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string CategoryId { get; set; } = string.Empty;

    #endregion
}