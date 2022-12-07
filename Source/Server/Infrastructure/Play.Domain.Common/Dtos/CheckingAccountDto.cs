using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.Domain.Common.Dtos;

public record CheckingAccountDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    [Required]
    [RoutingNumber]
    public string RoutingNumber { get; set; } = string.Empty;

    [Required]
    [AccountNumber]
    public string AccountNumber { get; set; } = string.Empty;

    #endregion
}