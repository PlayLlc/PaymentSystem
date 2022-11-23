using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

public record CreateLoyaltyMember
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string RewardsNumber { get; set; } = string.Empty;

    #endregion
}