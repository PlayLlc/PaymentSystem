using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;
using Play.Globalization.Time;

namespace Play.TimeClock.Contracts.Dtos;

public record TimeEntryDto : IDto
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
    [DateTimeUtc]
    public DateTime StartTime { get; set; }

    [Required]
    [DateTimeUtc]
    public DateTime EndTime { get; set; }

    #endregion
}