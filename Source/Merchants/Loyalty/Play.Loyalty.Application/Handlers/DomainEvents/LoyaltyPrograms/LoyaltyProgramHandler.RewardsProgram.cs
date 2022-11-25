using Play.Domain.Events;
using Play.Loyalty.Contracts;
using Play.Loyalty.Domain.Aggregates;

namespace Play.Loyalty.Application.Handlers.DomainEvents;

public partial class LoyaltyProgramHandler : DomainEventHandler, IHandleDomainEvents<RewardsProgramActiveStatusHasBeenUpdated>,
    IHandleDomainEvents<RewardsProgramHasBeenUpdated>
{
    #region Instance Members

    public async Task Handle(RewardsProgramActiveStatusHasBeenUpdated domainEvent)
    {
        Log(domainEvent);

        await _ProgramsRepository.SaveAsync(domainEvent.Programs).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<RewardProgramActiveStatusHasBeenUpdatedEvent>((a) =>
            {
                a.LoyaltyProgramId = domainEvent.Programs.Id;
                a.UserId = domainEvent.UserId;
                a.IsActive = domainEvent.IsActive;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(RewardsProgramHasBeenUpdated domainEvent)
    {
        Log(domainEvent);

        await _ProgramsRepository.SaveAsync(domainEvent.Programs).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<RewardsProgramHasBeenUpdatedEvent>((a) =>
            {
                a.LoyaltyProgramId = domainEvent.Programs.Id;
                a.UserId = domainEvent.UserId;
                a.RewardsProgram = domainEvent.Programs.AsDto().RewardsProgram;
            }, null)
            .ConfigureAwait(false);
    }

    #endregion
}