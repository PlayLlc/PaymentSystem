using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.ValueObjects;
using Play.Globalization.Currency;

namespace Play.Inventory.Contracts.Dtos;

public record AlertsDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public bool IsActive { get; set; }

    [Required]
    [StringLength(15)]
    public ushort LowInventoryThreshold { get; set; }

    #endregion
}