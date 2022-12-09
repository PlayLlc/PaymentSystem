using NServiceBus;

using Play.Loyalty.Contracts;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Endpoint.Handlers;

public class LoyaltyMembersHandler : IHandleMessages<LoyaltyMemberCreatedEvent>, IHandleMessages<LoyaltyMemberEarnedPointsEvent>,
    IHandleMessages<LoyaltyMemberEarnedRewardsEvent>, IHandleMessages<LoyaltyMemberLostPointsEvent>, IHandleMessages<LoyaltyMemberRedeemedRewardsEvent>,
    IHandleMessages<LoyaltyMemberRemovedEvent>, IHandleMessages<LoyaltyMemberUpdatedEvent>

{
    #region Instance Values

    private readonly IProgramsRepository _ProgramsRepository;
    private readonly IMemberRepository _MemberRepository;
    private readonly IRetrieveUsers _UserRetriever;

    #endregion

    #region Constructor

    public LoyaltyMembersHandler(IProgramsRepository programsRepository, IMemberRepository memberRepository, IRetrieveUsers userRetriever)
    {
        _ProgramsRepository = programsRepository;
        _MemberRepository = memberRepository;
        _UserRetriever = userRetriever;
    }

    #endregion

    #region Instance Members

    public Task Handle(LoyaltyMemberCreatedEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    public Task Handle(LoyaltyMemberEarnedPointsEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    public Task Handle(LoyaltyMemberEarnedRewardsEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    public Task Handle(LoyaltyMemberLostPointsEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    public Task Handle(LoyaltyMemberRedeemedRewardsEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    public Task Handle(LoyaltyMemberRemovedEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    public Task Handle(LoyaltyMemberUpdatedEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    #endregion
}