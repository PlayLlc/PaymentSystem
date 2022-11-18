using Play.Domain.Events;
using Play.Inventory.Domain;

namespace Play.Inventory.Application.Handlers;

public partial class ItemHandler : DomainEventHandler, IHandleDomainEvents<ItemIsAvailableForAllLocations>, IHandleDomainEvents<ItemLocationAdded>,
    IHandleDomainEvents<ItemLocationRemoved>
{
    #region Instance Members

    public async Task Handle(ItemLocationAdded domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(ItemLocationRemoved domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(ItemIsAvailableForAllLocations domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    #endregion
}