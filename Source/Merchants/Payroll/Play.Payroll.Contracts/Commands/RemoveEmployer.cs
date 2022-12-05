using Play.Domain.Common.Attributes;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Globalization.Time;

namespace Play.Payroll.Contracts.Commands;

public record RemoveEmployer
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

    #endregion
}

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