using Play.Accounts.Domain.Aggregates.UserRegistration;
using Play.Domain.Events;

namespace Play.Accounts.Application.Handlers.Domain
{
    internal class UserRegistrationCreatedHandler : DomainEventHandler<UserRegistrationCreatedDomainEvent>
    {
        #region Instance Members

        public override DomainEventTypeId GetEventTypeId()
        {
            return UserRegistrationCreatedDomainEvent.DomainEventTypeId;
        }

        public override Task Handle(UserRegistrationCreatedDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}