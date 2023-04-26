﻿using Play.Domain.Events;
using Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents;

namespace Play.Registration.Application.Handlers.UserRegistration;

public partial class UserRegistrationHandler : DomainEventHandler, IHandleDomainEvents<UserRegistrationAddressUpdated>,
    IHandleDomainEvents<UserRegistrationContactInfoUpdated>, IHandleDomainEvents<UserRegistrationPersonalDetailsUpdated>
{
    #region Instance Members

    private static void SubscribeUpdatePartial(UserRegistrationHandler handler)
    {
        handler.Subscribe((IHandleDomainEvents<UserRegistrationAddressUpdated>) handler);
        handler.Subscribe((IHandleDomainEvents<UserRegistrationContactInfoUpdated>) handler);
        handler.Subscribe((IHandleDomainEvents<UserRegistrationPersonalDetailsUpdated>) handler);
    }

    public async Task Handle(UserRegistrationAddressUpdated domainEvent)
    {
        Log(domainEvent);

        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

    public async Task Handle(UserRegistrationContactInfoUpdated domainEvent)
    {
        Log(domainEvent);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

    public async Task Handle(UserRegistrationPersonalDetailsUpdated domainEvent)
    {
        Log(domainEvent);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

    #endregion
}