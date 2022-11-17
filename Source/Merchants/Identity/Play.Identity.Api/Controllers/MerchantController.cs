using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.Repositories;
using Play.Identity.Contracts.Commands;
using Play.Identity.Contracts.Commands.MerchantRegistration;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Services;
using Play.Identity.Persistence.Sql.Repositories;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;

namespace Play.Identity.Api.Controllers
{
    [Authorize]
    [SecurityHeaders]
    [Route("[controller]/[action]")]
    public class MerchantController : Controller
    {
        #region Instance Values

        private readonly IRepository<Merchant, SimpleStringId> _MerchantRepository;
        private readonly IUnderwriteMerchants _MerchantUnderwriter;

        #endregion

        #region Constructor

        public MerchantController(IRepository<Merchant, SimpleStringId> merchantRepository, IUnderwriteMerchants merchantUnderwriter)
        {
            _MerchantRepository = merchantRepository;
            _MerchantUnderwriter = merchantUnderwriter;
        }

        #endregion

        #region Instance Members

        [Route("~/[area]/[controller]")]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<MerchantDto> Index([FromQuery] string id)
        {
            Merchant merchant = await _MerchantRepository.GetByIdAsync(new SimpleStringId(id)).ConfigureAwait(false)
                                ?? throw new NotFoundException(typeof(MerchantRegistration), id);

            return merchant.AsDto();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BusinessInfo([FromBody] UpdateMerchantBusinessInfo command)
        {
            this.ValidateModel();

            var merchant = await _MerchantRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                           ?? throw new NotFoundException(typeof(MerchantRegistration), command.Id);
            merchant.Update(_MerchantUnderwriter, command);

            return Ok();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyName([FromBody] UpdateMerchantCompanyName command)
        {
            this.ValidateModel();

            var merchant = await _MerchantRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                           ?? throw new NotFoundException(typeof(MerchantRegistration), command.Id);
            merchant.Update(_MerchantUnderwriter, command);

            return Ok();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Address([FromBody] UpdateAddressCommand command)
        {
            this.ValidateModel();

            var merchant = await _MerchantRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                           ?? throw new NotFoundException(typeof(MerchantRegistration), command.Id);
            merchant.Update(_MerchantUnderwriter, command);

            return Ok();
        }

        #endregion
    }
}