using AutoMapper;
using Play.MerchantPortal.Contracts.Services;
using Play.MerchantPortal.Contracts.DTO;
using MerchantPortal.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MerchantPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerminalsController : BaseController
    {
        private readonly ITerminalConfigurationService _terminalConfigurationService;

        public TerminalsController(ITerminalConfigurationService terminalConfigurationService, IMapper mapper) : base(mapper)
        {
            _terminalConfigurationService = terminalConfigurationService;
        }

        [HttpGet("storeTerminals/{storeId}")]
        public async Task<IEnumerable<TerminalDto>> GetStoreTerminals(long storeId)
        {
            return await _terminalConfigurationService.GetStoreTerminalsAsync(storeId);
        }

        // GET: api/<TerminalController>
        [HttpGet("{id}")]
        public async Task<TerminalDto> Get(long id)
        {
            return await _terminalConfigurationService.GetTerminalAsync(id);
        }

        // POST api/<TerminalController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TerminalDetailsRequest value)
        {
            var created = await _terminalConfigurationService.InsertTerminalAsync(_mapper.Map<TerminalDto>(value));

            return Created(created.ToString(), null);
        }

        // PUT api/<TerminalController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] TerminalDetailsRequest value)
        {
            await _terminalConfigurationService.UpdateTerminalAsync(_mapper.Map<TerminalDto>(value));

            return Ok();
        }

        // DELETE api/<TerminalController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _terminalConfigurationService.DeleteTerminalAsync(id);

            return NoContent();
        }
    }
}
