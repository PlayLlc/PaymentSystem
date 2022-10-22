using Microsoft.Extensions.Logging;

using Play.Domain.Events;

using NServiceBus;

using Play.Accounts.Contracts.Events;
using Play.Accounts.Domain.Aggregates;
using Play.Domain.Repositories;

namespace Play.Accounts.Application.Handlers.Domain;

public class MerchantRegistrationHandler : DomainEventHandler, IHandleDomainEvents<MerchantRegistrationHasExpired>,
    IHandleDomainEvents<MerchantRegistrationHasBeenRejected>, IHandleDomainEvents<MerchantRegistrationHasNotBeenApproved>,
    IHandleDomainEvents<MerchantHasBeenCreated>, IHandleDomainEvents<MerchantRegistrationApproved>, IHandleDomainEvents<MerchantRegistrationRejected>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;
    private readonly IRepository<Merchant, string> _MerchantRepository;
    private readonly IRepository<MerchantRegistration, string> _MerchantRegistrationRepository;

    #endregion

    #region Constructor

    public MerchantRegistrationHandler(
        IMessageHandlerContext messageHandler, IRepository<Merchant, string> merchantRepository,
        IRepository<MerchantRegistration, string> merchantRegistrationRepository, ILogger<MerchantRegistrationHandler> logger) : base(logger)
    {
        _MessageHandlerContext = messageHandler;
        _MerchantRepository = merchantRepository;
        _MerchantRegistrationRepository = merchantRegistrationRepository;

        // TODO: Find a way to automate this so there's not a runtime exception
        Subscribe((IHandleDomainEvents<MerchantRegistrationHasExpired>) this);
        Subscribe((IHandleDomainEvents<MerchantRegistrationHasBeenRejected>) this);
        Subscribe((IHandleDomainEvents<MerchantRegistrationHasNotBeenApproved>) this);
        Subscribe((IHandleDomainEvents<MerchantHasBeenCreated>) this);
        Subscribe((IHandleDomainEvents<MerchantRegistrationApproved>) this);
        Subscribe((IHandleDomainEvents<MerchantRegistrationRejected>) this);
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

        // Network Event sent through NServiceBus using Azure Service Bus
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

    public async Task Handle(MerchantRegistrationCreated domainEvent)
    {
        Log(domainEvent);
        await _MerchantRegistrationRepository.SaveAsync(domainEvent.MerchantRegistration).ConfigureAwait(false);
    }

    public async Task Handle(MerchantRegistrationRejected domainEvent)
    {
        Log(domainEvent);
        await _MerchantRegistrationRepository.SaveAsync(domainEvent.MerchantRegistration).ConfigureAwait(false);
    }

    #endregion
}