using Play.Domain.Common.Attributes;
using Play.Messaging.NServiceBus;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Payroll.Contracts.Dtos;
using Play.Payroll.Domain.Aggregates;
using Play.Payroll.Domain.Entities;

namespace Play.Payroll.Contracts.NetworkEvents;

public class EmployeePaychecksHaveBeenCreatedEvent : NetworkEvent
{
    #region Instance Values

    [Required]
    public Employer Employer { get; set; } = null!;

    #endregion
}