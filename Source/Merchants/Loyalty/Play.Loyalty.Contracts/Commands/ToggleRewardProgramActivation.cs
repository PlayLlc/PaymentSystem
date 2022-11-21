using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

public record ToggleRewardProgramActivation
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; }

    [Required]
    [StringLength(20)]
    public string UserId { get; set; }

    public bool Active { get; set; }

    #endregion
}