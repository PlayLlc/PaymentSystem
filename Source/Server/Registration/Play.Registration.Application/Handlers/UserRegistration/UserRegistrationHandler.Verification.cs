using Play.Domain.Events;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;
using Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents;
using Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents.Rules;

namespace Play.Identity.Application.Handlers;

public partial class UserRegistrationHandler : DomainEventHandler, IHandleDomainEvents<EmailVerificationCodeHasExpired>,
    IHandleDomainEvents<EmailVerificationCodeWasIncorrect>, IHandleDomainEvents<SmsVerificationCodeHasExpired>,
    IHandleDomainEvents<SmsVerificationCodeWasIncorrect>, IHandleDomainEvents<UserRegistrationPasswordWasTooWeak>,
    IHandleDomainEvents<UserRegistrationUsernameWasInvalid>, IHandleDomainEvents<EmailVerificationCodeFailedToSend>,
    IHandleDomainEvents<EmailVerificationCodeHasBeenSent>, IHandleDomainEvents<EmailVerificationWasSuccessful>,
    IHandleDomainEvents<SmsVerificationCodeFailedToSend>, IHandleDomainEvents<SmsVerificationCodeHasBeenSent>,
    IHandleDomainEvents<UserRegistrationPhoneVerified>
{
    #region Instance Members

    private static void SubscribeVerificationPartial(UserRegistrationHandler handler)
    {
        handler.Subscribe((IHandleDomainEvents<EmailVerificationCodeHasExpired>) handler);
        handler.Subscribe((IHandleDomainEvents<EmailVerificationCodeWasIncorrect>) handler);
        handler.Subscribe((IHandleDomainEvents<SmsVerificationCodeHasExpired>) handler);
        handler.Subscribe((IHandleDomainEvents<SmsVerificationCodeWasIncorrect>) handler);
        handler.Subscribe((IHandleDomainEvents<EmailVerificationCodeFailedToSend>) handler);
        handler.Subscribe((IHandleDomainEvents<EmailVerificationCodeHasBeenSent>) handler);
        handler.Subscribe((IHandleDomainEvents<EmailVerificationWasSuccessful>) handler);
        handler.Subscribe((IHandleDomainEvents<SmsVerificationCodeFailedToSend>) handler);
        handler.Subscribe((IHandleDomainEvents<SmsVerificationCodeHasBeenSent>) handler);
        handler.Subscribe((IHandleDomainEvents<UserRegistrationPhoneVerified>) handler);
        handler.Subscribe((IHandleDomainEvents<UserRegistrationPasswordWasTooWeak>) handler);
        handler.Subscribe((IHandleDomainEvents<UserRegistrationUsernameWasInvalid>) handler);
    }

    public async Task Handle(EmailVerificationCodeHasBeenSent domainEvent)
    {
        Log(domainEvent);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

    public Task Handle(UserRegistrationUsernameWasInvalid domainEvent)
    {
        Log(domainEvent);

        return Task.CompletedTask;
    }

    public Task Handle(UserRegistrationPasswordWasTooWeak domainEvent)
    {
        Log(domainEvent);

        return Task.CompletedTask;
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

    public Task Handle(EmailVerificationCodeFailedToSend domainEvent)
    {
        // HACK: ===============================================
        // TODO: We need some kind of exponential retry strategy
        // HACK: ===============================================
        Log(domainEvent);

        return Task.CompletedTask;
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

    public async Task Handle(UserRegistrationPhoneVerified domainEvent)
    {
        Log(domainEvent);
        await _UserRegistrationRepository.SaveAsync(domainEvent.UserRegistration).ConfigureAwait(false);
    }

    #endregion
}