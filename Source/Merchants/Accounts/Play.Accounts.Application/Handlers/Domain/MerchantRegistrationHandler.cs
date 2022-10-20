using Microsoft.Extensions.Logging;

using Play.Domain.Events;

using NServiceBus;

using Play.Accounts.Contracts.Events;
using Play.Accounts.Domain.Aggregates;
using Play.Domain.Repositories;

namespace Play.Accounts.Application.Handlers.Domain;

public class MerchantRegistrationHandler : DomainEventHandler, IHandleDomainEvents<MerchantRegistrationHasExpired>,
    IHandleDomainEvents<MerchantRejectedBecauseTheRegistrationPeriodExpired>, IHandleDomainEvents<MerchantRejectedBecauseOfProhibitedIndustry>,
    IHandleDomainEvents<MerchantRejectedBecauseItIsProhibited>, IHandleDomainEvents<MerchantRegistrationApproved>, IHandleDomainEvents<MerchantHasBeenCreated>,
    IHandleDomainEvents<MerchantRegistrationRejected>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;
    private readonly IRepository<Merchant, string> _MerchantRepository;
    private readonly IRepository<MerchantRegistration, string> _MerchantRegistrationRepository;

    #endregion

    #region Constructor

    public MerchantRegistrationHandler(IMessageHandlerContext messageHandler, ILogger<MerchantRegistrationHandler> logger) : base(logger)
    {
        _MessageHandlerContext = messageHandler;
        Subscribe((IHandleDomainEvents<MerchantRegistrationHasExpired>) this);
        Subscribe((IHandleDomainEvents<MerchantRejectedBecauseTheRegistrationPeriodExpired>) this);
        Subscribe((IHandleDomainEvents<MerchantRejectedBecauseOfProhibitedIndustry>) this);
        Subscribe((IHandleDomainEvents<MerchantRejectedBecauseItIsProhibited>) this);
    }

    #endregion

    #region Instance Members

    public async Task Handle(MerchantRegistrationHasExpired domainEvent)
    {
        Log(domainEvent);

        await _MessageHandlerContext.Publish<MerchantRegistrationWasRejectedEvent>((a) =>
            {
                a.MerchantRegistrationId = domainEvent.MerchantRegistrationId;
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(MerchantRejectedBecauseTheRegistrationPeriodExpired domainEvent)
    {
        Log(domainEvent);

        await _MessageHandlerContext.Publish<MerchantRegistrationWasRejectedEvent>((a) =>
            {
                a.MerchantRegistrationId = domainEvent.MerchantRegistrationId;
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(MerchantRejectedBecauseOfProhibitedIndustry domainEvent)
    {
        Log(domainEvent);

        await _MessageHandlerContext.Publish<MerchantRegistrationWasRejectedEvent>((a) =>
            {
                a.MerchantRegistrationId = domainEvent.MerchantRegistrationId;
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(MerchantRejectedBecauseItIsProhibited domainEvent)
    {
        Log(domainEvent);

        await _MessageHandlerContext.Publish<MerchantRegistrationWasRejectedEvent>((a) =>
            {
                a.MerchantRegistrationId = domainEvent.MerchantRegistrationId;
            })
            .ConfigureAwait(false);
    }

    /// <exception cref="Play.Domain.ValueObjects.ValueObjectException"></exception>
    /// <exception cref="Play.Domain.BusinessRuleValidationException"></exception>
    public async Task Handle(MerchantRegistrationApproved domainEvent)
    {
        Log(domainEvent);

        MerchantRegistration? merchantRegistration =
            await _MerchantRegistrationRepository.GetByIdAsync(domainEvent.MerchantRegistrationId).ConfigureAwait(false);

        Merchant merchant = merchantRegistration!.CreateMerchant();
        await _MerchantRepository.SaveAsync(merchant).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<MerchantHasBeenCreatedEvent>((a) =>
            {
                a.MerchantId = merchant.GetId();
            })
            .ConfigureAwait(false);
    }

    public Task Handle(MerchantHasBeenCreated domainEvent)
    {
        Log(domainEvent);

        return Task.CompletedTask;
    }

    public async Task Handle(MerchantRegistrationRejected domainEvent)
    {
        Log(domainEvent);
        await _MerchantRegistrationRepository.SaveAsync(domainEvent.MerchantRegistration).ConfigureAwait(false);
    }

    #endregion
}