using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Globalization.Time;
using Play.Payroll.Contracts.Dtos;

namespace Play.Payroll.Contracts.Commands;

public record AddTimeEntry
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string EmployeeId { get; set; } = string.Empty;

    /// <summary>
    ///     The type of compensation the employee receives in either an hourly wage or an annual salary
    /// </summary>
    [Required]
    [MinLength(1)]
    public string TimeEntryType { get; set; } = string.Empty;

    /// <summary>
    ///     The time the employee began working
    /// </summary>
    [Required]
    [DateTimeUtc]
    public DateTime Start { get; set; }

    /// <summary>
    ///     The time the employee ended working
    /// </summary>
    [Required]
    [DateTimeUtc]
    public DateTime End { get; set; }

    #endregion
}

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