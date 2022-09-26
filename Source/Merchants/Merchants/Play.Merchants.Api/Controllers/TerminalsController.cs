using AutoMapper;

using Microsoft.AspNetCore.Mvc;

using Play.Merchants.Api.Models;
using Play.Merchants.Contracts.DTO;
using Play.Merchants.Contracts.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Play.Merchants.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerminalsController : BaseController
    {
        #region Instance Values

        private readonly ITerminalConfigurationService _TerminalConfigurationService;

        #endregion

        #region Constructor

        public TerminalsController(ITerminalConfigurationService terminalConfigurationService, IMapper mapper) : base(mapper)
        {
            _TerminalConfigurationService = terminalConfigurationService;
        }

        #endregion

        #region Instance Members

        [HttpGet("storeTerminals/{storeId}")]
        public async Task<IEnumerable<TerminalDto>> GetStoreTerminals(long storeId)
        {
            return await _TerminalConfigurationService.GetStoreTerminalsAsync(storeId);
        }

        // GET: api/<TerminalController>
        [HttpGet("{id}")]
        public async Task<TerminalDto> Get(long id)
        {
            return await _TerminalConfigurationService.GetTerminalAsync(id);
        }

        // POST api/<TerminalController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TerminalDetailsRequest value)
        {
            long created = await _TerminalConfigurationService.InsertTerminalAsync(_Mapper.Map<TerminalDto>(value));

            return Created(created.ToString(), null);
        }

        // PUT api/<TerminalController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromBody] TerminalDetailsRequest value)
        {
            await _TerminalConfigurationService.UpdateTerminalAsync(_Mapper.Map<TerminalDto>(value));

            return Ok();
        }

        // DELETE api/<TerminalController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await _TerminalConfigurationService.DeleteTerminalAsync(id);

            return NoContent();
        }

        #endregion
    }
}