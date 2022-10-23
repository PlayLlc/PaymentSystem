using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Services;
using Play.Domain;
using Play.Domain.Events;
using Play.Domain.Exceptions;
using Play.Domain.Repositories;
using Play.Domain.ValueObjects;

namespace Play.Accounts.Application.Handlers.Domain;

public class UserRegistrationHandler : DomainEventHandler, IHandleDomainEvents<EmailVerificationCodeHasExpired>,
    IHandleDomainEvents<EmailVerificationCodeWasIncorrect>, IHandleDomainEvents<SmsVerificationCodeHasExpired>,
    IHandleDomainEvents<SmsVerificationCodeWasIncorrect>, IHandleDomainEvents<UserRegistrationHasBeenRejected>, IHandleDomainEvents<UserRegistrationHasExpired>,
    IHandleDomainEvents<UserRegistrationHasNotBeenApproved>, IHandleDomainEvents<EmailVerificationCodeFailedToSend>,
    IHandleDomainEvents<EmailVerificationCodeHasBeenSent>, IHandleDomainEvents<EmailVerificationWasSuccessful>,
    IHandleDomainEvents<SmsVerificationCodeFailedToSend>, IHandleDomainEvents<SmsVerificationCodeHasBeenSent>,
    IHandleDomainEvents<UserRegistrationAddressUpdated>, IHandleDomainEvents<UserRegistrationContactInfoUpdated>,
    IHandleDomainEvents<UserRegistrationPersonalDetailsUpdated>, IHandleDomainEvents<UserRegistrationCreated>,
    IHandleDomainEvents<UserRegistrationHasBeenApproved>, IHandleDomainEvents<UserRegistrationPhoneVerified>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;
    private readonly IHashPasswords _PasswordHasher;
    private readonly IVerifyEmailAccounts _EmailAccountVerifier;
    private readonly IVerifyMobilePhones _MobilePhoneVerifier;
    private readonly IUnderwriteMerchants _MerchantUnderwriter;
    private readonly IRepository<User, string> _UserRepository;
    private readonly IRepository<UserRegistration, string> _UserRegistrationRepository;

    #endregion

    #region Constructor

    public UserRegistrationHandler(
        IMessageHandlerContext messageHandlerContext, IVerifyEmailAccounts emailAccountVerifier, IVerifyMobilePhones mobilePhoneVerifier,
        IUnderwriteMerchants merchantUnderwriter, IRepository<User, string> userRepository, IRepository<UserRegistration, string> userRegistrationRepository,
        ILogger<UserRegistrationHandler> logger) : base(logger)
    {
        _MessageHandlerContext = messageHandlerContext;
        _EmailAccountVerifier = emailAccountVerifier;
        _MobilePhoneVerifier = mobilePhoneVerifier;
        _MerchantUnderwriter = merchantUnderwriter;
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

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    /// <exception cref="BusinessRuleValidationException"></exception>
    public async Task Handle(EmailVerificationCodeHasExpired domainEvent)
    {
        Log(domainEvent);
        await domainEvent.UserRegistration.SendEmailVerificationCode(_EmailAccountVerifier).ConfigureAwait(false);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

    public Task Handle(EmailVerificationCodeWasIncorrect domainEvent)
    {
        Log(domainEvent);

        return Task.CompletedTask;
    }

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task Handle(SmsVerificationCodeHasExpired domainEvent)
    {
        Log(domainEvent);
        await domainEvent.UserRegistration.SendSmsVerificationCode(_MobilePhoneVerifier).ConfigureAwait(false);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

    public Task Handle(SmsVerificationCodeWasIncorrect domainEvent)
    {
        Log(domainEvent);

        return Task.CompletedTask;
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

    public Task Handle(EmailVerificationCodeFailedToSend domainEvent)
    {
        // HACK: ===============================================
        // TODO: We need some kind of exponential retry strategy
        // HACK: ===============================================
        Log(domainEvent);

        return Task.CompletedTask;
    }

    public async Task Handle(EmailVerificationCodeHasBeenSent domainEvent)
    {
        Log(domainEvent);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

    public async Task Handle(EmailVerificationWasSuccessful domainEvent)
    {
        Log(domainEvent);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

    public Task Handle(SmsVerificationCodeFailedToSend domainEvent)
    {
        // HACK: ===============================================
        // TODO: We need some kind of exponential retry strategy
        // HACK: ===============================================
        Log(domainEvent);

        return Task.CompletedTask;
    }

    public async Task Handle(SmsVerificationCodeHasBeenSent domainEvent)
    {
        Log(domainEvent);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

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

    /// <exception cref="BusinessRuleValidationException"></exception>
    /// <exception cref="CommandOutOfSyncException"></exception>
    public async Task Handle(UserRegistrationHasBeenApproved domainEvent)
    {
        Log(domainEvent);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
        await _UserRepository.SaveAsync(domainEvent.UserRegistration.CreateUser(_PasswordHasher)).ConfigureAwait(false);
    }

    public async Task Handle(UserRegistrationPhoneVerified domainEvent)
    {
        Log(domainEvent);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

    #endregion
}