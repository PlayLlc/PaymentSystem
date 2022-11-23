using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.Loyalty.Contracts.Commands;

public record UpdateLoyaltyMember
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    [Phone]
    [Required]
    [Numeric]
    public string Phone { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; } = string.Empty;

    #endregion
}