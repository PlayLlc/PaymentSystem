using System.ComponentModel.DataAnnotations;

using Play.Globalization.Currency;

namespace Play.Loyalty.Contracts.Commands;

public record CreateLoyaltyProgram
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public NumericCurrencyCode NumericCurrencyCode { get; set; }

    #endregion
}