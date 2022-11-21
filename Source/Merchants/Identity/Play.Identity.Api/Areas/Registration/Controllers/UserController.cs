using Microsoft.AspNetCore.Mvc;

using Play.Identity.Contracts.Commands;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Repositories;
using Play.Identity.Domain.Services;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Mvc.Extensions;
using Play.Identity.Contracts.Commands.UserRegistration;
using Play.Mvc.Attributes;

namespace Play.Identity.Api.Areas.Registration.Controllers;

[Area($"{nameof(Registration)}")]
[Route("[area]/[controller]/[action]")]
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

    [HttpGetSwagger(template: "~/[area]/[controller]")]
    [ValidateAntiForgeryToken]
    public async Task<UserRegistrationDto> Get([FromQuery] string id)
    {
        UserRegistration userRegistration = await _UserRegistrationRepository.GetByIdAsync(new SimpleStringId(id)).ConfigureAwait(false)
                                            ?? throw new NotFoundException(typeof(UserRegistration));

        return userRegistration.AsDto();
    }

    [HttpPostSwagger(template: "~/[area]/[controller]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromBody] CreateUserRegistrationCommand command)
    {
        this.ValidateModel();

        UserRegistration userRegistration = UserRegistration.CreateNewUserRegistration(_UniqueEmailChecker, _PasswordHasher, command);

        await userRegistration.SendEmailVerificationCode(_EmailVerifier).ConfigureAwait(false);

        return Created(@Url.Action("Index", "User", new
        {
            area = nameof(Registration),
            id = userRegistration.Id
        })!, userRegistration.AsDto());
    }

    [HttpGetSwagger("RegistrationGetEmailVerificationForUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetEmailVerification([FromQuery] string id)
    {
        UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(new SimpleStringId(id)).ConfigureAwait(false)
                                             ?? throw new NotFoundException(typeof(UserRegistration));

        if (userRegistration.HasEmailBeenVerified())
            return View("EmailVerificationSuccessful");

        return View("EmailVerificationFailed");
    }

    [HttpPutSwagger("RegistrationUpdateEmailVerificationForUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateEmailVerification([FromBody] VerifyConfirmationCodeCommand command)
    {
        this.ValidateModel();

        UserRegistration? userRegistration =
            await _UserRegistrationRepository.GetByIdAsync(new SimpleStringId(command.UserRegistrationId)).ConfigureAwait(false)
            ?? throw new NotFoundException(typeof(UserRegistration), command.UserRegistrationId);

        userRegistration.VerifyEmail(command);

        return Ok();
    }

    [HttpPutSwagger("RegistrationUpdateContactForUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateContact([FromBody] UpdateContactCommand command)
    {
        this.ValidateModel();

        UserRegistration userRegistration = await _UserRegistrationRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                                            ?? throw new NotFoundException(typeof(UserRegistration));

        userRegistration.UpdateContactInfo(command);
        await userRegistration.SendSmsVerificationCode(_MobilePhoneVerifier).ConfigureAwait(false);

        return Ok();
    }

    [HttpGetSwagger("RegistrationGetPhoneVerificationForUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetPhoneVerification([FromQuery] string id)
    {
        UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(new SimpleStringId(id)).ConfigureAwait(false)
                                             ?? throw new NotFoundException(typeof(UserRegistration));

        if (userRegistration.HasEmailBeenVerified())
            return View("EmailVerificationSuccessful");

        return View("EmailVerificationFailed");
    }

    [HttpPutSwagger("RegistrationUpdatePhoneVerificationForUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePhoneVerification([FromBody] VerifyConfirmationCodeCommand command)
    {
        this.ValidateModel();

        UserRegistration? userRegistration =
            await _UserRegistrationRepository.GetByIdAsync(new SimpleStringId(command.UserRegistrationId)).ConfigureAwait(false)
            ?? throw new NotFoundException(typeof(UserRegistration));

        userRegistration.VerifyMobilePhone(command);

        return Ok();
    }

    [HttpPutSwagger("RegistrationUpdateAddressForUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressCommand command)
    {
        this.ValidateModel();
        UserRegistration userRegistration = await _UserRegistrationRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                                            ?? throw new NotFoundException(typeof(UserRegistration));

        userRegistration.UpdateUserAddress(command);

        return Ok();
    }

    [HttpPutSwagger("RegistrationUpdatePersonalDetailsForUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePersonalDetails([FromBody] UpdatePersonalDetailCommand command)
    {
        this.ValidateModel();

        UserRegistration userRegistration = await _UserRegistrationRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                                            ?? throw new NotFoundException(typeof(UserRegistration));

        userRegistration.UpdatePersonalDetails(command);

        return Ok();
    }

    [HttpPostSwagger]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve([FromQuery] string id)
    {
        this.ValidateModel();

        UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(new SimpleStringId(id)).ConfigureAwait(false)
                                             ?? throw new NotFoundException(typeof(MerchantRegistration));

        userRegistration.AnalyzeUserRisk(_MerchantUnderwriter);

        if (!userRegistration.IsApproved())
            return new ForbidResult();

        return Created(Url.Action("Index", $"{nameof(User)}", userRegistration.GetId())!, userRegistration.AsDto());
    }

    #endregion
}