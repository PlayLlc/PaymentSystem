using Play.Domain.Common.Attributes;

using System.ComponentModel.DataAnnotations;

namespace Play.Loyalty.Contracts.Commands;

public record ToggleRewardProgramActivation
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public bool Active { get; set; }

    #endregion
}