using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

public record RemoveItem
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    #endregion
}