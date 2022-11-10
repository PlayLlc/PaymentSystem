using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Contracts.Commands.MerchantRegistration;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates;
using Play.Accounts.Domain.Services;
using Play.Domain.Repositories;
using Play.Mvc.Extensions;

using NotFoundException = Play.Domain.Exceptions.NotFoundException;

namespace Play.Identity.Api.Areas.Registration.Controllers;

[Area($"{nameof(Registration)}")]
[Route("[area]/[controller]/[action]")]

//[ApiController]
public class MerchantController : Controller
{
    #region Instance Values

    private readonly ILogger<UserController> _Logger;
    private readonly IRepository<MerchantRegistration, string> _MerchantRegistrationRepository;
    private readonly IRepository<Merchant, string> _MerchantRepository;
    private readonly IUnderwriteMerchants _MerchantUnderwriter;

    #endregion

    #region Constructor

    public MerchantController(
        ILogger<UserController> logger, IRepository<MerchantRegistration, string> merchantRegistrationRepository,
        IRepository<Merchant, string> merchantRepository, IUnderwriteMerchants merchantUnderwriter)
    {
        _Logger = logger;
        _MerchantRegistrationRepository = merchantRegistrationRepository;
        _MerchantRepository = merchantRepository;
        _MerchantUnderwriter = merchantUnderwriter;
    }

    #endregion

    #region Instance Members

    [HttpGet("")]
    [ValidateAntiForgeryToken]
    public async Task<MerchantRegistrationDto> Index([FromQuery] string id)
    {
        MerchantRegistration merchantRegistration = await _MerchantRegistrationRepository.GetByIdAsync(id).ConfigureAwait(false)
                                                    ?? throw new NotFoundException(typeof(MerchantRegistration), id);

        return merchantRegistration.AsDto();
    }

    [HttpPost("")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index([FromBody] CreateMerchantRegistrationCommand command)
    {
        this.ValidateModel();

        MerchantRegistration merchantRegistration = MerchantRegistration.CreateNewMerchantRegistration(command);

        return Created(Url.Action("Index", $"{nameof(Merchant)}", merchantRegistration.GetId())!, merchantRegistration.AsDto());
    }

    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve([FromQuery] string id)
    {
        MerchantRegistration? merchantRegistration = await _MerchantRegistrationRepository.GetByIdAsync(id).ConfigureAwait(false)
                                                     ?? throw new NotFoundException(typeof(UserRegistration), id);

        if (merchantRegistration.IsApproved())
            return View("RegistrationSuccessful");

        return View("RegistrationFailed");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve([FromBody] UpdateMerchantRegistrationCommand command)
    {
        this.ValidateModel();

        MerchantRegistration? merchantRegistration = await _MerchantRegistrationRepository.GetByIdAsync(command.Id).ConfigureAwait(false)
                                                     ?? throw new NotFoundException(typeof(MerchantRegistration), command.Id);

        merchantRegistration.VerifyMerchantAccount(_MerchantUnderwriter, command);

        if (!merchantRegistration.IsApproved())
            return new ForbidResult();

        Merchant merchant = await _MerchantRepository.GetByIdAsync(command.Id).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Merchant), command.Id);

        return Created(Url.Action("Index", $"{nameof(Merchant)}", merchant.GetId())!, merchant.AsDto());
    }

    #endregion
}