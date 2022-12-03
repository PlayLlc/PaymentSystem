using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Payroll.Contracts.Dtos;

namespace Play.Payroll.Contracts.Commands;

public record CutChecks
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string EmployerId { get; set; } = string.Empty;

    [Required]
    public PayPeriodDto PayPeriod { get; set; } = null!;

    #endregion
}