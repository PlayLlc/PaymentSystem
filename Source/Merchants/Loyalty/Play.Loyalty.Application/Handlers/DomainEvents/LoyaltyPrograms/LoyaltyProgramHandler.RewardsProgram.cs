using Play.Domain.Events;
using Play.Loyalty.Domain.Aggregates;

namespace Play.Loyalty.Application.Handlers.DomainEvents
{
    public partial class LoyaltyProgramHandler : DomainEventHandler,
        IHandleDomainEvents<RewardsProgramActivationHasBeenToggled>,
        IHandleDomainEvents<RewardsProgramHasBeenUpdated>,
    {
        public Task Handle(RewardsProgramActivationHasBeenToggled domainEvent) => throw new NotImplementedException();

        public Task Handle(RewardsProgramHasBeenUpdated domainEvent) => throw new NotImplementedException();
    }
     
}