using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Accounts.Contracts.Dtos;

public class UserDto : IDto
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
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