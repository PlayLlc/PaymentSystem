using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Events;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;

namespace Play.Loyalty.Application.Handlers.DomainEvents;

public partial class LoyaltyProgramHandler : DomainEventHandler, IHandleDomainEvents<LoyaltyProgramHasBeenCreated>,
    IHandleDomainEvents<LoyaltyProgramHasBeenRemoved>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;
    private readonly IProgramsRepository _ProgramsRepository;

    #endregion

    #region Constructor

    public LoyaltyProgramHandler(
        IMessageHandlerContext messageHandlerContext, IProgramsRepository programsRepository, ILogger<LoyaltyProgramHandler> logger) : base(logger)
    {
        _MessageHandlerContext = messageHandlerContext;
        _ProgramsRepository = programsRepository;
    }

    #endregion

    #region Instance Members

    public async Task Handle(LoyaltyProgramHasBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _ProgramsRepository.SaveAsync(domainEvent.Programs).ConfigureAwait(false);
    }

    public async Task Handle(LoyaltyProgramHasBeenRemoved domainEvent)
    {
        Log(domainEvent);
        await _ProgramsRepository.SaveAsync(domainEvent.Programs).ConfigureAwait(false);
    }

    #endregion
}