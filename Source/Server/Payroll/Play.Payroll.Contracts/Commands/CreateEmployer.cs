﻿using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.Payroll.Contracts.Commands;

public record CreateEmployer
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string PaydayRecurrence { get; set; } = string.Empty;

    [Range(0, 7)]
    public byte? WeeklyPayday { get; set; }

    [Range(0, 31)]
    public byte? MonthlyPayday { get; set; }

    [Range(0, 31)]
    public byte? SecondMonthlyPayday { get; set; }

    #endregion
}