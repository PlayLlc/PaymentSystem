using NServiceBus;

using Play.Loyalty.Contracts;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Endpoint.Handlers;

public class LoyaltyProgramsHandler : IHandleMessages<DiscountHasBeenRemovedEvent>, IHandleMessages<DiscountHasBeenUpdatedEvent>

{
    #region Instance Values

    private readonly IProgramsRepository _ProgramsRepository;
    private readonly IMemberRepository _MemberRepository;
    private readonly IRetrieveUsers _UserRetriever;

    #endregion

    #region Constructor

    public LoyaltyProgramsHandler(IProgramsRepository programsRepository, IMemberRepository memberRepository, IRetrieveUsers userRetriever)
    {
        _ProgramsRepository = programsRepository;
        _MemberRepository = memberRepository;
        _UserRetriever = userRetriever;
    }

    #endregion

    #region Instance Members

    public Task Handle(DiscountHasBeenRemovedEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    public Task Handle(DiscountHasBeenUpdatedEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    #endregion
}