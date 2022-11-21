using Play.Domain;

using System.ComponentModel.DataAnnotations;

namespace Play.Loyalty.Contracts.Dtos;

public record PercentageOffDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [Range(0, 100)]
    public byte Percentage { get; set; }

    #endregion
}