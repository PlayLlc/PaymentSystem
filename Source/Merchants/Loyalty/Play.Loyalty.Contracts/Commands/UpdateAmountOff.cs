using System.ComponentModel.DataAnnotations;

using Play.Globalization.Currency;

namespace Play.Inventory.Contracts.Commands;

public record UpdateRewardsProgram
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public uint PointsPerDollar { get; set; }

    [Required]
    public uint PointsRequired { get; set; }

    [Required]
    public Money Reward { get; set; } = null!;

    #endregion
}

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