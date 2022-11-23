using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.Loyalty.Contracts.Commands;

public record RemoveLoyaltyMember
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
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string LoyaltyMemberId { get; set; } = string.Empty;

    #endregion
}