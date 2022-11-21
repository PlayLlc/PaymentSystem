using Microsoft.AspNetCore.Mvc;

using Play.Identity.Contracts.Commands.MerchantRegistration;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Services;
using Play.Domain.Common.ValueObjects;
using Play.Domain.Repositories;
using Play.Identity.Domain.Repositories;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;

using NotFoundException = Play.Domain.Exceptions.NotFoundException;

namespace Play.Identity.Api.Areas.Registration.Controllers;

[Area($"{nameof(Registration)}")]
[Route("/[area]/[controller]/[action]")]
[ApiController]
public class MerchantController : Controller
{
    #region Instance Values

    private readonly IUserRegistrationRepository _UserRegistrationRepository;
    private readonly IRepository<MerchantRegistration, SimpleStringId> _MerchantRegistrationRepository;
    private readonly IRepository<Merchant, SimpleStringId> _MerchantRepository;
    private readonly IUnderwriteMerchants _MerchantUnderwriter;

    #endregion

    #region Constructor

    public MerchantController(
        IRepository<MerchantRegistration, SimpleStringId> merchantRegistrationRepository, IRepository<Merchant, SimpleStringId> merchantRepository,
        IUnderwriteMerchants merchantUnderwriter, IUserRegistrationRepository userRegistrationRepository)
    {
        _MerchantRegistrationRepository = merchantRegistrationRepository;
        _MerchantRepository = merchantRepository;
        _MerchantUnderwriter = merchantUnderwriter;
        _UserRegistrationRepository = userRegistrationRepository;
    }

    #endregion

    #region Instance Members

    [HttpGetSwagger(template: "~/[area]/[controller]")]
    [ValidateAntiForgeryToken]
    public async Task<MerchantRegistrationDto> Get([FromQuery] string id)
    {
        MerchantRegistration merchantRegistration = await _MerchantRegistrationRepository.GetByIdAsync(new SimpleStringId(id)).ConfigureAwait(false)
                                                    ?? throw new NotFoundException(typeof(MerchantRegistration), id);

        return merchantRegistration.AsDto();
    }

    [HttpPostSwagger(template: "~/[area]/[controller]")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromBody] CreateMerchantRegistrationCommand command)
    {
        this.ValidateModel();

        MerchantRegistration merchantRegistration = MerchantRegistration.CreateNewMerchantRegistration(_UserRegistrationRepository, command);

        return Created(@Url.Action("Index", "Merchant", new
        {
            area = nameof(Registration),
            id = merchantRegistration.Id
        })!, merchantRegistration.AsDto());
    }

    [HttpPostSwagger]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve([FromBody] UpdateMerchantRegistrationCommand command)
    {
        this.ValidateModel();

        MerchantRegistration? merchantRegistration = await _MerchantRegistrationRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                                                     ?? throw new NotFoundException(typeof(MerchantRegistration));

        merchantRegistration.VerifyMerchantAccount(_MerchantUnderwriter, command);

        if (!merchantRegistration.IsApproved())
            return new ForbidResult();

        Merchant merchant = await _MerchantRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                            ?? throw new NotFoundException(typeof(Merchant));

        return Created(Url.Action("Index", $"{nameof(Merchant)}", merchant.GetId())!, merchant.AsDto());
    }

    #endregion
}