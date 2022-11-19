using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Inventory.Contracts.Dtos;

public record ItemDto : IDto
{
    #region Instance Values

    [Required]
    public string Id { get; set; }

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int Quantity { get; set; }

    [Required]
    public LocationsDto Locations { get; set; } = null!;

    public string Description { get; set; } = string.Empty;
    public AlertsDto Alerts { get; set; } = new();
    public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
    public IEnumerable<VariationDto> Variations { get; set; } = new List<VariationDto>();

    #endregion
}