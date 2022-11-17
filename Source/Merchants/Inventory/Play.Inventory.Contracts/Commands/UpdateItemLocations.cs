using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

/// <summary>
///     Update the stores that an item belongs to
/// </summary>
public record UpdateItemLocations
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
    public IEnumerable<string> StoreIds { get; set; } = new List<string>();

    #endregion
}