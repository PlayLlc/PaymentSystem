﻿using System.ComponentModel.DataAnnotations;

using Play.Accounts.Contracts.Dtos;

namespace Play.Accounts.Contracts.Commands.MerchantRegistration;

public record CreateMerchantRegistrationCommand
{
    #region Instance Values

    [Required]
    public UserDto User { get; set; } = new();

    [Required]
    [MinLength(1)]
    public string Name { get; set; } = string.Empty;

    #endregion
}