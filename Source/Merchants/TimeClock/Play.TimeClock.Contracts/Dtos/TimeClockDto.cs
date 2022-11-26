using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;
using Play.Globalization.Time;

namespace Play.TimeClock.Contracts.Dtos;

public record TimeClockDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string EmployeeId { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string TimeClockStatus { get; set; } = string.Empty;

    [DateTimeUtc]
    public DateTime? ClockedInAt { get; set; }

    #endregion
}