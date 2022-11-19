﻿using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Domain.Repositories;
using Play.Identity.Contracts.Events;
using Play.Identity.Domain.Aggregates;

namespace Play.Identity.Application.Handlers;

public partial class MerchantRegistrationHandler : DomainEventHandler, IHandleDomainEvents<MerchantRegistrationHasBeenRejected>,
    IHandleDomainEvents<MerchantRegistrationHasExpired>, IHandleDomainEvents<MerchantRegistrationHasNotBeenApproved>,
    IHandleDomainEvents<MerchantRegistrationHasBeenCreated>, IHandleDomainEvents<MerchantRegistrationApproved>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;
    private readonly IRepository<Merchant, SimpleStringId> _MerchantRepository;
    private readonly IRepository<MerchantRegistration, SimpleStringId> _MerchantRegistrationRepository;

    #endregion

    #region Constructor

    public MerchantRegistrationHandler(
        IMessageHandlerContext messageHandler, IRepository<Merchant, SimpleStringId> merchantRepository,
        IRepository<MerchantRegistration, SimpleStringId> merchantRegistrationRepository, ILogger<MerchantRegistrationHandler> logger) : base(logger)
    {
        _MessageHandlerContext = messageHandler;
        _MerchantRepository = merchantRepository;
        _MerchantRegistrationRepository = merchantRegistrationRepository;
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
        await _MessageHandlerContext.Publish<MerchantHasBeenCreatedEvent>((a) =>
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