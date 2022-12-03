using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Globalization.Time;

namespace Play.Payroll.Contracts.Commands;

public record CreateMonthlyPaySchedule
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string EmployeeId { get; set; } = string.Empty;

    /// <summary>
    ///     The day of the month that employees receive their paycheck
    /// </summary>
    [Required]
    [DayOfTheMonth]
    public byte Payday { get; set; }

    #endregion
}