using Microsoft.Extensions.Logging;

using Play.Domain.Events;
using Play.Payroll.Domain.Aggregates;
using Play.Payroll.Domain.NetworkEvents;

namespace Play.Payroll.Application.Handlers.DomainEvents;

public partial class EmployerHandlers : DomainEventHandler, IHandleDomainEvents<PaychecksHaveBeenCreated>, IHandleDomainEvents<PaychecksHaveBeenDelivered>,
    IHandleDomainEvents<PayPeriodHasNotEnded>
{
    #region Instance Members

    public async Task Handle(PaychecksHaveBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _EmployerRepository.SaveAsync(domainEvent.Employer).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<EmployeePaychecksHaveBeenCreatedEvent>(a =>
            {
                a.Employer = domainEvent.Employer;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(PaychecksHaveBeenDelivered domainEvent)
    {
        Log(domainEvent);
        await _EmployerRepository.SaveAsync(domainEvent.Employer).ConfigureAwait(false);
    }

    public Task Handle(PayPeriodHasNotEnded domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    #endregion
}