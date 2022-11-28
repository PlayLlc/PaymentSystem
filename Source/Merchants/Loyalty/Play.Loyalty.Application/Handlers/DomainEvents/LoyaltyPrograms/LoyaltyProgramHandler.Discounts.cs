using Microsoft.Extensions.Logging;

using Play.Domain.Events;
using Play.Loyalty.Contracts;
using Play.Loyalty.Domain.Aggregates;

namespace Play.Loyalty.Application.Handlers.DomainEvents;

public partial class LoyaltyProgramHandler : DomainEventHandler, IHandleDomainEvents<DiscountHasBeenCreated>, IHandleDomainEvents<DiscountPriceHasBeenUpdated>,
    IHandleDomainEvents<DiscountHasBeenRemoved>, IHandleDomainEvents<DiscountItemDoesNotExist>, IHandleDomainEvents<DiscountProgramActiveStatusHasBeenUpdated>
{
    #region Instance Members

    public async Task Handle(DiscountProgramActiveStatusHasBeenUpdated domainEvent)
    {
        Log(domainEvent);

        await _ProgramsRepository.SaveAsync(domainEvent.Programs).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<DiscountProgramActiveStatusHasBeenUpdatedEvent>((a) =>
            {
                a.LoyaltyProgramId = domainEvent.Programs.Id;
                a.UserId = domainEvent.UserId;
                a.IsActive = domainEvent.IsActive;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(DiscountHasBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _ProgramsRepository.SaveAsync(domainEvent.Programs).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<DiscountHasBeenCreatedEvent>((a) =>
            {
                a.Discount = domainEvent.Discount.AsDto();
                a.UserId = domainEvent.UserId;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(DiscountPriceHasBeenUpdated domainEvent)
    {
        Log(domainEvent);
        await _ProgramsRepository.SaveAsync(domainEvent.Programs).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<DiscountHasBeenUpdatedEvent>((a) =>
            {
                a.DiscountId = domainEvent.DiscountId;
                a.Price = domainEvent.Price;
                a.UserId = domainEvent.UserId;
            }, null)
            .ConfigureAwait(false);
    }

    public async Task Handle(DiscountHasBeenRemoved domainEvent)
    {
        Log(domainEvent);
        await _ProgramsRepository.SaveAsync(domainEvent.Programs).ConfigureAwait(false);

        await _MessageHandlerContext.Publish<DiscountHasBeenRemovedEvent>((a) =>
            {
                a.DiscountId = domainEvent.DiscountId;
                a.UserId = domainEvent.UserId;
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