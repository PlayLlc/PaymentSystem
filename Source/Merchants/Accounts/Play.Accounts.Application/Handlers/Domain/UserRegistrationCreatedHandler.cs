using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.Events;
using Play.Merchants.Onboarding.Domain.Aggregates;

namespace Play.Accounts.Application.Handlers.Domain
{
    internal class UserRegistrationCreatedHandler : DomainEventHandler<UserRegistrationCreatedDomainEvent>
    {
        #region Instance Members

        public override DomainEventTypeId GetEventTypeId()
        {
            return UserRegistrationCreatedDomainEvent.DomainEventTypeId;
        }

        public override void Handle(UserRegistrationCreatedDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}