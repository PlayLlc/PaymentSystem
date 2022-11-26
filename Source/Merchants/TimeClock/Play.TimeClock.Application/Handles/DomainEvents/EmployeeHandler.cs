using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Events;
using Play.TimeClock.Contracts.NetworkEvents;
using Play.TimeClock.Domain.Aggregates._Shared.DomainEvents;
using Play.TimeClock.Domain.Aggregates.Employees;
using Play.TimeClock.Domain.Aggregates.Employees.DomainEvents;
using Play.TimeClock.Domain.Repositories;

namespace Play.TimeClock.Application.Handles.DomainEvents
{
    public class EmployeeHandler : DomainEventHandler, IHandleDomainEvents<EmployeeHasClockedIn>, IHandleDomainEvents<EmployeeWasNotClockedIn>,
        IHandleDomainEvents<EmployeeWasNotClockedOut>, IHandleDomainEvents<UnauthorizedUserAttemptedToUpdateEmployeeTimeClock>,
        IHandleDomainEvents<AggregateUpdateWasAttemptedByUnknownUser<Employee>>, IHandleDomainEvents<DeactivatedMerchantAttemptedToCreateAggregate<Employee>>,
        IHandleDomainEvents<DeactivatedUserAttemptedToUpdateAggregate<Employee>>
    {
        #region Instance Values

        private readonly IMessageHandlerContext _MessageHandlerContext;
        private readonly IEmployeeRepository _EmployeeRepository;

        #endregion

        #region Constructor

        public EmployeeHandler(ILogger logger, IMessageHandlerContext messageHandlerContext) : base(logger)
        {
            _MessageHandlerContext = messageHandlerContext;
        }

        #endregion

        #region Instance Members

        public async Task Handle(EmployeeHasClockedIn domainEvent)
        {
            Log(domainEvent);
            await _EmployeeRepository.SaveAsync(domainEvent.Employee).ConfigureAwait(false);
        }

        public async Task Handle(EmployeeHasClockedOut domainEvent)
        {
            Log(domainEvent);
            await _EmployeeRepository.SaveAsync(domainEvent.Employee).ConfigureAwait(false);

            await _MessageHandlerContext.Publish<EmployeeHasClockedOutEvent>((a) =>
                {
                    a.Employee = domainEvent.Employee.AsDto();
                    a.TimeEntry = domainEvent.TimeEntry.AsDto();
                })
                .ConfigureAwait(false);
        }

        public Task Handle(EmployeeWasNotClockedIn domainEvent) => throw new NotImplementedException();

        public Task Handle(EmployeeWasNotClockedOut domainEvent) => throw new NotImplementedException();

        public Task Handle(UnauthorizedUserAttemptedToUpdateEmployeeTimeClock domainEvent) => throw new NotImplementedException();

        public Task Handle(AggregateUpdateWasAttemptedByUnknownUser<Employee> domainEvent) => throw new NotImplementedException();

        public Task Handle(DeactivatedMerchantAttemptedToCreateAggregate<Employee> domainEvent) => throw new NotImplementedException();

        public Task Handle(DeactivatedUserAttemptedToUpdateAggregate<Employee> domainEvent) => throw new NotImplementedException();

        #endregion
    }
}