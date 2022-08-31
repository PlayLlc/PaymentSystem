using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.DTO;
using MerchantPortal.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace MerchantPortal.WebApi.Controllers.Merchant
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantsController : ControllerBase
    {
        private readonly IMerchantConfigurationService _merchantConfigurationService;

        public MerchantsController(IMerchantConfigurationService merchantConfigurationService)
        {
            _merchantConfigurationService = merchantConfigurationService;
        }

        [HttpGet]
        public async Task<MerchantDto> Get(long id)
        {
            var merchant = await _merchantConfigurationService.GetMerchantAsync(id);

            return merchant;
        }

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] MerchantDetails merchantDetails)
        //{

        //}
    }
}
