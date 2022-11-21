using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;
using Play.Domain.Common.Entitiesd;

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
    public RewardProgramDto RewardProgram { get; set; } = null!;

    #endregion
}