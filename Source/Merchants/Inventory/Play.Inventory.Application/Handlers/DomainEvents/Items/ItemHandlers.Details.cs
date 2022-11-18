using Play.Domain.Events;
using Play.Inventory.Domain;

namespace Play.Inventory.Application.Handlers;

public partial class ItemHandler : DomainEventHandler, IHandleDomainEvents<ItemDescriptionUpdated>, IHandleDomainEvents<ItemNameUpdated>,
    IHandleDomainEvents<ItemPriceUpdated>, IHandleDomainEvents<ItemPriceWasNotPositive>, IHandleDomainEvents<ItemSkuUpdated>
{
    #region Instance Members

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

    public async Task Handle(ItemPriceUpdated domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(ItemSkuUpdated domainEvent)
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