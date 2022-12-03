using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.Payroll.Contracts.Commands;

public record CreateWeeklyPaySchedule
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string EmployeeId { get; set; } = string.Empty;

    /// <summary>
    ///     The day of the week that employees receive their paycheck
    /// </summary>
    [Required]
    [DayOfTheWeek]
    public byte DayOfTheWeek { get; set; }

    #endregion
}