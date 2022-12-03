using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;

namespace Play.Payroll.Contracts.Commands;

public record CreateSemiMonthlyPaySchedule
{
    #region Instance Values

    /*
     *     string EmployerId;
    DayOfWeek DayOfTheWeek;
     */

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string EmployeeId { get; set; } = string.Empty;

    /// <summary>
    ///     The day of the week that employees receive their paycheck
    /// </summary>
    [Required]
    [DayOfTheMonth]
    public int FirstPaydayOfTheMonth { get; set; }

    /// <summary>
    ///     The day of the week that employees receive their paycheck
    /// </summary>
    [Required]
    [DayOfTheMonth]
    public int SecondPaydayOfTheMonth { get; set; }

    #endregion
}