using System.ComponentModel.DataAnnotations;

using Play.Accounts.Contracts.Dtos;

namespace Play.Accounts.Contracts.Commands;

public class RegisterUserRequest
{
    #region Instance Values

    /// <summary>
    ///     The home address of the user
    /// </summary>
    [Required]
    public AddressDto AddressDto { get; set; } = new();

    /// <summary>
    ///     The personal contact info of the user
    /// </summary>
    [Required]
    public ContactDto ContactDto { get; set; } = new();

    [Required]
    public PersonalInfoDto PersonalInfoDto { get; set; } = new();

    /// <summary>
    ///     Passwords must be at least 8 characters containing numeric, alphabetic, and special characters
    /// </summary>
    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;

    #endregion
}