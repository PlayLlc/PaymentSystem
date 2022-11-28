using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Events;
using Play.Inventory.Domain.Aggregates;
using Play.Inventory.Domain.Repositories;

namespace Play.Inventory.Application.Handlers;

public class CategoryHandler : DomainEventHandler, IHandleDomainEvents<AggregateUpdateWasAttemptedByUnknownUser<Category>>,
    IHandleDomainEvents<DeactivatedMerchantAttemptedToCreateAggregate<Category>>, IHandleDomainEvents<CategoryAlreadyExists>,
    IHandleDomainEvents<CategoryHasBeenCreated>, IHandleDomainEvents<CategoryHasBeenRemoved>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;
    private readonly ICategoryRepository _CategoryRepository;

    #endregion

    #region Constructor

    public CategoryHandler(IMessageHandlerContext messageHandlerContext, ICategoryRepository categoryRepository, ILogger<CategoryHandler> logger) : base(logger)
    {
        _MessageHandlerContext = messageHandlerContext;
        _CategoryRepository = categoryRepository;
    }

    #endregion

    #region Instance Members

    /// <summary>
    ///     We are receiving this event because there is an error in the client integration. The User is deactivated and should
    ///     not be authorized to use this capability
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <returns></returns>
    public Task Handle(AggregateUpdateWasAttemptedByUnknownUser<Category> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The User is not associated with the specified Merchant");

        return Task.CompletedTask;
    }

    /// <summary>
    ///     We are receiving this event because there is an error in the client integration. The Merchant is deactivated and
    ///     should not be authorized to use this capability
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <returns></returns>
    public Task Handle(DeactivatedMerchantAttemptedToCreateAggregate<Category> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The Merchant is deactivated and should not be authorized to use this capability");

        return Task.CompletedTask;
    }

    /// <summary>
    ///     We are receiving this event because there is an error in the client integration. The User is deactivated and
    ///     should not be authorized to use this capability
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <returns></returns>
    public Task Handle(DeactivatedUserAttemptedToUpdateAggregate<Category> domainEvent)
    {
        Log(domainEvent, LogLevel.Warning,
            "\n\n\n\nWARNING: There is likely an error in the client integration. The User is deactivated and should not be authorized to use this capability");

        return Task.CompletedTask;
    }

    /// <summary>
    ///     We are receiving this event because there is a race condition or an error in the client integration. The client
    ///     is either sending a CreateCategory command for a Category that already exists, or a race condition has occured
    ///     and the Category has already been created
    /// </summary>
    public Task Handle(CategoryAlreadyExists domainEvent)
    {
        Log(domainEvent, LogLevel.Warning, "\n\n\n\nWARNING: There is likely an error in the client integration");

        return Task.CompletedTask;
    }

    public async Task Handle(CategoryHasBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _CategoryRepository.SaveAsync(domainEvent.Category).ConfigureAwait(false);
    }

    public async Task Handle(CategoryHasBeenRemoved domainEvent)
    {
        Log(domainEvent);
        await _CategoryRepository.RemoveAsync(domainEvent.Category).ConfigureAwait(false);
    }

    #endregion
}