using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;
using Play.Domain.Common.Dtos;
using Play.Globalization.Currency;

namespace Play.Loyalty.Contracts.Dtos;

public record DiscountDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string VariationId { get; set; } = string.Empty;

    /// <summary>
    ///     The discounted price of the inventory item
    /// </summary>
    [Required]
    public MoneyDto Price { get; set; } = null!;

    #endregion
}