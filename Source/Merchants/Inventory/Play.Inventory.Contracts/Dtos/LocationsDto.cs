using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.ValueObjects;

namespace Play.Inventory.Contracts.Dtos;

public record LocationsDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    public bool AllLocations { get; set; }

    [Required]
    public IEnumerable<SimpleStringId> StoreIds { get; set; } = new List<SimpleStringId>();

    #endregion
}