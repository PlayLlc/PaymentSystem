using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Dtos;

namespace Play.Identity.Contracts.Commands;

/// <summary>
///     Update the address of the intended resource
/// </summary>
public record UpdateAddressCommand
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string Id { get; set; } = string.Empty;

    [Required]
    public AddressDto Address { get; set; } = new();

    #endregion
}