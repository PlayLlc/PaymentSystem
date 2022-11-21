using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Entitiesd;

namespace Play.Loyalty.Contracts.Dtos;

public record RewardProgramDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    public AmountOffDto AmountOff { get; set; } = null!;

    [Required]
    public PercentageOffDto PercentageOff { get; set; } = null!;

    [Required]
    public uint PointsPerDollar { get; set; }

    [Required]
    public uint RewardThreshold { get; set; }

    [Required]
    public string RewardType { get; set; } = string.Empty;

    #endregion
}