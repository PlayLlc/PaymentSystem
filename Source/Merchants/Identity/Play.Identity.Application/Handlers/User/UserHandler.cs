using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Events;
using Play.Domain.Repositories;
using Play.Identity.Contracts.Events;
using Play.Identity.Domain.Aggregates;

namespace Play.Identity.Application.Handlers;

public class UserHandler : DomainEventHandler, IHandleDomainEvents<UserHasBeenCreated>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;
    private readonly IRepository<User, SimpleStringId> _UserRepository;

    #endregion

    #region Constructor

    public UserHandler(
        IMessageHandlerContext messageHandlerContext, IRepository<User, SimpleStringId> userRepository, ILogger<UserHandler> logger) : base(logger)
    {
        _MessageHandlerContext = messageHandlerContext;
        _UserRepository = userRepository;
    }

    #endregion

    #region Instance Members

    public async Task Handle(UserHasBeenCreated domainEvent)
    {
        Log(domainEvent);
        await _UserRepository.SaveAsync(domainEvent.User).ConfigureAwait(false);

        // Broadcast that a new user has been created to Azure Service Bus
        await _MessageHandlerContext.Publish<UserHasBeenCreatedEvent>((a) =>
            {
                a.UserId = domainEvent.User.GetId();
            })
            .ConfigureAwait(false);
    }

    #endregion
}