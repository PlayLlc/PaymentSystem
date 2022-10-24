﻿using System.ComponentModel.DataAnnotations;

using Play.Domain;

namespace Play.Accounts.Contracts.Dtos;

public class PasswordDto : IDto
{
    #region Instance Values

    [Required]
    [MinLength(1)]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string HashedPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public DateTime CreatedOn { get; set; }

    #endregion
}