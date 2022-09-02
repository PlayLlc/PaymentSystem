using AutoMapper;
using MerchantPortal.Application.Contracts.Services;
using MerchantPortal.Application.DTO;
using MerchantPortal.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MerchantPortal.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : BaseController
    {
        private readonly IStoreConfigurationService _storeConfigurationService;

        public StoresController(IStoreConfigurationService storeConfigurationService, IMapper mapper) : base(mapper)
        {
            _storeConfigurationService = storeConfigurationService;
        }

        // GET: api/<StoreController>/merchantStores/{merchantId}
        [HttpGet("merchantStores/{merchantId}")]
        public async Task<IEnumerable<StoreDto>> GetMerchantStores(long merchantId)
        {
            return await _storeConfigurationService.GetMerchantStoresAsync(merchantId);
        }

        // GET api/<StoreController>/5
        [HttpGet("{id}")]
        public async Task<StoreDto> Get(long id)
        {
            return await _storeConfigurationService.GetStoreAsync(id);
        }

        // POST api/<StoreController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StoreRequest value)
        {
            var created = await _storeConfigurationService.InsertStoreAsync(_mapper.Map<StoreDto>(value));

            return Created(created.ToString(), null);
        }

        // PUT api/<StoreController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromBody] StoreRequest value)
        {
            await _storeConfigurationService.UpdateStoreAsync(_mapper.Map<StoreDto>(value));

            return Ok();
        }

        // DELETE api/<StoreController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _storeConfigurationService.DeleteStoreAsync(id);

            return NoContent();
        }
    }
}
