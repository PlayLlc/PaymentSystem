using AutoMapper;
using Play.MerchantPortal.Contracts.Services;
using Play.MerchantPortal.Contracts.DTO;
using MerchantPortal.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MerchantPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantsController : BaseController
    {
        private readonly IMerchantConfigurationService _merchantConfigurationService;

        public MerchantsController(IMerchantConfigurationService merchantConfigurationService, IMapper mapper) : base(mapper)
        {
            _merchantConfigurationService = merchantConfigurationService;
        }

        [HttpGet]
        public async Task<MerchantDto> Get(long id)
        {
            var merchant = await _merchantConfigurationService.GetMerchantAsync(id);

            return merchant;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InsertMerchantRequest value)
        {
            long created = await _merchantConfigurationService.InsertMerchantAsync(_mapper.Map<MerchantDto>(value));

            return Created(created.ToString(), null);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateMerchantRequest value)
        {
            await _merchantConfigurationService.UpdateMerchantAsync(_mapper.Map<MerchantDto>(value));

            return Ok();
        }
    }
}
