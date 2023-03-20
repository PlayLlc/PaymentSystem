using Microsoft.Extensions.Logging;

using Play.Domain.Events;
using Play.Payroll.Domain.Aggregates;

namespace Play.Payroll.Application.Handlers.DomainEvents;

public partial class EmployerHandlers : DomainEventHandler, IHandleDomainEvents<EmployeeDoesNotExist>, IHandleDomainEvents<EmployeeHasBeenCreated>,
    IHandleDomainEvents<EmployeeHasBeenRemoved>, IHandleDomainEvents<EmployeeHasUndeliveredPaychecks>

{
    #region Instance Members

    private static void SubscribeEmployeesPartial(EmployerHandlers handler)
    {
        handler.Subscribe((IHandleDomainEvents<EmployeeDoesNotExist>) handler);
        handler.Subscribe((IHandleDomainEvents<EmployeeHasBeenCreated>) handler);
        handler.Subscribe((IHandleDomainEvents<EmployeeHasBeenRemoved>) handler);
        handler.Subscribe((IHandleDomainEvents<EmployeeHasUndeliveredPaychecks>) handler);
    }

    public async Task Handle(EmployeeHasBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _EmployerRepository.SaveAsync(domainEvent.Employer).ConfigureAwait(false);
    }

    public async Task Handle(EmployeeHasBeenRemoved domainEvent)
    {
        Log(domainEvent);
        await _EmployerRepository.SaveAsync(domainEvent.Employer).ConfigureAwait(false);
    }

    public Task Handle(EmployeeHasUndeliveredPaychecks domainEvent)
    {
        Log(domainEvent);

        return Task.CompletedTask;
    }

    public Task Handle(EmployeeDoesNotExist domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    #endregion
}