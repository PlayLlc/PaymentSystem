using System.ComponentModel.DataAnnotations;

using Play.Inventory.Contracts.Dtos;

namespace Play.Loyalty.Contracts.Commands;

public record ClaimReward
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string RewardId { get; set; } = string.Empty;

    [Required]
    public MoneyDto RewardAmount { get; set; } = null!;

    #endregion
}