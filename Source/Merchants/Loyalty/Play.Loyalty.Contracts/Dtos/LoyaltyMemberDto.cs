using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Globalization.Time;

namespace Play.Loyalty.Contracts.Dtos;

public record LoyaltyMemberDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(10)]
    public string RewardsNumber { get; set; } = string.Empty;

    [Required]
    public RewardsDto Rewards { get; set; } = null!;

    #endregion
}