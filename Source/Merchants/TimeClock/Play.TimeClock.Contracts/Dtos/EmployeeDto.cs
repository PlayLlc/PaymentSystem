﻿using System.ComponentModel.DataAnnotations;

using Play.Domain;
using Play.Domain.Common.Attributes;

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
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public TimeClockDto TimeClock { get; set; } = null!;

    [Required]
    public IEnumerable<TimeEntryDto> TimeEntries { get; set; } = Array.Empty<TimeEntryDto>();

    #endregion
}