using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Domain.Events;
using Play.Loyalty.Contracts;
using Play.Loyalty.Domain.Aggregates;
using Play.Loyalty.Domain.Repositories;

namespace Play.Loyalty.Application.Handlers.DomainEvents
{
    public partial class LoyaltyProgramHandler : DomainEventHandler, IHandleDomainEvents<LoyaltyProgramHasBeenCreated>,
        IHandleDomainEvents<LoyaltyProgramHasBeenRemoved>
    {
        #region Instance Values

        private readonly IMessageHandlerContext _MessageHandlerContext;
        private readonly ILoyaltyProgramRepository _LoyaltyProgramRepository;

        #endregion

        #region Constructor

        public LoyaltyProgramHandler(
            IMessageHandlerContext messageHandlerContext, ILoyaltyProgramRepository loyaltyProgramRepository,
            ILogger<LoyaltyProgramHandler> logger) : base(logger)
        {
            _MessageHandlerContext = messageHandlerContext;
            _LoyaltyProgramRepository = loyaltyProgramRepository;
        }

        #endregion

        #region Instance Members

        public async Task Handle(LoyaltyProgramHasBeenCreated domainEvent)
        {
            Log(domainEvent);
            await _LoyaltyProgramRepository.SaveAsync(domainEvent.LoyaltyProgram).ConfigureAwait(false);
        }

        public async Task Handle(LoyaltyProgramHasBeenRemoved domainEvent)
        {
            Log(domainEvent);
            await _LoyaltyProgramRepository.SaveAsync(domainEvent.LoyaltyProgram).ConfigureAwait(false);
        }

        #endregion
    }
}