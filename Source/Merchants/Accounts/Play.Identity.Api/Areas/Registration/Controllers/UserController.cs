using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Commands.User;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Repositories;
using Play.Accounts.Domain.Services;
using Play.Domain;
using Play.Domain.Exceptions;
using Play.Identity.Api.Extensions;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Play.Identity.Api.Areas.Registration.Controllers
{
    [Area($"{nameof(Registration)}")]
    [Route("[area]/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        #region Instance Values

        private readonly ILogger<UserController> _Logger;
        private readonly IUserRegistrationRepository _UserRegistrationRepository;
        private readonly IVerifyMobilePhones _MobilePhoneVerifier;
        private readonly IUnderwriteMerchants _MerchantUnderwriter;
        private readonly IVerifyEmailAccounts _EmailVerifier;
        private readonly IEnsureUniqueEmails _UniqueEmailChecker;
        private readonly IHashPasswords _PasswordHasher;

        #endregion

        #region Constructor

        public UserController(
            ILogger<UserController> logger, IUserRegistrationRepository userRegistrationRepository, IVerifyMobilePhones mobilePhoneVerifier,
            IUnderwriteMerchants merchantUnderwriter, IVerifyEmailAccounts emailVerifier, IEnsureUniqueEmails uniqueEmailChecker, IHashPasswords passwordHasher)
        {
            _Logger = logger;
            _UserRegistrationRepository = userRegistrationRepository;
            _MobilePhoneVerifier = mobilePhoneVerifier;
            _MerchantUnderwriter = merchantUnderwriter;
            _EmailVerifier = emailVerifier;
            _UniqueEmailChecker = uniqueEmailChecker;
            _PasswordHasher = passwordHasher;
        }

        #endregion

        #region Instance Members

        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<UserRegistrationDto> Index(string id)
        {
            UserRegistration userRegistration = await _UserRegistrationRepository.GetByIdAsync(id).ConfigureAwait(false)
                                                ?? throw new NotFoundException(typeof(UserRegistration), id);

            return userRegistration.AsDto();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromBody] CreateUserRegistrationCommand command)
        {
            this.ValidateModel();

            UserRegistration userRegistration = UserRegistration.CreateNewUserRegistration(_UniqueEmailChecker, _PasswordHasher, command);
            await userRegistration.SendEmailVerificationCode(_EmailVerifier).ConfigureAwait(false);

            return Created(Url.Action("Index", $"{nameof(User)}", userRegistration.GetId())!, userRegistration.AsDto());
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyConfirmationCodeCommand command)
        {
            this.ValidateModel();

            UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(command.UserRegistrationId).ConfigureAwait(false)
                                                 ?? throw new NotFoundException(typeof(UserRegistration), command.UserRegistrationId);

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

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([FromBody] UpdateContactCommand command)
        {
            this.ValidateModel();

            UserRegistration userRegistration = await _UserRegistrationRepository.GetByIdAsync(command.Id).ConfigureAwait(false)
                                                ?? throw new NotFoundException(typeof(UserRegistration), command.Id);

            userRegistration.UpdateContactInfo(command);
            await userRegistration.SendSmsVerificationCode(_MobilePhoneVerifier).ConfigureAwait(false);

            return Ok();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhone([FromBody] VerifyConfirmationCodeCommand command)
        {
            this.ValidateModel();

            UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(command.UserRegistrationId).ConfigureAwait(false)
                                                 ?? throw new NotFoundException(typeof(UserRegistration), command.UserRegistrationId);

            userRegistration.VerifyMobilePhone(command);

            return Ok();

            // Redirect exceptions thrown specifically for BusinessRuleValidations, let the Exception Filter take care of the rest
            //try
            //{

            //}
            //catch (BusinessRuleValidationException e)
            //{
            //    _Logger.Log(LogLevel.Error, e.Message);

            //    return View("SmsVerificationFailed");
            //} 
            //return View("SmsVerificationSuccessful");
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Address([FromBody] UpdateAddressCommand command)
        {
            this.ValidateModel();
            UserRegistration userRegistration = await _UserRegistrationRepository.GetByIdAsync(command.Id).ConfigureAwait(false)
                                                ?? throw new NotFoundException(typeof(UserRegistration), command.Id);

            userRegistration.UpdateUserAddress(command);

            return Ok();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersonalDetail(UpdatePersonalDetailCommand command)
        {
            this.ValidateModel();

            UserRegistration userRegistration = await _UserRegistrationRepository.GetByIdAsync(command.Id).ConfigureAwait(false)
                                                ?? throw new NotFoundException(typeof(UserRegistration), command.Id);

            userRegistration.UpdatePersonalDetails(command);
            userRegistration.AnalyzeUserRisk(_MerchantUnderwriter);

            return Ok();
        }

        #endregion
    }
}