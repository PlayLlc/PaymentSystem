using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.Payroll.Contracts.Commands;

public record UpdatePaydaySchedule
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string EmployerId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string RecurrenceType { get; set; } = string.Empty;

    #endregion
}