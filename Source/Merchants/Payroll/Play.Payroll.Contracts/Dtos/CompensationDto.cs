using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;
using Play.Domain.Common.Dtos;

namespace Play.Loyalty.Contracts.Dtosd;

public record CompensationDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    [Required]
    public MoneyDto HourlyWage { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public string CompensationType { get; set; } = string.Empty;

    #endregion
}