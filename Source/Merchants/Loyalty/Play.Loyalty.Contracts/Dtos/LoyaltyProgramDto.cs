using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Loyalty.Contracts.Dtos;

public record LoyaltyProgramDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    public IEnumerable<DiscountDto> Discounts { get; set; } = null!;

    [Required]
    public RewardsProgramDto RewardsProgram { get; set; } = null!;

    #endregion
}