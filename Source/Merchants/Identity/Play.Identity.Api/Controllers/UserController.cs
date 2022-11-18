using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.Repositories;
using Play.Identity.Application.Services;
using Play.Identity.Contracts.Commands;
using Play.Identity.Contracts.Dtos;
using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Repositories;
using Play.Identity.Domain.Services;
using Play.Identity.Persistence.Sql.Repositories;
using Play.Mvc.Attributes;
using Play.Mvc.Extensions;

namespace Play.Identity.Api.Controllers
{
    [Authorize]
    [SecurityHeaders]
    [Route("[controller]/[action]")]
    public class UserController : Controller
    {
        #region Instance Values

        private readonly IUserRepository _UserRepository;

        private readonly IUnderwriteMerchants _MerchantUnderwriter;

        #endregion

        #region Constructor

        public UserController(IUserRepository userRepository, IUnderwriteMerchants merchantUnderwriter)
        {
            _UserRepository = userRepository;
            _MerchantUnderwriter = merchantUnderwriter;
        }

        #endregion

        #region Instance Members

        [Route("~/[controller]/{id=id}")]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<UserDto> Index([FromQuery] string id)
        {
            User user = await _UserRepository.GetByIdAsync(new SimpleStringId(id)).ConfigureAwait(false)
                        ?? throw new NotFoundException(typeof(MerchantRegistration), id);

            return user.AsDto();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Address([FromBody] UpdateAddressCommand command)
        {
            this.ValidateModel();

            var user = await _UserRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                       ?? throw new NotFoundException(typeof(MerchantRegistration), command.Id);

            user.Update(_MerchantUnderwriter, command);

            return Ok();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactInfo([FromBody] UpdateContactCommand command)
        {
            this.ValidateModel();

            var user = await _UserRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                       ?? throw new NotFoundException(typeof(MerchantRegistration), command.Id);

            user.Update(_MerchantUnderwriter, command);

            return Ok();
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PersonalDetails([FromBody] UpdatePersonalDetailCommand command)
        {
            this.ValidateModel();

            var user = await _UserRepository.GetByIdAsync(new SimpleStringId(command.Id)).ConfigureAwait(false)
                       ?? throw new NotFoundException(typeof(MerchantRegistration), command.Id);

            user.Update(_MerchantUnderwriter, command);

            return Ok();
        }

        #endregion
    }
}