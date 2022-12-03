﻿using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;

namespace Play.Payroll.Contracts.Dtos;

public record DirectDepositDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string EmployeeId { get; set; } = string.Empty;

    [Required]
    public CheckingAccountDto CheckingAccount { get; set; } = null!;

    #endregion
}