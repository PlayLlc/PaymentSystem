using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.Identity.Contracts.Commands;

public record RemoveMerchant
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    #endregion
}