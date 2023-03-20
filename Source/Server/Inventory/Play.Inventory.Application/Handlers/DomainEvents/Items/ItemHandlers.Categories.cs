using Microsoft.Extensions.Logging;

using Play.Domain.Events;
using Play.Inventory.Domain.Aggregates;

namespace Play.Inventory.Application.Handlers;

public partial class ItemHandler : DomainEventHandler, IHandleDomainEvents<CategoryMerchantDidNotMatch>, IHandleDomainEvents<ItemCategoriesAdded>,
    IHandleDomainEvents<ItemCategoriesRemoved>

{
    #region Instance Members

    private static void SubscribeCategoriesPartial(ItemHandler handler)
    {
        handler.Subscribe((IHandleDomainEvents<CategoryMerchantDidNotMatch>) handler);
        handler.Subscribe((IHandleDomainEvents<ItemCategoriesAdded>) handler);
        handler.Subscribe((IHandleDomainEvents<ItemCategoriesRemoved>) handler);
    }

    public async Task Handle(ItemCategoriesAdded domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(ItemCategoriesRemoved domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    // When we handle this domain event, it means there's a race condition or a problem with the client implementation
    public Task Handle(CategoryMerchantDidNotMatch domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: This is likely a client integration error");

        return Task.CompletedTask;
    }

    #endregion
}