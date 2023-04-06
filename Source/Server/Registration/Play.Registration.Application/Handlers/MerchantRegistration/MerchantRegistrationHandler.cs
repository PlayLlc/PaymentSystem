﻿using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Domain.Repositories;
using Play.Identity.Contracts;
using Play.Registration.Domain.Aggregates.MerchantRegistration;
using Play.Registration.Domain.Aggregates.MerchantRegistration.DomainEvents;
using Play.Registration.Domain.Aggregates.MerchantRegistration.DomainEvents.Rules;

namespace Play.Identity.Application.Handlers;

public class MerchantRegistrationHandler : DomainEventHandler, IHandleDomainEvents<MerchantRegistrationHasBeenRejected>,
    IHandleDomainEvents<MerchantRegistrationHasExpired>, IHandleDomainEvents<MerchantRegistrationHasNotBeenApproved>,
    IHandleDomainEvents<MerchantRegistrationHasBeenCreated>, IHandleDomainEvents<MerchantRegistrationApproved>
{
    #region Instance Values

    private readonly IMessageSession _MessageSession;
    private readonly IRepository<Merchant, SimpleStringId> _MerchantRepository;
    private readonly IRepository<MerchantRegistration, SimpleStringId> _MerchantRegistrationRepository;

    #endregion

    #region Constructor

    public MerchantRegistrationHandler(
        IMessageSession messageHandler, IRepository<Merchant, SimpleStringId> merchantRepository,
        IRepository<MerchantRegistration, SimpleStringId> merchantRegistrationRepository, ILogger<MerchantRegistrationHandler> logger) : base(logger)
    {
        _MessageSession = messageHandler;
        _MerchantRepository = merchantRepository;
        _MerchantRegistrationRepository = merchantRegistrationRepository;

        Subscribe((IHandleDomainEvents<MerchantRegistrationHasBeenRejected>) this);
        Subscribe((IHandleDomainEvents<MerchantRegistrationHasExpired>) this);
        Subscribe((IHandleDomainEvents<MerchantRegistrationHasNotBeenApproved>) this);
        Subscribe((IHandleDomainEvents<MerchantRegistrationHasBeenCreated>) this);
        Subscribe((IHandleDomainEvents<MerchantRegistrationApproved>) this);
    }

    #endregion

    #region Instance Members

    public async Task Handle(MerchantRegistrationHasExpired domainEvent)
    {
        Log(domainEvent);
        await _MerchantRegistrationRepository.SaveAsync(domainEvent.MerchantRegistration).ConfigureAwait(false);
    }

    public async Task Handle(MerchantRegistrationHasBeenRejected domainEvent)
    {
        Log(domainEvent);
        await _MerchantRegistrationRepository.SaveAsync(domainEvent.MerchantRegistration).ConfigureAwait(false);
    }

    public Task Handle(MerchantRegistrationHasNotBeenApproved domainEvent)
    {
        Log(domainEvent);

        return Task.CompletedTask;
    }

    public async Task Handle(MerchantHasBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _MerchantRepository.SaveAsync(domainEvent.Merchant).ConfigureAwait(false);

        // Network Event sent through NServiceBus 
        await _MessageSession.Publish<MerchantHasBeenCreatedEvent>(a =>
            {
                a.MerchantId = domainEvent.Merchant.GetId();
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(MerchantRegistrationApproved domainEvent)
    {
        Log(domainEvent);
        await _MerchantRegistrationRepository.SaveAsync(domainEvent.MerchantRegistration).ConfigureAwait(false);
        await _MerchantRepository.SaveAsync(domainEvent.MerchantRegistration.CreateMerchant()).ConfigureAwait(false);
    }

    public async Task Handle(MerchantRegistrationHasBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _MerchantRegistrationRepository.SaveAsync(domainEvent.MerchantRegistration).ConfigureAwait(false);
    }

    #endregion
}