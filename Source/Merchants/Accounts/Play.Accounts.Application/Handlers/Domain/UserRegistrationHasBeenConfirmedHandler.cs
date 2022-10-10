﻿using Play.Accounts.Domain.Aggregates.UserRegistration;
using Play.Domain.Events;

namespace Play.Accounts.Application.Handlers.Domain
{
    internal class UserRegistrationHasBeenConfirmedHandler : DomainEventHandler<UserRegistrationHasBeenConfirmedDomainEvent>
    {
        #region Instance Members

        public override DomainEventTypeId GetEventTypeId()
        {
            return UserRegistrationHasBeenConfirmedDomainEvent.DomainEventTypeId;
        }

        public override Task Handle(UserRegistrationHasBeenConfirmedDomainEvent domainEvent)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}