using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Dtos;

namespace Play.Identity.Contracts.Dtos;

public class UserDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string TerminalId { get; set; } = string.Empty;

    [Required]
    public PasswordDto Password { get; set; } = new();

    [Required]
    public AddressDto Address { get; set; } = new();

    [Required]
    public ContactDto Contact { get; set; } = new();

    [Required]
    public PersonalDetailDto PersonalDetail { get; set; } = new();

    public bool IsActive { get; set; }

    #endregion
}