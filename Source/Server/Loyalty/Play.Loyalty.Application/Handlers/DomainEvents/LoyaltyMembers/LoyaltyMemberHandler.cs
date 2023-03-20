using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Events;
using Play.Loyalty.Contracts;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;

namespace Play.Loyalty.Application.Handlers.DomainEvents;

public partial class LoyaltyMemberHandler : DomainEventHandler, IHandleDomainEvents<LoyaltyMemberCreated>, IHandleDomainEvents<LoyaltyMemberRemoved>,
    IHandleDomainEvents<LoyaltyMemberUpdated>, IHandleDomainEvents<RewardsNumberIsNotUnique>,
    IHandleDomainEvents<AggregateUpdateWasAttemptedByUnknownUser<Member>>, IHandleDomainEvents<DeactivatedMerchantAttemptedToCreateAggregate<Member>>,
    IHandleDomainEvents<DeactivatedUserAttemptedToUpdateAggregate<Member>>
{
    #region Instance Values

    private readonly IMessageSession _MessageSession;
    private readonly IMemberRepository _MemberRepository;

    #endregion

    #region Constructor

    public LoyaltyMemberHandler(IMessageSession messageSession, IMemberRepository memberRepository, ILogger<LoyaltyMemberHandler> logger) : base(logger)
    {
        _MessageSession = messageSession;
        _MemberRepository = memberRepository;
        SubscribeRewardsPartial(this);
        Subscribe((IHandleDomainEvents<LoyaltyMemberCreated>) this);
        Subscribe((IHandleDomainEvents<LoyaltyMemberRemoved>) this);
        Subscribe((IHandleDomainEvents<LoyaltyMemberUpdated>) this);
        Subscribe((IHandleDomainEvents<RewardsNumberIsNotUnique>) this);
        Subscribe((IHandleDomainEvents<AggregateUpdateWasAttemptedByUnknownUser<Member>>) this);
        Subscribe((IHandleDomainEvents<DeactivatedMerchantAttemptedToCreateAggregate<Member>>) this);
        Subscribe((IHandleDomainEvents<DeactivatedUserAttemptedToUpdateAggregate<Member>>) this);
    }

    #endregion

    #region Instance Members

    public Task Handle(AggregateUpdateWasAttemptedByUnknownUser<Member> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The User is not associated with the specified Merchant");

        return Task.CompletedTask;
    }

    public Task Handle(DeactivatedMerchantAttemptedToCreateAggregate<Member> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The Merchant is deactivated and should not be authorized to use this capability");

        return Task.CompletedTask;
    }

    public Task Handle(DeactivatedUserAttemptedToUpdateAggregate<Member> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The User is deactivated and should not be authorized to use this capability");

        return Task.CompletedTask;
    }

    public async Task Handle(LoyaltyMemberCreated domainEvent)
    {
        Log(domainEvent);
        await _MemberRepository.SaveAsync(domainEvent.Member).ConfigureAwait(false);

        await _MessageSession.Publish<LoyaltyMemberCreatedEvent>(a =>
            {
                a.LoyaltyMember = domainEvent.Member.AsDto();
                a.UserId = domainEvent.UserId;
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(LoyaltyMemberRemoved domainEvent)
    {
        Log(domainEvent);
        await _MemberRepository.SaveAsync(domainEvent.Member).ConfigureAwait(false);

        await _MessageSession.Publish<LoyaltyMemberRemovedEvent>(a =>
            {
                a.LoyaltyMember = domainEvent.Member.AsDto();
                a.UserId = domainEvent.UserId;
            })
            .ConfigureAwait(false);
    }

    public async Task Handle(LoyaltyMemberUpdated domainEvent)
    {
        Log(domainEvent);
        await _MemberRepository.SaveAsync(domainEvent.Member).ConfigureAwait(false);
        await _MessageSession.Publish<LoyaltyMemberUpdatedEvent>(a =>
            {
                a.LoyaltyMember = domainEvent.Member.AsDto();
                a.UserId = domainEvent.UserId;
            })
            .ConfigureAwait(false);
    }

    public Task Handle(RewardsNumberIsNotUnique domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: If we're having this problem we're doing pretty well");

        return Task.CompletedTask;
    }

    #endregion
}