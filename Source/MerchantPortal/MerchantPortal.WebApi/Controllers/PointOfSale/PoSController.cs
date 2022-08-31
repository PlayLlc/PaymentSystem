using MerchantPortal.Application.Contracts.Services;
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
        private readonly IDefaultPoSConfigurationService _defaultPoSConfigurationService;

        public PoSController(IPoSConfigurationService posConfigurationService, IDefaultPoSConfigurationService defaultPoSConfigurationService)
        {
            _posConfigurationService = posConfigurationService;
            _defaultPoSConfigurationService = defaultPoSConfigurationService;
        }

        // GET: api/<PointsOfSaleController>
        [HttpGet("terminalConfiguration/{terminalId}")]
        public async Task<PoSConfigurationDto> GetByTerminal(long terminalId)
        {
            return await _posConfigurationService.GetTerminalPoSConfiguration(terminalId);
        }

        // GET api/<PointsOfSaleController>/5
        [HttpGet("{id}")]
        public async Task<PoSConfigurationDto> Get(Guid id)
        {
            return await _posConfigurationService.GetPoSConfiguration(id);
        }

        // POST api/<PointsOfSaleController>
        [HttpPost]
        public void Post([FromBody] Create value)
        {
        }

        // PUT api/<PointsOfSaleController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PointsOfSaleController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
