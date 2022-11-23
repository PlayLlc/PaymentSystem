using Microsoft.Extensions.Logging;

using Play.Domain.Events;
using Play.Loyalty.Contracts.NetworkEvents;
using Play.Loyalty.Domain.Aggregates;

namespace Play.Loyalty.Application.Handlers.DomainEvents;

public partial class LoyaltyProgramHandler : DomainEventHandler, IHandleDomainEvents<DiscountHasBeenCreated>, IHandleDomainEvents<DiscountHasBeenUpdated>,
    IHandleDomainEvents<DiscountHasBeenRemoved>, IHandleDomainEvents<DiscountItemDoesNotExist>
{
    #region Instance Members

    public async Task Handle(DiscountHasBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _LoyaltyProgramRepository.SaveAsync(domainEvent.LoyaltyProgram).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<DiscountHasBeenCreatedEvent>((a) =>
            {
                a.Discount = domainEvent.Discount.AsDto();
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(DiscountHasBeenUpdated domainEvent)
    {
        Log(domainEvent);
        await _LoyaltyProgramRepository.SaveAsync(domainEvent.LoyaltyProgram).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<DiscountHasBeenUpdatedEvent>((a) =>
            {
                a.Discount = domainEvent.Discount.AsDto();
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(DiscountHasBeenRemoved domainEvent)
    {
        Log(domainEvent);
        await _LoyaltyProgramRepository.SaveAsync(domainEvent.LoyaltyProgram).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<DiscountHasBeenRemovedEvent>((a) =>
            {
                a.Discount = domainEvent.Discount.AsDto();
            }, null)
            .ConfigureAwait(false);
    }

    public Task Handle(DiscountItemDoesNotExist domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, $"\n\n\n\nWARNING: There is likely a race condition occurring or an error in the client integration");

        return Task.CompletedTask;
    }

    #endregion
}