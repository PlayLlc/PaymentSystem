using System.ComponentModel.DataAnnotations;

using Play.Messaging.NServiceBus;
using Play.Payroll.Domain.Aggregates;

namespace Play.Payroll.Domain.NetworkEvents;

/// <summary>
///     A network event that is handled internally by this domain's message handlers
/// </summary>
public class EmployeePaychecksHaveBeenCreatedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    public Employer Employer { get; set; } = null!;

    #endregion
}