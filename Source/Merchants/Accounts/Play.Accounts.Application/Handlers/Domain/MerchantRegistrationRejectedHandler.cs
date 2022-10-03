using Play.Domain.Events;
using Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration.Events;

namespace Play.Accounts.Application.Handlers.Domain;

internal class MerchantRegistrationRejectedHandler : DomainEventHandler<MerchantRegistrationCreatedDomainEvent>
{
    #region Instance Members

    public override DomainEventTypeId GetEventTypeId()
    {
        return MerchantRegistrationCreatedDomainEvent.DomainEventTypeId;
    }

    public override void Handle(MerchantRegistrationCreatedDomainEvent domainEvent)
    {
        throw new NotImplementedException();
    }

    #endregion
}