using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Domain.Common.Dtos;
using Play.Globalization.Time;

namespace Play.Payroll.Contracts.Commands;

public record UpdateEmployeeCompensation
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string EmployeeId { get; set; } = string.Empty;

    /// <summary>
    ///     The type of compensation the employee receives in either an hourly wage or an annual salary
    /// </summary>
    [Required]
    [MinLength(1)]
    public string CompensationType { get; set; } = string.Empty;

    /// <summary>
    ///     The hourly or salary wage that the employee earns
    /// </summary>
    [Required]
    public MoneyDto CompensationRate { get; set; } = null!;

    #endregion
}

public record UpdateTimeEntry
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string EmployeeId { get; set; } = string.Empty;

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string TimeEntryId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string TimeEntryType { get; set; } = string.Empty;

    [Required]
    [DateTimeUtc]
    public DateTime Start { get; set; }

    [Required]
    [DateTimeUtc]
    public DateTime End { get; set; }

    #endregion
}