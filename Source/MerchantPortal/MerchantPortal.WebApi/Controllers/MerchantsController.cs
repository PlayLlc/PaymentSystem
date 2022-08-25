using MerchantPortal.Application.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MerchantPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantsController : ControllerBase
    {
        private readonly IMerchantConfigurationService _MerchantConfigurationService;

        public MerchantsController(IMerchantConfigurationService merchantConfigurationService)
        {
            _MerchantConfigurationService = merchantConfigurationService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(long id)
        {
            return Ok();
        }
    }
}
