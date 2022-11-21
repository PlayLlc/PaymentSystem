using System.ComponentModel.DataAnnotations;

using Play.Globalization.Currency;

namespace Play.Domain.Common.Entitiesd;

public record DiscountDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string VariationId { get; set; } = string.Empty;

    /// <summary>
    ///     The discounted price of the inventory item
    /// </summary>
    [Required]
    public Money Price { get; set; } = null!;

    #endregion
}