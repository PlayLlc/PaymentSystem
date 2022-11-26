using Play.Domain.Common.Attributes;
using Play.Inventory.Contracts.Dtos;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Globalization.Time;

namespace Play.TimeClock.Contracts.Commands;

public record CreateEmployee
{
    #region Instance Values

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string MerchantId { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public string UserId { get; set; } = string.Empty;

    #endregion
}

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