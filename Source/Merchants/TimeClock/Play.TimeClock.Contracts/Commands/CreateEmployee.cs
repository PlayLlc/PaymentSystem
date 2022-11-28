using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.TimeClock.Contracts.Commands;

public record CreateEmployee
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

    #endregion
}