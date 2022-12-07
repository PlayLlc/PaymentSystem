using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Dtos;

namespace Play.Loyalty.Contracts.Dtos;

public record RewardsDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    public uint Points { get; set; }

    [Required]
    public MoneyDto Balance { get; set; } = null!;

    #endregion
}