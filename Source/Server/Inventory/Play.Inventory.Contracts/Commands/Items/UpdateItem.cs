using Play.Globalization.Currency;
using Play.Inventory.Contracts.Dtos;
using System.ComponentModel.DataAnnotations;

namespace Play.Inventory.Contracts.Commands;

public record UpdateItem
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string ItemId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    public string? Name { get; set; } = string.Empty;

    [Required]
    public Money? Price { get; set; } = null!;

    public AlertsDto? Alerts { get; set; }
    public string? Description { get; set; } = string.Empty;

    public string? Sku { get; set; } = string.Empty;

    public IEnumerable<CategoryDto> Categories { get; set; } = Array.Empty<CategoryDto>();
    public LocationsDto? Locations { get; set; }


    #endregion
}