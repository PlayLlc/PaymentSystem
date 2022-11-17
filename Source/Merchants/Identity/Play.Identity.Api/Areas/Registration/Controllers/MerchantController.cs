using Microsoft.AspNetCore.Mvc;

using Play.Identity.Contracts.Commands.MerchantRegistration;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Services;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Mvc.Extensions;

using NotFoundException = Play.Domain.Exceptions.NotFoundException;

namespace Play.Identity.Api.Areas.Registration.Controllers;

[Area($"{nameof(Registration)}")]
[Route("/[area]/[controller]/[action]")]
[ApiController]
public class MerchantController : Controller
{
    #region Instance Values

    private readonly IRepository<MerchantRegistration, SimpleStringId> _MerchantRegistrationRepository;
    private readonly IRepository<Merchant, SimpleStringId> _MerchantRepository;
    private readonly IUnderwriteMerchants _MerchantUnderwriter;

    #endregion

    #region Constructor

    public MerchantController(
        IRepository<MerchantRegistration, SimpleStringId> merchantRegistrationRepository, IRepository<Merchant, SimpleStringId> merchantRepository,
        IUnderwriteMerchants merchantUnderwriter)
    {
        _MerchantRegistrationRepository = merchantRegistrationRepository;
        _MerchantRepository = merchantRepository;
        _MerchantUnderwriter = merchantUnderwriter;
    }

    #endregion

    #region Instance Members

    [Route("~/[area]/[controller]")]
    [HttpGet]
    [ValidateAntiForgeryToken]
    public async Task<MerchantRegistrationDto> Index([FromQuery] string id)
    {
        MerchantRegistration merchantRegistration = await _MerchantRegistrationRepository.GetByIdAsync(new SimpleStringId(id)).ConfigureAwait(false)
                                                    ?? throw new NotFoundException(typeof(MerchantRegistration), id);

        return merchantRegistration.AsDto();
    }

    [Route("~/[area]/[controller]")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index([FromBody] CreateMerchantRegistrationCommand command)
    {
        this.ValidateModel();

        MerchantRegistration merchantRegistration = MerchantRegistration.CreateNewMerchantRegistration(command);

        return Created(Url.Action("Index", $"{nameof(Merchant)}", merchantRegistration.GetId())!, merchantRegistration.AsDto());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve([FromBody] UpdateMerchantRegistrationCommand command)
    {
        this.ValidateModel();

        MerchantRegistration? merchantRegistration = await _MerchantRegistrationRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                                                     ?? throw new NotFoundException(typeof(MerchantRegistration), command.Id);

        merchantRegistration.VerifyMerchantAccount(_MerchantUnderwriter, command);

        if (!merchantRegistration.IsApproved())
            return new ForbidResult();

        Merchant merchant = await _MerchantRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Merchant), command.Id);

        return Created(Url.Action("Index", $"{nameof(Merchant)}", merchant.GetId())!, merchant.AsDto());
    }

    #endregion
}