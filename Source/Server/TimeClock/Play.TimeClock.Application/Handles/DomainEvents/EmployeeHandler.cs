using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Events;
using Play.TimeClock.Contracts.NetworkEvents;
using Play.TimeClock.Domain.Aggregates;
using Play.TimeClock.Domain.Repositories;

namespace Play.TimeClock.Application.Handles.DomainEvents;

public class EmployeeHandler : DomainEventHandler, IHandleDomainEvents<EmployeeAlreadyExists>, IHandleDomainEvents<EmployeeHasBeenCreated>,
    IHandleDomainEvents<EmployeeTimeEntryHasBeenEdited>, IHandleDomainEvents<EmployeeHasBeenRemoved>, IHandleDomainEvents<EmployeeHasClockedIn>,
    IHandleDomainEvents<EmployeeHasClockedOut>, IHandleDomainEvents<EmployeeWasNotClockedIn>, IHandleDomainEvents<EmployeeWasNotClockedOut>,
    IHandleDomainEvents<UnauthorizedUserAttemptedToUpdateEmployeeTimeClock>, IHandleDomainEvents<AggregateUpdateWasAttemptedByUnknownUser<Employee>>,
    IHandleDomainEvents<DeactivatedMerchantAttemptedToCreateAggregate<Employee>>, IHandleDomainEvents<DeactivatedUserAttemptedToUpdateAggregate<Employee>>

{
    #region Instance Values

    private readonly IMessageSession _MessageSession;
    private readonly IEmployeeRepository _EmployeeRepository;

    #endregion

    #region Constructor

    public EmployeeHandler(ILogger logger, IMessageSession messageSession, IEmployeeRepository employeeRepository) : base(logger)
    {
        _MessageSession = messageSession;
        _EmployeeRepository = employeeRepository;

        Subscribe((IHandleDomainEvents<EmployeeAlreadyExists>) this);
        Subscribe((IHandleDomainEvents<EmployeeHasBeenCreated>) this);
        Subscribe((IHandleDomainEvents<EmployeeTimeEntryHasBeenEdited>) this);
        Subscribe((IHandleDomainEvents<EmployeeHasBeenRemoved>) this);
        Subscribe((IHandleDomainEvents<EmployeeHasClockedIn>) this);
        Subscribe((IHandleDomainEvents<EmployeeHasClockedOut>) this);
        Subscribe((IHandleDomainEvents<EmployeeWasNotClockedIn>) this);
        Subscribe((IHandleDomainEvents<EmployeeWasNotClockedOut>) this);
        Subscribe((IHandleDomainEvents<UnauthorizedUserAttemptedToUpdateEmployeeTimeClock>) this);
        Subscribe((IHandleDomainEvents<AggregateUpdateWasAttemptedByUnknownUser<Employee>>) this);
        Subscribe((IHandleDomainEvents<DeactivatedMerchantAttemptedToCreateAggregate<Employee>>) this);
        Subscribe((IHandleDomainEvents<DeactivatedUserAttemptedToUpdateAggregate<Employee>>) this);
    }

    #endregion

    #region Instance Members

    public async Task Handle(EmployeeHasBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _EmployeeRepository.SaveAsync(domainEvent.Employee).ConfigureAwait(false);
    }

    public async Task Handle(EmployeeHasClockedIn domainEvent)
    {
        Log(domainEvent);
        await _EmployeeRepository.SaveAsync(domainEvent.Employee).ConfigureAwait(false);
    }

    public async Task Handle(EmployeeHasClockedOut domainEvent)
    {
        Log(domainEvent);
        await _EmployeeRepository.SaveAsync(domainEvent.Employee).ConfigureAwait(false);

        await _MessageSession.Publish<EmployeeHasClockedOutEvent>(a =>
            {
                a.Employee = domainEvent.Employee.AsDto();
                a.TimeEntry = domainEvent.TimeEntry.AsDto();
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(EmployeeHasBeenRemoved domainEvent)
    {
        Log(domainEvent);
        await _EmployeeRepository.RemoveAsync(domainEvent.Employee).ConfigureAwait(false);
    }

    public Task Handle(UnauthorizedUserAttemptedToUpdateEmployeeTimeClock domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: There is likely an error in the client integration");

        return Task.CompletedTask;
    }

    public Task Handle(AggregateUpdateWasAttemptedByUnknownUser<Employee> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    public Task Handle(DeactivatedMerchantAttemptedToCreateAggregate<Employee> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    public Task Handle(DeactivatedUserAttemptedToUpdateAggregate<Employee> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    public Task Handle(EmployeeWasNotClockedIn domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    public Task Handle(EmployeeWasNotClockedOut domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    public Task Handle(EmployeeAlreadyExists domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    public async Task Handle(EmployeeTimeEntryHasBeenEdited domainEvent)
    {
        Log(domainEvent);

        await _EmployeeRepository.RemoveAsync(domainEvent.Employee).ConfigureAwait(false);

        await _MessageSession.Publish<EmployeeTimeEntryHasBeenEditedEvent>(a =>
            {
                a.Employee = domainEvent.Employee.AsDto();
                a.TimeEntry = domainEvent.TimeEntry.AsDto();
                a.UserId = domainEvent.UserId;
            })
            .ConfigureAwait(false);
    }

    #endregion
}