using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Domain.Repositories;
using Play.Identity.Contracts;
using Play.Identity.Domain.Aggregates;

namespace Play.Identity.Application.Handlers;

public class MerchantHandler : DomainEventHandler, IHandleDomainEvents<MerchantHasBeenCreated>, IHandleDomainEvents<MerchantAddressHasBeenUpdated>,
    IHandleDomainEvents<MerchantBusinessInfoHasBeenUpdated>, IHandleDomainEvents<MerchantCategoryCodeIsProhibited>,
    IHandleDomainEvents<MerchantCompanyNameBeenUpdated>, IHandleDomainEvents<MerchantHasBeenRemoved>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;
    private readonly IRepository<Merchant, SimpleStringId> _MerchantRepository;

    #endregion

    #region Constructor

    public MerchantHandler(
        IMessageHandlerContext messageHandlerContext, IRepository<Merchant, SimpleStringId> merchantRepository, ILogger<MerchantHandler> logger) : base(logger)
    {
        _MessageHandlerContext = messageHandlerContext;
        _MerchantRepository = merchantRepository;
    }

    #endregion

    #region Instance Members

    public async Task Handle(MerchantHasBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _MerchantRepository.SaveAsync(domainEvent.Merchant).ConfigureAwait(false);

        // Broadcast that a new user has been created to Azure Service Bus
        await _MessageHandlerContext.Publish<MerchantHasBeenCreatedEvent>((a) =>
            {
                a.MerchantId = domainEvent.Merchant.GetId();
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(MerchantAddressHasBeenUpdated domainEvent)
    {
        Log(domainEvent);
        await _MerchantRepository.SaveAsync(domainEvent.Merchant).ConfigureAwait(false);
    }

    public async Task Handle(MerchantBusinessInfoHasBeenUpdated domainEvent)
    {
        Log(domainEvent);
        await _MerchantRepository.SaveAsync(domainEvent.Merchant).ConfigureAwait(false);
    }

    public async Task Handle(MerchantCategoryCodeIsProhibited domainEvent)
    {
        Log(domainEvent);
        await _MerchantRepository.SaveAsync(domainEvent.Merchant).ConfigureAwait(false);
    }

    public async Task Handle(MerchantCompanyNameBeenUpdated domainEvent)
    {
        Log(domainEvent);
        await _MerchantRepository.SaveAsync(domainEvent.Merchant).ConfigureAwait(false);
    }

    public async Task Handle(MerchantHasBeenRemoved domainEvent)
    {
        Log(domainEvent);
        SimpleStringId merchantId = domainEvent.Merchant.Id;
        await _MerchantRepository.RemoveAsync(domainEvent.Merchant).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<MerchantHasBeenRemovedEvent>((a) =>
            {
                a.MerchantId = merchantId;
            }, null)
            .ConfigureAwait(false);
    }

    #endregion
}