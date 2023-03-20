using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Events;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;

namespace Play.Loyalty.Application.Handlers.DomainEvents;

public partial class LoyaltyProgramHandler : DomainEventHandler, IHandleDomainEvents<LoyaltyProgramHasBeenCreated>,
    IHandleDomainEvents<LoyaltyProgramHasBeenRemoved>, IHandleDomainEvents<AggregateUpdateWasAttemptedByUnknownUser<Programs>>,
    IHandleDomainEvents<DeactivatedMerchantAttemptedToCreateAggregate<Programs>>, IHandleDomainEvents<DeactivatedUserAttemptedToUpdateAggregate<Programs>>
{
    #region Instance Values

    private readonly IMessageSession _MessageHandlerContext;
    private readonly IProgramsRepository _ProgramsRepository;

    #endregion

    #region Constructor

    public LoyaltyProgramHandler(
        IMessageSession messageHandlerContext, IProgramsRepository programsRepository, ILogger<LoyaltyProgramHandler> logger) : base(logger)
    {
        _MessageHandlerContext = messageHandlerContext;
        _ProgramsRepository = programsRepository;
        SubscribeDiscountsPartial(this);
        SubscribeRewardsProgramPartial(this);
        Subscribe((IHandleDomainEvents<LoyaltyProgramHasBeenCreated>) this);
        Subscribe((IHandleDomainEvents<LoyaltyProgramHasBeenRemoved>) this);
        Subscribe((IHandleDomainEvents<AggregateUpdateWasAttemptedByUnknownUser<Programs>>) this);
        Subscribe((IHandleDomainEvents<DeactivatedMerchantAttemptedToCreateAggregate<Programs>>) this);
        Subscribe((IHandleDomainEvents<DeactivatedUserAttemptedToUpdateAggregate<Programs>>) this);
    }

    #endregion

    #region Instance Members

    public Task Handle(AggregateUpdateWasAttemptedByUnknownUser<Programs> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The User is not associated with the specified Merchant");

        return Task.CompletedTask;
    }

    public Task Handle(DeactivatedMerchantAttemptedToCreateAggregate<Programs> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The Merchant is deactivated and should not be authorized to use this capability");

        return Task.CompletedTask;
    }

    public Task Handle(DeactivatedUserAttemptedToUpdateAggregate<Programs> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The User is deactivated and should not be authorized to use this capability");

        return Task.CompletedTask;
    }

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