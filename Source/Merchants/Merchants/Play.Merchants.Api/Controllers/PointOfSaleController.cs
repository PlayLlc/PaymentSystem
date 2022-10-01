using Microsoft.AspNetCore.Mvc;

using Play.Merchants.Contracts.DTO;
using Play.Merchants.Contracts.Messages;
using Play.Merchants.Contracts.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Play.Merchants.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PointOfSaleController : ControllerBase
{
    #region Instance Values

    private readonly IPointOfSaleConfigurationService _PosConfigurationService;

    #endregion

    #region Constructor

    public PointOfSaleController(IPointOfSaleConfigurationService posConfigurationService)
    {
        _PosConfigurationService = posConfigurationService;
    }

    #endregion

    #region Instance Members

    #region Terminal

    [HttpGet("terminal/{terminalId}")]
    public async Task<PointOfSaleConfigurationDto> GetByTerminal(long terminalId)
    {
        return await _PosConfigurationService.GetTerminalConfigurationAsync(terminalId);
    }

    #endregion

    #region Store

    [HttpGet("store/{storeId}")]
    public async Task<IEnumerable<PointOfSaleConfigurationDto>> GetByStore(long storeId)
    {
        return await _PosConfigurationService.GetPosConfigurationByStoreIdAsync(storeId);
    }

    #endregion

    #endregion

    #region PointOfSale

    [HttpGet("{id}")]
    public async Task<PointOfSaleConfigurationDto> Get(Guid id)
    {
        return await _PosConfigurationService.GetPosConfigurationAsync(id);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreatePosConfigurationDto posConfigurationHeader)
    {
        await _PosConfigurationService.CreatePosConfigurationAsync(posConfigurationHeader);

        return Ok();
    }

    [HttpPut("terminalconfiguration/{id}")]
    public async Task<IActionResult> UpdatePoSTerminalConfiguration(Guid id, [FromBody] TerminalConfigurationDto terminalConfiguration)
    {
        await _PosConfigurationService.UpdateTerminalConfigurationAsync(id, terminalConfiguration);

        return Ok();
    }

    [HttpPut("kernelconfiguration/{id}")]
    public async Task<IActionResult> UpdatePoSKernelConfiguration(Guid id, [FromBody] KernelConfigurationDto kernelConfiguration)
    {
        await _PosConfigurationService.UpdateKernelConfigurationAsync(id, kernelConfiguration);

        return Ok();
    }

    [HttpPut("displayconfiguration/{id}")]
    public async Task<IActionResult> UpdateDisplayConfiguration(Guid id, [FromBody] DisplayConfigurationDto displayConfiguration)
    {
        await _PosConfigurationService.UpdateDisplayConfigurationAsync(id, displayConfiguration);

        return Ok();
    }

    [HttpPut("combinationconfiguration/{id}")]
    public async Task<IActionResult> UpdateCombinationsConfiguration(Guid id, [FromBody] IEnumerable<CombinationConfigurationDto> combinations)
    {
        await _PosConfigurationService.UpdateCombinationsConfigurationAsync(id, combinations);

        return Ok();
    }

    [HttpPut("certificateconfiguration/{id}")]
    public async Task<IActionResult> UpdateCertificateAuthorityConfiguration(
        Guid id, [FromBody] CertificateAuthorityConfigurationDto certificateAuthorityConfiguration)
    {
        await _PosConfigurationService.UpdateCertificateAuthorityConfigurationAsync(id, certificateAuthorityConfiguration);

        return Ok();
    }

    #endregion
}