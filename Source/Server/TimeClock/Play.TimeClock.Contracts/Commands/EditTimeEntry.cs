using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Globalization.Time;

namespace Play.TimeClock.Contracts.Commands;

public record EditTimeEntry
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string TimeEntryId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [DateTimeUtc]
    public DateTime StartTime { get; set; }

    [Required]
    [DateTimeUtc]
    public DateTime EndTime { get; set; }

    #endregion
}