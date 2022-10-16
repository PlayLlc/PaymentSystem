using Microsoft.Extensions.Logging;

using Play.Accounts.Domain.Aggregates.MerchantRegistration;
using Play.Domain.Events;

using NServiceBus;

using Play.Accounts.Contracts.Events;

namespace Play.Accounts.Application.Handlers.Domain;

public class MerchantRegistrationHandler : DomainEventHandler, IHandleDomainEvents<MerchantRegistrationHasExpired>,
    IHandleDomainEvents<MerchantRejectedBecauseTheRegistrationPeriodExpired>, IHandleDomainEvents<MerchantRejectedBecauseOfProhibitedIndustry>,
    IHandleDomainEvents<MerchantRejectedBecauseItIsProhibited>, IHandleDomainEvents<MerchantRegistrationConfirmedDomainEvent>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;

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

    public async Task Handle(MerchantRegistrationConfirmedDomainEvent domainEvent)
    {
        Log(domainEvent);

        await _MessageHandlerContext.Publish<MerchantRegistrationWasRejectedEvent>((a) =>
            {
                a.MerchantRegistrationId = domainEvent.MerchantRegistrationId;
            })
            .ConfigureAwait(false);
    }

    #endregion
}