using Play.Domain.Events;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Play.Accounts.Domain.Aggregates;

using NServiceBus;
using NServiceBus.Logging;

using Play.Accounts.Domain.Services;
using Play.Domain.Repositories;
using Play.Accounts.Contracts.Events;

namespace Play.Accounts.Application.Handlers.Domain
{
    public class UserHandler : DomainEventHandler, IHandleDomainEvents<UserHasBeenCreated>
    {
        #region Instance Values

        private readonly IMessageHandlerContext _MessageHandlerContext;
        private readonly IRepository<User, string> _UserRepository;

        #endregion

        #region Constructor

        public UserHandler(IMessageHandlerContext messageHandlerContext, IRepository<User, string> userRepository, ILogger<UserHandler> logger) : base(logger)
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
}