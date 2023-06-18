using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

public record CreateStockItem
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    #endregion
}