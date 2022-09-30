using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Play.Merchants.Api.Models;
using Play.Merchants.Contracts.DTO;
using Play.Merchants.Contracts.Services;

namespace Play.Merchants.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MerchantsController : BaseController
{
    #region Instance Values

    private readonly IMerchantConfigurationService _MerchantConfigurationService;

    #endregion

    #region Constructor

    public MerchantsController(IMerchantConfigurationService merchantConfigurationService, IMapper mapper) : base(mapper)
    {
        _MerchantConfigurationService = merchantConfigurationService;
    }

    #endregion

    #region Instance Members

    [HttpGet]
    public async Task<MerchantDto> Get(long id)
    {
        MerchantDto merchant = await _MerchantConfigurationService.GetMerchantAsync(id);

        return merchant;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] InsertMerchantRequest value)
    {
        long created = await _MerchantConfigurationService.InsertMerchantAsync(_Mapper.Map<MerchantDto>(value));

        return Created(created.ToString(), null);
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateMerchantRequest value)
    {
        await _MerchantConfigurationService.UpdateMerchantAsync(_Mapper.Map<MerchantDto>(value));

        return Ok();
    }

    #endregion
}