using Play.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Play.Payroll.Domain.Aggregates;
using Play.Payroll.Domain.Repositories;

using NServiceBus;

using Play.Payroll.Contracts.NetworkEvents;
using Play.Payroll.Domain.Entities;
using Play.Payroll.Domain.Services;

namespace Play.Payroll.Application.Handlers.DomainEvents;

public partial class EmployerHandlers : DomainEventHandler, IHandleDomainEvents<EmployeePaychecksHaveBeenCreated>
{
    #region Instance Members

    public async Task Handle(EmployeePaychecksHaveBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _EmployerRepository.SaveAsync(domainEvent.Employer).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<EmployeePaychecksHaveBeenCreatedEvent>(a =>
            {
                a.Employer = domainEvent.Employer;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(EmployeePaychecksHaveBeenDelivered domainEvent)
    {
        Log(domainEvent);

        await _EmployerRepository.SaveAsync(domainEvent.Employer).ConfigureAwait(false);
    }

    #endregion
}