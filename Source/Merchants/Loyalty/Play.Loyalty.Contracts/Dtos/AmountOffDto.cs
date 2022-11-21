using Play.Globalization.Currency;

using System.ComponentModel.DataAnnotations;

namespace Play.Domain.Common.Entitiesd;

public record AmountOffDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    public Money Amount { get; set; } = null!;

    #endregion
}