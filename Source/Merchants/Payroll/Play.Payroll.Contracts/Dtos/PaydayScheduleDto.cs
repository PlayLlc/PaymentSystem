using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;

namespace Play.Payroll.Contracts.Dtos;

public record PaydayScheduleDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string PaydayRecurrence { get; set; } = string.Empty;

    [DayOfTheWeek]
    public byte? WeeklyPayday { get; set; }

    [DayOfTheMonth]
    public byte? MonthlyPayday { get; set; }

    [DayOfTheMonth]
    public byte? SecondMonthlyPayday { get; set; }

    #endregion
}