using Play.Domain.Events;
using Play.Identity.Domain.Aggregates;

namespace Play.Identity.Application.Handlers;

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