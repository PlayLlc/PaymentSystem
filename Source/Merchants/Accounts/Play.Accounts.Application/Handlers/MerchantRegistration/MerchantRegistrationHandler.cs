using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Accounts.Contracts.Events;
using Play.Accounts.Domain.Aggregates;
using Play.Domain.Events;
using Play.Domain.Repositories;

namespace Play.Accounts.Application.Handlers;

public partial class MerchantRegistrationHandler : DomainEventHandler, IHandleDomainEvents<MerchantRegistrationHasBeenRejected>,
    IHandleDomainEvents<MerchantRegistrationHasExpired>, IHandleDomainEvents<MerchantRegistrationHasNotBeenApproved>,
    IHandleDomainEvents<MerchantRegistrationHasBeenCreated>, IHandleDomainEvents<MerchantRegistrationApproved>
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

    public async Task Handle(MerchantRegistrationHasBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _MerchantRegistrationRepository.SaveAsync(domainEvent.MerchantRegistration).ConfigureAwait(false);
    }

    #endregion
}