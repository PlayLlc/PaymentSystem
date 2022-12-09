using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Globalization.Time;

namespace Play.Payroll.Contracts.Commands;

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