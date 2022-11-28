using Play.Domain.Common.Attributes;
using Play.Messaging.NServiceBus;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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