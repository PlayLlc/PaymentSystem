using Microsoft.Extensions.Logging;

using Play.Domain.Events;
using Play.Inventory.Domain;

namespace Play.Inventory.Application.Handlers;

internal partial class ItemHandlers : DomainEventHandler, IHandleDomainEvents<CategoryMerchantDidNotMatch>, IHandleDomainEvents<ItemCategoryAdded>,
    IHandleDomainEvents<ItemCategoryRemoved>

{
    #region Instance Members

    public async Task Handle(ItemCategoryAdded domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    public async Task Handle(ItemCategoryRemoved domainEvent)
    {
        Log(domainEvent);
        await _ItemRepository.SaveAsync(domainEvent.Item).ConfigureAwait(false);
    }

    // When we handle this domain event, it means there's a race condition or a problem with the client implementation
    public Task Handle(CategoryMerchantDidNotMatch domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, $"\n\n\n\nWARNING: This is likely a client integration error");

        return Task.CompletedTask;
    }

    #endregion
}