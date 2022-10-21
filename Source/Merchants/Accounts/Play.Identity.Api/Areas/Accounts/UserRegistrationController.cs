using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Commands.User;
using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;
using Play.Core;
using Play.Domain;
using Play.Domain.Exceptions;
using Play.Domain.ValueObjects;

namespace Play.Identity.Api.Areas.Accounts
{
    [Area("Accounts")]
    [Route("[area]/[controller]")]
    [ApiController]
    public class UserRegistrationController : Controller
    {
        #region Instance Values

        private readonly ILogger<UserRegistrationController> _Logger;
        private readonly IUserRegistrationRepository _UserRegistrationRepository;
        private readonly IVerifyMobilePhones _MobilePhoneVerifier;
        private readonly IUnderwriteMerchants _MerchantUnderwriter;
        private readonly IVerifyEmailAccounts _EmailVerifier;
        private readonly IEnsureUniqueEmails _UniqueEmailChecker;
        private readonly IHashPasswords _PasswordHasher;

        #endregion

        #region Constructor

        public UserRegistrationController(
            ILogger<UserRegistrationController> logger, IUserRegistrationRepository userRegistrationRepository, IEnsureUniqueEmails uniqueEmailChecker,
            IHashPasswords passwordHasher)
        {
            _Logger = logger;
            _UserRegistrationRepository = userRegistrationRepository;
            _UniqueEmailChecker = uniqueEmailChecker;
            _PasswordHasher = passwordHasher;
        }

        #endregion

        #region Instance Members

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IAsyncResult> Create(CreateUserRegistrationCommand command)
        {
            var a = UserRegistration.CreateNewUserRegistration(_UniqueEmailChecker, _PasswordHasher, command);
            await a.SendEmailVerificationCode(_EmailVerifier).ConfigureAwait(false);

            return Task.FromResult(Ok());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailVerification(VerifyConfirmationCodeCommand command)
        {
            UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(command.UserRegistrationId).ConfigureAwait(false)
                                                 ?? throw new NotFoundException(typeof(UserRegistration), command.UserRegistrationId);

            // Redirect exceptions thrown specifically for BusinessRuleValidations, let the Exception Filter take care of the rest
            try
            {
                userRegistration.VerifyEmail(command);
            }
            catch (BusinessRuleValidationException e)
            {
                _Logger.Log(LogLevel.Error, e.Message);

                return View("EmailVerificationFailed");
            }

            return View("EmailVerificationSuccessful");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateContactInfo(UpdateContactCommand command)
        {
            UserRegistration userRegistration = await _UserRegistrationRepository.GetByIdAsync(command.Id).ConfigureAwait(false)
                                                ?? throw new NotFoundException(typeof(UserRegistration), command.Id);

            userRegistration.UpdateContactInfo(command);
            await userRegistration.SendSmsVerificationCode(_MobilePhoneVerifier).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SmsVerification(VerifyConfirmationCodeCommand command)
        {
            UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(command.UserRegistrationId).ConfigureAwait(false)
                                                 ?? throw new NotFoundException(typeof(UserRegistration), command.UserRegistrationId);

            // Redirect exceptions thrown specifically for BusinessRuleValidations, let the Exception Filter take care of the rest
            try
            {
                userRegistration.VerifyMobilePhone(command);
            }
            catch (BusinessRuleValidationException e)
            {
                _Logger.Log(LogLevel.Error, e.Message);

                return View("SmsVerificationFailed");
            }

            return View("SmsVerificationSuccessful");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateContactInfo(UpdateAddressCommand command)
        {
            UserRegistration userRegistration = await _UserRegistrationRepository.GetByIdAsync(command.Id).ConfigureAwait(false)
                                                ?? throw new NotFoundException(typeof(UserRegistration), command.Id);

            userRegistration.UpdateUserAddress(command);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePersonalDetails(UpdatePersonalDetailsCommand command)
        {
            UserRegistration userRegistration = await _UserRegistrationRepository.GetByIdAsync(command.Id).ConfigureAwait(false)
                                                ?? throw new NotFoundException(typeof(UserRegistration), command.Id);

            userRegistration.UpdatePersonalDetails(command);
            userRegistration.AnalyzeUserRisk(_MerchantUnderwriter);

            return Ok();
        }

        #endregion
    }
}