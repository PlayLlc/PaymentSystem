using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Dtos;

namespace Play.Identity.Contracts.Dtos;

public class StoreDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string StoreName { get; set; } = string.Empty;

    [Required]
    public AddressDto Address { get; set; } = new();

    #endregion
}