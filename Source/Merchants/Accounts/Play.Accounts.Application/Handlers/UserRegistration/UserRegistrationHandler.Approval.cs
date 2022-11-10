using Microsoft.Extensions.Logging;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;
using Play.Domain;
using Play.Domain.Events;
using Play.Domain.Exceptions;

namespace Play.Accounts.Application.Handlers;

public partial class UserRegistrationHandler : DomainEventHandler, IHandleDomainEvents<UserRegistrationHasBeenRejected>,
    IHandleDomainEvents<UserRegistrationHasExpired>, IHandleDomainEvents<UserRegistrationHasNotBeenApproved>, IHandleDomainEvents<UserRegistrationCreated>,
    IHandleDomainEvents<UserRegistrationHasBeenApproved>

{
    #region Instance Values

    private readonly IHashPasswords _PasswordHasher;
    private readonly IVerifyEmailAccounts _EmailAccountVerifier;
    private readonly IVerifyMobilePhones _MobilePhoneVerifier;
    private readonly IUserRepository _UserRepository;
    private readonly IUserRegistrationRepository _UserRegistrationRepository;

    #endregion

    #region Constructor

    public UserRegistrationHandler(
        IHashPasswords passwordHasher, IVerifyEmailAccounts emailAccountVerifier, IVerifyMobilePhones mobilePhoneVerifier, IUserRepository userRepository,
        IUserRegistrationRepository userRegistrationRepository, ILogger<UserRegistrationHandler> logger) : base(logger)
    {
        _PasswordHasher = passwordHasher;
        _EmailAccountVerifier = emailAccountVerifier;
        _MobilePhoneVerifier = mobilePhoneVerifier;
        _UserRepository = userRepository;
        _UserRegistrationRepository = userRegistrationRepository;
    }

    #endregion

    #region Instance Members

    public async Task Handle(UserRegistrationCreated domainEvent)
    {
        Log(domainEvent);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

    public async Task Handle(UserRegistrationHasBeenRejected domainEvent)
    {
        Log(domainEvent);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

    public Task Handle(UserRegistrationHasExpired domainEvent)
    {
        // HACK:================================
        // TODO: What in the heck do we do here?
        // HACK:================================

        Log(domainEvent);

        return Task.CompletedTask;
    }

    public Task Handle(UserRegistrationHasNotBeenApproved domainEvent)
    {
        // This means that we have an internal exception in our client registration implementation
        Log(domainEvent);

        return Task.CompletedTask;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    public async Task Handle(UserRegistrationHasBeenApproved domainEvent)
    {
        Log(domainEvent);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
        await _UserRepository.SaveAsync(domainEvent.UserRegistration.CreateUser(_PasswordHasher)).ConfigureAwait(false);
    }

    #endregion
}