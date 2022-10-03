using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Events;
using Play.Merchants.Onboarding.Domain.Aggregates.MerchantRegistration.Events;

namespace Play.Accounts.Application.Handlers.Domain
{
    internal class MerchantRegistrationCreatedHandler : DomainEventHandler<MerchantRegistrationCreatedDomainEvent>
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
}