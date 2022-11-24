using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;

namespace Play.Loyalty.Contracts.Dtos;

public record DiscountsProgramDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public IEnumerable<DiscountDto> Discounts { get; set; } = Array.Empty<DiscountDto>();

    #endregion
}