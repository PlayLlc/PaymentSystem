using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Events;
using Play.Loyalty.Contracts;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;

namespace Play.Loyalty.Application.Handlers.DomainEvents;

public partial class LoyaltyMemberHandler : DomainEventHandler, IHandleDomainEvents<LoyaltyMemberCreated>, IHandleDomainEvents<LoyaltyMemberRemoved>,
    IHandleDomainEvents<LoyaltyMemberUpdated>, IHandleDomainEvents<RewardsNumberIsNotUnique>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;
    private readonly ILoyaltyMemberRepository _LoyaltyMemberRepository;

    #endregion

    #region Constructor

    public LoyaltyMemberHandler(
        IMessageHandlerContext messageHandlerContext, ILoyaltyMemberRepository loyaltyMemberRepository, ILogger<LoyaltyMemberHandler> logger) : base(logger)
    {
        _MessageHandlerContext = messageHandlerContext;
        _LoyaltyMemberRepository = loyaltyMemberRepository;
    }

    #endregion

    #region Instance Members

    public async Task Handle(LoyaltyMemberCreated domainEvent)
    {
        Log(domainEvent);
        await _LoyaltyMemberRepository.SaveAsync(domainEvent.LoyaltyMember).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<LoyaltyMemberCreatedEvent>((a) =>
            {
                a.LoyaltyMember = domainEvent.LoyaltyMember.AsDto();
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(LoyaltyMemberRemoved domainEvent)
    {
        Log(domainEvent);
        await _LoyaltyMemberRepository.SaveAsync(domainEvent.LoyaltyMember).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<LoyaltyMemberRemovedEvent>((a) =>
            {
                a.LoyaltyMember = domainEvent.LoyaltyMember.AsDto();
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(LoyaltyMemberUpdated domainEvent)
    {
        Log(domainEvent);
        await _LoyaltyMemberRepository.SaveAsync(domainEvent.LoyaltyMember).ConfigureAwait(false);
        await _MessageHandlerContext.Publish<LoyaltyMemberUpdatedEvent>((a) =>
            {
                a.LoyaltyMember = domainEvent.LoyaltyMember.AsDto();
            })
            .ConfigureAwait(false);
    }

    public Task Handle(RewardsNumberIsNotUnique domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, $"\n\n\n\nWARNING: If we're having this problem we're doing pretty well");

        return Task.CompletedTask;
    }

    #endregion
}