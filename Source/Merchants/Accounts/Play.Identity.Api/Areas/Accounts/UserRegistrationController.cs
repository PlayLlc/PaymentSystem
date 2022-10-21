using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Contracts.Commands;
using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;
using Play.Core;
using Play.Domain;
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
            var userRegistration = UserRegistration.CreateNewUserRegistration(_UniqueEmailChecker, _PasswordHasher, command);
            await _UserRegistrationRepository.SaveAsync(userRegistration).ConfigureAwait(false);

            return Task.FromResult(Ok());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SmsVerification(VerifyConfirmationCodeCommand command)
        {
            UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(command.UserRegistrationId).ConfigureAwait(false);

            if (userRegistration is null)
                return View("SmsVerificationFailed");

            await _UserRegistrationRepository.SaveAsync(userRegistration!).ConfigureAwait(false);

            try
            {
                userRegistration.VerifyMobilePhone(command);
            }
            catch (BusinessRuleValidationException e)
            {
                return View("SmsVerificationFailed");
            }

            return View("SmsVerificationSuccessful");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EmailVerification(VerifyConfirmationCodeCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Error");

                UserRegistration? merchantRegistration = await _UserRegistrationRepository.GetByIdAsync(command.UserRegistrationId).ConfigureAwait(false);

                if (merchantRegistration is null)
                    return View("EmailVerificationFailed");

                await _UserRegistrationRepository.SaveAsync(merchantRegistration!).ConfigureAwait(false);

                if (!merchantRegistration.VerifyEmail(new VerifyConfirmationCodeCommand()).Succeeded)
                    return View("EmailVerificationFailed");

                return View("EmailVerificationSuccessful");
            }
            catch (ValueObjectException e)
            {
                _Logger.Log(LogLevel.Error,
                    $"The {nameof(EmailVerification)} could not be completed. TheThe {nameof(MerchantRegistration)} with the Id: [{command.UserRegistrationId}] contained bad information. An exception occurred initializing a value object");

                return BadRequest();
            }
            catch (InvalidOperationException e)
            {
                _Logger.Log(LogLevel.Error,
                    $"The {nameof(MerchantRegistration)} with the Id: [{command.UserRegistrationId}] contained bad information. An exception occurred initializing a value object");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}