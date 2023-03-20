using Play.Domain.Events;
using Play.Inventory.Domain.Aggregates;

namespace Play.Inventory.Application.Handlers;

public partial class ItemHandler : DomainEventHandler, IHandleDomainEvents<ItemDescriptionUpdated>, IHandleDomainEvents<ItemNameUpdated>,
    IHandleDomainEvents<ItemPriceWasNotPositive>, IHandleDomainEvents<SkuUpdated>
{
    #region Instance Members

    private static void SubscribeDetailsPartial(ItemHandler handler)
    {
        handler.Subscribe((IHandleDomainEvents<ItemDescriptionUpdated>) handler);
        handler.Subscribe((IHandleDomainEvents<ItemNameUpdated>) handler);
        handler.Subscribe((IHandleDomainEvents<ItemPriceWasNotPositive>) handler);
        handler.Subscribe((IHandleDomainEvents<SkuUpdated>) handler);
    }

    public async Task Handle(ItemDescriptionUpdated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(ItemNameUpdated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(SkuUpdated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    // When we handle this domain event, it means there a problem with the client integration
    public Task Handle(ItemPriceWasNotPositive domainEvent)
    {
        Log(domainEvent);

        return Task.CompletedTask;
    }

    #endregion
}