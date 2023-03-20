using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Events;
using Play.Payroll.Domain.Aggregates;
using Play.Payroll.Domain.Repositories;

namespace Play.Payroll.Application.Handlers.DomainEvents;

public partial class EmployerHandlers : DomainEventHandler, IHandleDomainEvents<EmployerHasBeenCreated>, IHandleDomainEvents<EmployerHasBeenRemoved>,
    IHandleDomainEvents<EmployerHasUndeliveredPaychecks>, IHandleDomainEvents<AggregateUpdateWasAttemptedByUnknownUser<Employer>>,
    IHandleDomainEvents<DeactivatedMerchantAttemptedToCreateAggregate<Employer>>, IHandleDomainEvents<DeactivatedUserAttemptedToUpdateAggregate<Employer>>
{
    #region Instance Values

    private readonly IMessageSession _MessageSession;
    private readonly IEmployerRepository _EmployerRepository;

    #endregion

    #region Constructor

    public EmployerHandlers(ILogger logger, IMessageSession messageSession, IEmployerRepository employerRepository) : base(logger)
    {
        _MessageSession = messageSession;
        _EmployerRepository = employerRepository;
        SubscribeEmployeesPartial(this);
        SubscribePaychecksPartial(this);
        Subscribe((IHandleDomainEvents<EmployerHasBeenCreated>) this);
        Subscribe((IHandleDomainEvents<EmployerHasBeenRemoved>) this);
        Subscribe((IHandleDomainEvents<EmployerHasUndeliveredPaychecks>) this);
        Subscribe((IHandleDomainEvents<AggregateUpdateWasAttemptedByUnknownUser<Employer>>) this);
        Subscribe((IHandleDomainEvents<DeactivatedMerchantAttemptedToCreateAggregate<Employer>>) this);
        Subscribe((IHandleDomainEvents<DeactivatedUserAttemptedToUpdateAggregate<Employer>>) this);
    }

    #endregion

    #region Instance Members

    public Task Handle(AggregateUpdateWasAttemptedByUnknownUser<Employer> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The User is not associated with the specified Merchant");

        return Task.CompletedTask;
    }

    public Task Handle(DeactivatedMerchantAttemptedToCreateAggregate<Employer> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The Merchant is deactivated and should not be authorized to use this capability");

        return Task.CompletedTask;
    }

    public Task Handle(DeactivatedUserAttemptedToUpdateAggregate<Employer> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The User is deactivated and should not be authorized to use this capability");

        return Task.CompletedTask;
    }

    public async Task Handle(EmployerHasBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _EmployerRepository.SaveAsync(domainEvent.Employer).ConfigureAwait(false);
    }

    public async Task Handle(EmployerHasBeenRemoved domainEvent)
    {
        Log(domainEvent);
        await _EmployerRepository.SaveAsync(domainEvent.Employer).ConfigureAwait(false);
    }

    public Task Handle(EmployerHasUndeliveredPaychecks domainEvent)
    {
        Log(domainEvent);

        return Task.CompletedTask;
    }

    #endregion
}