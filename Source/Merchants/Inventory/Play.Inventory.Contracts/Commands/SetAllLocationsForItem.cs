using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

public record SetAllLocationsForItem
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    #endregion
}