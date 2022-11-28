using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;
using Play.Globalization.Time;

namespace Play.Loyalty.Contracts.Dtosd;

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