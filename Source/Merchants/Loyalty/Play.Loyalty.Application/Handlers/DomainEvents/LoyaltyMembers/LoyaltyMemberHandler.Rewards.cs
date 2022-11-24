using Microsoft.Extensions.Logging;

using Play.Domain.Events;
using Play.Loyalty.Contracts;
using Play.Loyalty.Domain.Aggregates;

namespace Play.Loyalty.Application.Handlers.DomainEvents;

public partial class LoyaltyMemberHandler : DomainEventHandler, IHandleDomainEvents<LoyaltyMemberEarnedPoints>, IHandleDomainEvents<LoyaltyMemberEarnedRewards>,
    IHandleDomainEvents<LoyaltyMemberLostPoints>, IHandleDomainEvents<LoyaltyMemberLostRewards>, IHandleDomainEvents<RewardBalanceIsInsufficient>,
    IHandleDomainEvents<RewardProgramIsNotActive>
{
    #region Instance Members

    public async Task Handle(LoyaltyMemberEarnedPoints domainEvent)
    {
        Log(domainEvent);
        await _LoyaltyMemberRepository.SaveAsync(domainEvent.Member).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<LoyaltyMemberEarnedPointsEvent>((a) =>
            {
                a.LoyaltyMember = domainEvent.Member.AsDto();
                a.TransactionId = domainEvent.TransactionId;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(LoyaltyMemberEarnedRewards domainEvent)
    {
        Log(domainEvent);
        await _LoyaltyMemberRepository.SaveAsync(domainEvent.Member).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<LoyaltyMemberEarnedRewardsEvent>((a) =>
            {
                a.LoyaltyMember = domainEvent.Member.AsDto();
                a.TransactionId = domainEvent.TransactionId;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(LoyaltyMemberLostPoints domainEvent)
    {
        Log(domainEvent);
        await _LoyaltyMemberRepository.SaveAsync(domainEvent.Member).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<LoyaltyMemberLostPointsEvent>((a) =>
            {
                a.LoyaltyMember = domainEvent.Member.AsDto();
                a.TransactionId = domainEvent.TransactionId;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(LoyaltyMemberLostRewards domainEvent)
    {
        Log(domainEvent);
        await _LoyaltyMemberRepository.SaveAsync(domainEvent.Member).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<LoyaltyMemberLostPointsEvent>((a) =>
            {
                a.LoyaltyMember = domainEvent.Member.AsDto();
                a.TransactionId = domainEvent.TransactionId;
            }, null)
            .ConfigureAwait(false);
    }

    public Task Handle(RewardBalanceIsInsufficient domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, $"\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    public Task Handle(RewardProgramIsNotActive domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, $"\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    #endregion
}