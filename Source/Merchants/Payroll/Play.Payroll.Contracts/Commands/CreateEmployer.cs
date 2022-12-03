using Play.Domain.Common.Attributes;
using Play.Domain.Common.Dtos;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Globalization.Time;

namespace Play.Payroll.Contracts.Commands;

public record CreateEmployer
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string MerchantId { get; set; } = string.Empty;

    #endregion
}

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

public record UpdateEmployeeCompensation
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
    public string CompensationType { get; set; } = string.Empty;

    /// <summary>
    ///     The hourly or salary wage that the employee earns
    /// </summary>
    [Required]
    public MoneyDto CompensationRate { get; set; } = null!;

    #endregion
}

public record CreateEmployee
{
    #region Instance Values

    [Required]
    [AlphaNumericSpecial]
    [StringLength(20)]
    public string UserId { get; set; } = string.Empty;

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
    public int DayOfTheWeek { get; set; }

    #endregion
}

public record CreateBiweeklyPaySchedule
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
    public int DayOfTheWeek { get; set; }

    #endregion
}

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

public record CreateMonthlyPaySchedule
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
    public int DayOfTheMonth { get; set; }

    #endregion
}