using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Dtos;

namespace Play.Identity.Contracts.Commands.Store;

/// <summary>
///     Create a new store for a Merchant
/// </summary>
public record CreateNewStore
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    public AddressDto Address { get; set; } = new AddressDto();

    #endregion
}