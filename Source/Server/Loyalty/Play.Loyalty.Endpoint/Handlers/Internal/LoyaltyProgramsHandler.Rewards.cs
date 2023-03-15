using NServiceBus;

using Play.Loyalty.Contracts;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Endpoint.Handlers;

public partial class LoyaltyProgramsHandler : IHandleMessages<RewardProgramActiveStatusHasBeenUpdatedEvent>, IHandleMessages<RewardsProgramHasBeenUpdatedEvent>

{
    #region Instance Members

    public Task Handle(RewardProgramActiveStatusHasBeenUpdatedEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    public Task Handle(RewardsProgramHasBeenUpdatedEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    #endregion
}