using NServiceBus;

using Play.Loyalty.Contracts;
using Play.Loyalty.Domain.Repositories;
using Play.Loyalty.Domain.Services;

namespace Play.Loyalty.Endpoint.Handlers;

public partial class LoyaltyProgramsHandler : IHandleMessages<DiscountHasBeenRemovedEvent>, IHandleMessages<DiscountHasBeenUpdatedEvent>
{
    #region Instance Members

    public Task Handle(DiscountHasBeenRemovedEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    public Task Handle(DiscountHasBeenUpdatedEvent message, IMessageHandlerContext context) => throw new NotImplementedException();

    #endregion
}