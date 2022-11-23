using System.ComponentModel.DataAnnotations;

using Play.Globalization.Currency;

namespace Play.Loyalty.Contracts.Commands;

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
    public Money RewardAmount { get; set; } = null!;

    #endregion
}