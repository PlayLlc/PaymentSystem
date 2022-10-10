using Play.Accounts.Domain.Aggregates.MerchantRegistration;
using Play.Domain.Events;

namespace Play.Accounts.Application.Handlers.Domain
{
    internal class MerchantRegistrationCreatedHandler : DomainEventHandler<MerchantRegistrationCreatedDomainEvent>
    {
        #region Instance Members

        public override DomainEventTypeId GetEventTypeId()
        {
            return MerchantRegistrationCreatedDomainEvent.DomainEventTypeId;
        }

        public override Task Handle(MerchantRegistrationCreatedDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}