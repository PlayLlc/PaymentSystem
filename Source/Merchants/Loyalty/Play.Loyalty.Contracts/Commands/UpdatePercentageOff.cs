using System.ComponentModel.DataAnnotations;

using Play.Globalization.Currency;
using Play.Loyalty.Domain.Enums;

namespace Play.Inventory.Contracts.Commands;

public record UpdatePercentageOff
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; }

    [Required]
    [StringLength(20)]
    public string UserId { get; set; }

    [Required]
    [MinLength(1)]
    public byte Percentage { get; set; }

    #endregion
}

public record UpdateAmountOff
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string UserId { get; set; }

    [Required]
    public Money AmountOff { get; set; } = null!;

    #endregion
}

public record ActivateRewardProgram
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; }

    [Required]
    [StringLength(20)]
    public string UserId { get; set; }

    [Required]
    public bool Activate { get; set; }

    #endregion
}

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