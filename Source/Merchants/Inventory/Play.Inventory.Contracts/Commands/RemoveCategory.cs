using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

public record RemoveCategory
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
    public string CategoryId { get; set; } = string.Empty;

    #endregion
}