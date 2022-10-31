using Play.Accounts.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Application.Handlers
{
    public partial class UserRegistrationHandler : DomainEventHandler, IHandleDomainEvents<UserRegistrationAddressUpdated>,
        IHandleDomainEvents<UserRegistrationContactInfoUpdated>, IHandleDomainEvents<UserRegistrationPersonalDetailsUpdated>
    {
        #region Instance Members

        public async Task Handle(UserRegistrationAddressUpdated domainEvent)
        {
            Log(domainEvent);

            await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
        }

        public async Task Handle(UserRegistrationContactInfoUpdated domainEvent)
        {
            Log(domainEvent);
            await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
        }

        public async Task Handle(UserRegistrationPersonalDetailsUpdated domainEvent)
        {
            Log(domainEvent);
            await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
        }

        #endregion
    }
}