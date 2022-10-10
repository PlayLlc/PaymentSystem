using Play.Accounts.Domain.Aggregates.UserRegistration;
using Play.Domain.Events;

namespace Play.Accounts.Application.Handlers.Domain
{
    internal class UserRegistrationHasExpiredHandler : DomainEventHandler<UserRegistrationHasExpiredDomainEvent>
    {
        #region Instance Members

        public override DomainEventTypeId GetEventTypeId()
        {
            return UserRegistrationHasExpiredDomainEvent.DomainEventTypeId;
        }

        public override Task Handle(UserRegistrationHasExpiredDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}