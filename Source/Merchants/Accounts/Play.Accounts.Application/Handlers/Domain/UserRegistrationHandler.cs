using Microsoft.Extensions.Logging;

using NServiceBus;

using Play.Accounts.Contracts.Events;
using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Services;
using Play.Domain.Events;
using Play.Domain.Repositories;
using Play.Domain.ValueObjects;

namespace Play.Accounts.Application.Handlers.Domain;

// HACK: DAN --> Go back through all this and update the InvalidOperationExceptions to something more specific
public class UserRegistrationHandler : DomainEventHandler, IHandleDomainEvents<UserRegistrationRiskAnalysisApproved>,
    IHandleDomainEvents<UserRegistrationCreated>, IHandleDomainEvents<UserRegistrationContactInfoUpdated>, IHandleDomainEvents<UserRegistrationPhoneVerified>
{
    #region Instance Values

    private readonly IMessageHandlerContext _MessageHandlerContext;
    private readonly IVerifyEmailAccounts _EmailAccountVerifier;
    private readonly IVerifyMobilePhones _MobilePhoneVerifier;
    private readonly IUnderwriteMerchants _MerchantUnderwriter;
    private readonly IRepository<User, string> _UserRepository;
    private readonly IRepository<UserRegistration, string> _UserRegistrationRepository;

    #endregion

    #region Constructor

    public UserRegistrationHandler(
        ILogger logger, IMessageHandlerContext messageHandlerContext, IVerifyEmailAccounts emailAccountVerifier, IRepository<User, string> userRepository,
        IRepository<UserRegistration, string> userRegistrationRepository, IVerifyMobilePhones mobilePhoneVerifier) : base(logger)
    {
        _MessageHandlerContext = messageHandlerContext;
        _EmailAccountVerifier = emailAccountVerifier;
        _UserRepository = userRepository;
        _UserRegistrationRepository = userRegistrationRepository;
        _MobilePhoneVerifier = mobilePhoneVerifier;
    }

    #endregion

    #region Instance Members

    /// <exception cref="Play.Domain.ValueObjects.ValueObjectException"></exception>
    public async Task Handle(UserRegistrationCreated domainEvent)
    {
        UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(domainEvent.Id).ConfigureAwait(false);

        await userRegistration!.SendEmailAccountVerificationCode(_EmailAccountVerifier).ConfigureAwait(false);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task Handle(UserRegistrationContactInfoUpdated domainEvent)
    {
        UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(domainEvent.Id).ConfigureAwait(false);

        if (userRegistration is null)
            throw new InvalidOperationException();

        await userRegistration!.SendSmsVerificationCode(_MobilePhoneVerifier).ConfigureAwait(false);
    }

    /// <exception cref="ValueObjectException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task Handle(UserRegistrationPhoneVerified domainEvent)
    {
        UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(domainEvent.Id).ConfigureAwait(false);

        if (userRegistration is null)
            throw new InvalidOperationException();

        userRegistration.AnalyzeUserRisk(_MerchantUnderwriter);
        await _UserRegistrationRepository.SaveAsync(userRegistration).ConfigureAwait(false);
    }

    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ValueObjectException"></exception>
    public async Task Handle(UserRegistrationRiskAnalysisApproved domainEvent)
    {
        UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(domainEvent.Id).ConfigureAwait(false);

        if (userRegistration is null)
            throw new InvalidOperationException();

        await userRegistration!.SendSmsVerificationCode(_MobilePhoneVerifier).ConfigureAwait(false);

        await _UserRegistrationRepository.SaveAsync(userRegistration).ConfigureAwait(false);
    }

    public async Task Handle(MerchantRejectedBecauseItIsProhibited domainEvent)
    {
        Log(domainEvent);

        await _MessageHandlerContext.Publish<MerchantRegistrationWasRejectedEvent>((a) =>
            {
                a.MerchantRegistrationId = domainEvent.MerchantRegistrationId;
            })
            .ConfigureAwait(false);
    }

    #endregion

    ///// <exception cref="Play.Domain.ValueObjects.ValueObjectException"></exception>
    //public async Task Handle(MerchantRegistrationConfirmedDomainEvent domainEvent)
    //{
    //    Log(domainEvent);

    //    MerchantRegistration? merchantRegistration =
    //        await _MerchantRegistrationRepository.GetByIdAsync(domainEvent.MerchantRegistrationId).ConfigureAwait(false);

    //    Merchant merchant = Merchant.CreateFromMerchantRegistration(merchantRegistration!);
    //    await _MerchantRepository.SaveAsync(merchant).ConfigureAwait(false);

    //    // BUG: Update this. We need to
    //    await _MessageHandlerContext.Publish<MerchantRegistrationWasRejectedEvent>((a) =>
    //    {
    //        a.MerchantRegistrationId = domainEvent.MerchantRegistrationId;
    //    })
    //        .ConfigureAwait(false);
    //}
}