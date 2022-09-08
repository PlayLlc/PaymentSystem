using Microsoft.AspNetCore.Mvc;
using Play.MerchantPortal.Contracts.DTO.PointOfSale;
using Play.MerchantPortal.Contracts.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Play.MerchantPortal.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PosController : ControllerBase
    {
        #region Instance Values

        private readonly IPosConfigurationService _posConfigurationService;

        #endregion

        #region Constructors

        public PosController(IPosConfigurationService posConfigurationService)
        {
            _posConfigurationService = posConfigurationService;
        }

        #endregion

        #region Instance Members

        // GET: api/<PointsOfSaleController>/terminal/{terminalId}
        [HttpGet("terminal/{terminalId}")]
        public async Task<PosConfigurationDto> GetByTerminal(long terminalId)
        {
            return await _posConfigurationService.GetTerminalPoSConfigurationAsync(terminalId);
        }

        // GET api/<PointsOfSaleController>/store/{storeId}
        [HttpGet("store/{storeId}")]
        public async Task<IEnumerable<PosConfigurationDto>> GetByStore(long storeId)
        {
            return await _posConfigurationService.GetStorePoSConfigurationsAsync(storeId);
        }

        [HttpGet("{id}")]
        public async Task<PosConfigurationDto> Get(Guid id)
        {
            return await _posConfigurationService.GetPoSConfigurationAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePosConfigurationDto posConfigurationHeader)
        {
            await _posConfigurationService.CreateNewPosConfiguratioAsync(posConfigurationHeader);

            return Ok();
        }

        // PUT api/<PointsOfSaleController>/5
        [HttpPut("terminalconfiguration/{id}")]
        public async Task<IActionResult> UpdatePoSTerminalConfiguration(Guid id, [FromBody] TerminalConfigurationDto terminalConfiguration)
        {
            await _posConfigurationService.UpdateTerminalConfigurationAsync(id, terminalConfiguration);

            return Ok();
        }

        [HttpPut("kernelconfiguration/{id}")]
        public async Task<IActionResult> UpdatePoSKernelConfiguration(Guid id, [FromBody] KernelConfigurationDto kernelConfiguration)
        {
            await _posConfigurationService.UpdateKernelConfigurationAsync(id, kernelConfiguration);

            return Ok();
        }

        [HttpPut("displayconfiguration/{id}")]
        public async Task<IActionResult> UpdateDisplayConfiguration(Guid id, [FromBody] DisplayConfigurationDto displayConfiguration)
        {
            await _posConfigurationService.UpdateDisplayConfigurationAsync(id, displayConfiguration);

            return Ok();
        }

        [HttpPut("combinationconfiguration/{id}")]
        public async Task<IActionResult> UpdateCombinationsConfiguration(Guid id, [FromBody] IEnumerable<CombinationConfigurationDto> combinations)
        {
            await _posConfigurationService.UpdateCombinationsConfigurationAsync(id, combinations);

            return Ok();
        }

        [HttpPut("certificateconfiguration/{id}")]
        public async Task<IActionResult> UpdateCertificateAuthorityConfiguration(Guid id, [FromBody] CertificateAuthorityConfigurationDto certificateAuthorityConfiguration)
        {
            await _posConfigurationService.UpdateCertificateAuthorityConfigurationAsync(id, certificateAuthorityConfiguration);

            return Ok();
        }

        #endregion
    }
}