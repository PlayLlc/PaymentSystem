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
    public class StoresController : BaseController
    {
        #region Instance Values

        private readonly IStoreConfigurationService _StoreConfigurationService;

        #endregion

        #region Constructor

        public StoresController(IStoreConfigurationService storeConfigurationService, IMapper mapper) : base(mapper)
        {
            _StoreConfigurationService = storeConfigurationService;
        }

        #endregion

        #region Instance Members

        // GET: api/<StoreController>/merchantStores/{merchantId}
        [HttpGet("merchantStores/{merchantId}")]
        public async Task<IEnumerable<StoreDto>> GetMerchantStores(long merchantId)
        {
            return await _StoreConfigurationService.GetMerchantStoresAsync(merchantId);
        }

        // GET api/<StoreController>/5
        [HttpGet("{id}")]
        public async Task<StoreDto> Get(long id)
        {
            return await _StoreConfigurationService.GetStoreAsync(id);
        }

        // POST api/<StoreController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StoreDetailsRequest value)
        {
            var created = await _StoreConfigurationService.InsertStoreAsync(_Mapper.Map<StoreDto>(value));

            return Created(created.ToString(), null);
        }

        // PUT api/<StoreController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] StoreDetailsRequest value)
        {
            await _StoreConfigurationService.UpdateStoreAsync(_Mapper.Map<StoreDto>(value));

            return Ok();
        }

        // DELETE api/<StoreController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _StoreConfigurationService.DeleteStoreAsync(id);

            return NoContent();
        }

        #endregion
    }
}