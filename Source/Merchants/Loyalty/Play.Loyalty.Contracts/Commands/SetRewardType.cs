using System.ComponentModel.DataAnnotations;

using Play.Loyalty.Domain.Enums;

namespace Play.Inventory.Contracts.Commands;

public record SetRewardType
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string UserId { get; set; }

    [Required]
    public RewardTypes RewardType { get; set; } = null!;

    #endregion
}