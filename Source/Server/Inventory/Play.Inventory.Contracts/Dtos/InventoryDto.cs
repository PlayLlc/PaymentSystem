using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Inventory.Contracts.Dtos;

public record InventoryDto : IDto
{
    #region Instance Values

    [Required]
    public IEnumerable<StockItemDto> StockItems = new List<StockItemDto>();

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string StoreId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    #endregion
}