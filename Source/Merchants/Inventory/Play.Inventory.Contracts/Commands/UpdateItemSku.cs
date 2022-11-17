using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.Inventory.Contracts.Commands;

public record UpdateItemSku
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    [MaxLength(15)]
    [AlphaNumericSpecial]
    public string Sku { get; set; } = string.Empty;

    #endregion
}