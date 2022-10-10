using NServiceBus;

using Play.Accounts.Application.Events;
using Play.Accounts.Domain.Aggregates.MerchantRegistration;
using Play.Domain.Events;

namespace Play.Accounts.Application.Handlers.Domain;

internal class MerchantRegistrationConfirmedHandler : DomainEventHandler<MerchantRegistrationConfirmedDomainEvent>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;

    #endregion

    #region Constructor

    public MerchantRegistrationConfirmedHandler(IMessageHandlerContext messageHandlerContext)
    {
        _MessageHandlerContext = messageHandlerContext;
    }

    #endregion

    #region Instance Members

    public override DomainEventTypeId GetEventTypeId()
    {
        return MerchantRegistrationConfirmedDomainEvent.DomainEventTypeId;
    }

    public override async Task Handle(MerchantRegistrationConfirmedDomainEvent domainEvent)
    {
        // logic 

        // logging

        await _MessageHandlerContext.Publish<MerchantRegistrationConfirmedEvent>((a) =>
            {
                a.MerchantRegistrationId = domainEvent.Id;
            })
            .ConfigureAwait(false);
    }

    #endregion
}