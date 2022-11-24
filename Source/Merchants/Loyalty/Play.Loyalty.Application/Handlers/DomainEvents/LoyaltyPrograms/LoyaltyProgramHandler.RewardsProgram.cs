using Play.Domain.Events;
using Play.Loyalty.Contracts;
using Play.Loyalty.Domain.Aggregates;

namespace Play.Loyalty.Application.Handlers.DomainEvents;

public partial class LoyaltyProgramHandler : DomainEventHandler, IHandleDomainEvents<RewardsProgramActivationStatusUpdated>,
    IHandleDomainEvents<RewardsProgramHasBeenUpdated>
{
    #region Instance Members

    public async Task Handle(RewardsProgramActivationStatusUpdated domainEvent)
    {
        Log(domainEvent);

        await _LoyaltyProgramRepository.SaveAsync(domainEvent.LoyaltyProgram).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<RewardsProgramActivationStatusUpdatedEvent>((a) =>
            {
                a.LoyaltyProgramId = domainEvent.LoyaltyProgram.Id;
                a.UserId = domainEvent.UserId;
                a.IsActive = domainEvent.IsActive;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(RewardsProgramHasBeenUpdated domainEvent)
    {
        Log(domainEvent);

        await _LoyaltyProgramRepository.SaveAsync(domainEvent.LoyaltyProgram).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<RewardsProgramHasBeenUpdatedEvent>((a) =>
            {
                a.LoyaltyProgramId = domainEvent.LoyaltyProgram.Id;
                a.UserId = domainEvent.UserId;
                a.RewardsProgram = domainEvent.LoyaltyProgram.AsDto().RewardsProgram;
            }, null)
            .ConfigureAwait(false);
    }

    #endregion
}