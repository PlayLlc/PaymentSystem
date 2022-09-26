using Microsoft.AspNetCore.Mvc;

using Play.Merchants.Contracts.Messages;
using Play.Merchants.Contracts.Messages.PointOfSale;
using Play.Merchants.Contracts.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Play.Merchants.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointOfSaleController : ControllerBase
    {
        #region Instance Values

        private readonly IPointOfSaleConfigurationService _posConfigurationService;

        #endregion

        #region Constructor

        public PointOfSaleController(IPointOfSaleConfigurationService posConfigurationService)
        {
            _posConfigurationService = posConfigurationService;
        }

        #endregion

        #region Instance Members

        #region Terminal

        [HttpGet("terminal/{terminalId}")]
        public async Task<PointOfSaleConfigurationDto> GetByTerminal(long terminalId)
        {
            return await _posConfigurationService.GetTerminalPoSConfigurationAsync(terminalId);
        }

        #endregion

        #region Store

        [HttpGet("store/{storeId}")]
        public async Task<IEnumerable<PointOfSaleConfigurationDto>> GetByStore(long storeId)
        {
            return await _posConfigurationService.GetStorePoSConfigurationsAsync(storeId);
        }

        #endregion

        #endregion

        #region PointOfSale

        [HttpGet("{id}")]
        public async Task<PointOfSaleConfigurationDto> Get(Guid id)
        {
            return await _posConfigurationService.GetPoSConfigurationAsync(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePosConfigurationDto posConfigurationHeader)
        {
            await _posConfigurationService.CreateNewPosConfiguratioAsync(posConfigurationHeader);

            return Ok();
        }

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
        public async Task<IActionResult> UpdateCertificateAuthorityConfiguration(
            Guid id, [FromBody] CertificateAuthorityConfigurationDto certificateAuthorityConfiguration)
        {
            await _posConfigurationService.UpdateCertificateAuthorityConfigurationAsync(id, certificateAuthorityConfiguration);

            return Ok();
        }

        #endregion
    }
}