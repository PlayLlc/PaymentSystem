﻿using System.ComponentModel.DataAnnotations;

using Play.Accounts.Contracts.Dtos;

namespace Play.Accounts.Contracts.Commands;

public class UpdateUserRegistrationDetailsCommand
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

    /// <summary>
    ///     Personal details about the user
    /// </summary>
    [Required]
    public PersonalInfoDto PersonalInfo { get; set; } = new();

    #endregion
}