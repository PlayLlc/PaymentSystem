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

        #region Instance Members

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Result> Create(CreateUserRegistrationCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var userRegistration = UserRegistration.CreateNewUserRegistration(_UniqueEmailChecker, _PasswordHasher, command);
                await _UserRegistrationRepository.SaveAsync(userRegistration).ConfigureAwait(false);

                return Ok();
            }

            // TODO: Let's abstract the handling of exceptions for business rules and value object exceptions
            catch (BusinessRuleValidationException e)
            { }
            catch (ValueObjectException e)
            { }
            catch (Exception e)
            {
                _Logger.Log(LogLevel.Error,
                    $"The {nameof(UserRegistration)} with the email: [{command.Email}] could not be created because of an internal server error", e);

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SmsVerification(VerifyConfirmationCodeCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                UserRegistration? merchantRegistration = await _UserRegistrationRepository.GetByIdAsync(command.UserRegistrationId).ConfigureAwait(false);

                if (merchantRegistration is null)
                    return View("SmsVerificationFailed");

                await _UserRegistrationRepository.SaveAsync(merchantRegistration!).ConfigureAwait(false);

                if (!merchantRegistration.VerifyMobilePhone(new VerifyConfirmationCodeCommand()).Succeeded)
                    return View("SmsVerificationFailed");

                return View("SmsVerificationSuccessful");
            }
            catch (ValueObjectException e)
            {
                _Logger.Log(LogLevel.Error,
                    $"The {nameof(SmsVerification)} could not be completed. The {nameof(MerchantRegistration)} with the Id: [{command.UserRegistrationId}] contained bad information. An exception occurred initializing a value object");

                return BadRequest();
            }
            catch (InvalidOperationException e)
            {
                _Logger.Log(LogLevel.Error,
                    $"The {nameof(MerchantRegistration)} with the Id: [{command.UserRegistrationId}] contained bad information. An exception occurred initializing a value object");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
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