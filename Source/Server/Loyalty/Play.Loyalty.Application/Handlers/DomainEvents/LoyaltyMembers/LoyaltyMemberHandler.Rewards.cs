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

    private static void SubscribeRewardsPartial(LoyaltyMemberHandler handler)
    {
        handler.Subscribe((IHandleDomainEvents<LoyaltyMemberEarnedPoints>) handler);
        handler.Subscribe((IHandleDomainEvents<LoyaltyMemberEarnedRewards>) handler);
        handler.Subscribe((IHandleDomainEvents<LoyaltyMemberLostPoints>) handler);
        handler.Subscribe((IHandleDomainEvents<LoyaltyMemberLostRewards>) handler);
        handler.Subscribe((IHandleDomainEvents<RewardBalanceIsInsufficient>) handler);
        handler.Subscribe((IHandleDomainEvents<RewardProgramIsNotActive>) handler);
    }

    public async Task Handle(LoyaltyMemberEarnedPoints domainEvent)
    {
        Log(domainEvent);
        await _MemberRepository.SaveAsync(domainEvent.Member).ConfigureAwait(false);

        await _MessageSession.Publish<LoyaltyMemberEarnedPointsEvent>(a =>
            {
                a.LoyaltyMember = domainEvent.Member.AsDto();
                a.TransactionId = domainEvent.TransactionId;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(LoyaltyMemberEarnedRewards domainEvent)
    {
        Log(domainEvent);
        await _MemberRepository.SaveAsync(domainEvent.Member).ConfigureAwait(false);

        await _MessageSession.Publish<LoyaltyMemberEarnedRewardsEvent>(a =>
            {
                a.LoyaltyMember = domainEvent.Member.AsDto();
                a.TransactionId = domainEvent.TransactionId;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(LoyaltyMemberLostPoints domainEvent)
    {
        Log(domainEvent);
        await _MemberRepository.SaveAsync(domainEvent.Member).ConfigureAwait(false);

        await _MessageSession.Publish<LoyaltyMemberLostPointsEvent>(a =>
            {
                a.LoyaltyMember = domainEvent.Member.AsDto();
                a.TransactionId = domainEvent.TransactionId;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(LoyaltyMemberLostRewards domainEvent)
    {
        Log(domainEvent);
        await _MemberRepository.SaveAsync(domainEvent.Member).ConfigureAwait(false);

        await _MessageSession.Publish<LoyaltyMemberLostPointsEvent>(a =>
            {
                a.LoyaltyMember = domainEvent.Member.AsDto();
                a.TransactionId = domainEvent.TransactionId;
            }, null)
            .ConfigureAwait(false);
    }

    public Task Handle(RewardBalanceIsInsufficient domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    public Task Handle(RewardProgramIsNotActive domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    #endregion
}