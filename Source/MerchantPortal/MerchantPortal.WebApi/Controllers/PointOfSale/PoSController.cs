using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.DTO;
using MerchantPortal.Application.DTO.PointOfSale;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MerchantPortal.WebApi.Controllers.PointOfSale
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoSController : ControllerBase
    {
        private readonly IPoSConfigurationService _posConfigurationService;

        public PoSController(IPoSConfigurationService posConfigurationService)
        {
            _posConfigurationService = posConfigurationService;
        }

        // GET: api/<PointsOfSaleController>/terminal/{terminalId}
        [HttpGet("terminal/{terminalId}")]
        public async Task<PoSConfigurationDto> GetByTerminal(long terminalId)
        {
            return await _posConfigurationService.GetTerminalPoSConfiguration(terminalId);
        }

        // GET api/<PointsOfSaleController>/store/{storeId}
        [HttpGet("store/{storeId}")]
        public async Task<IEnumerable<PoSConfigurationDto>> GetByStore(long storeId)
        {
            return await _posConfigurationService.GetStorePoSConfigurations(storeId);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePosConfigurationDto posConfigurationHeader)
        {
            await _posConfigurationService.CreateNewPosConfiguration(posConfigurationHeader);

            return Ok();
        }

        // PUT api/<PointsOfSaleController>/5
        [HttpPut("terminalconfiguration/{id}")]
        public async Task<IActionResult> UpdatePoSTerminalConfiguration(long id, [FromBody] TerminalConfigurationDto terminalConfiguration)
        {
            await _posConfigurationService.UpdatePosTerminalConfiguration(id, terminalConfiguration);

            return Ok();
        }

        [HttpPut("kernelconfiguration/{id}")]
        public async Task<IActionResult> UpdatePoSKernelConfiguration(long id, [FromBody] KernelConfigurationDto kernelConfiguration)
        {
            await _posConfigurationService.UpdatePosKernelConfiguration(id, kernelConfiguration);

            return Ok();
        }

        [HttpPut("displayconfiguration/{id}")]
        public async Task<IActionResult> UpdateDisplayConfiguration(long id, [FromBody] DisplayConfigurationDto displayConfiguration)
        {
            await _posConfigurationService.UpdatePosDisplayConfiguration(id, displayConfiguration);

            return Ok();
        }

        [HttpPut("combinationconfiguration/{id}")]
        public async Task<IActionResult> UpdateCombinationsConfiguration(long id, [FromBody] IEnumerable<CombinationDto> combinations)
        {
            await _posConfigurationService.UpdatePosCombinationsConfiguration(id, combinations);

            return Ok();
        }

        [HttpPut("certificateconfiguration/{id}")]
        public async Task<IActionResult> UpdateCertificateAuthorityConfiguration(long id, [FromBody] CertificateAuthorityConfigurationDto certificateAuthorityConfiguration)
        {
            await _posConfigurationService.UpdateCertificateAuthorityConfiguration(id, certificateAuthorityConfiguration);

            return Ok();
        }
    }
}
