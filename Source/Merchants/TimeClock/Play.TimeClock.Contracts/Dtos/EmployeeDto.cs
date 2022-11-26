using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;
using Play.Domain.Common.Attributes;
using Play.Globalization.Time;
using Play.Inventory.Contracts.Dtos;

namespace Play.TimeClock.Contracts.Dtos;

public record EmployeeDto : IDto
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    public TimeClockDto TimeClock { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public string CompensationType { get; set; } = null!;

    #endregion
}

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

public record TimeEntryeDto : IDto
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