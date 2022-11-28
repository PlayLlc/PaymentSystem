using System.ComponentModel.DataAnnotations;

using Play.Domain.Common.Attributes;
using Play.Messaging.NServiceBus;
using Play.TimeClock.Contracts.Dtos;

namespace Play.TimeClock.Contracts.NetworkEvents;

public class EmployeeHasClockedOutEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    public EmployeeDto Employee { get; set; } = null!;

    [Required]
    [StringLength(20)]
    [AlphaNumericSpecial]
    public TimeEntryDto TimeEntry { get; set; } = null!;

    #endregion
}