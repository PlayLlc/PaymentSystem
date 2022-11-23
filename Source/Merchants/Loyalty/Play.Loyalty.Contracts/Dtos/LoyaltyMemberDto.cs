using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;
using Play.Globalization.Time;

namespace Play.Loyalty.Contracts.Dtos;

public record LoyaltyMemberDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; } = string.Empty;

    [Required]
    [Numeric]
    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    [Numeric]
    [StringLength(20)]
    public string RewardsNumber { get; set; } = string.Empty;

    [Required]
    public RewardsDto Rewards { get; set; } = null!;

    #endregion
}