using Microsoft.AspNetCore.Mvc;

using Play.Domain.Common.ValueObjects;
using Play.Domain.Exceptions;
using Play.Domain.Repositories;
using Play.Identity.Domain.Aggregates;
using Play.Identity.Domain.Repositories;
using Play.Mvc.Attributes;

namespace Play.Identity.Api.Areas.Registration.Controllers
{
    [Area($"{nameof(Registration)}")]
    [Route("/[area]/[action]")]
    [ApiController]
    public class HomeController : Controller
    {
        #region Instance Values

        private readonly IUserRegistrationRepository _UserRegistrationRepository;
        private readonly IRepository<MerchantRegistration, SimpleStringId> _MerchantRegistrationRepository;

        #endregion

        #region Constructor

        public HomeController(
            IUserRegistrationRepository userRegistrationRepository, IRepository<MerchantRegistration, SimpleStringId> merchantRegistrationRepository)
        {
            _UserRegistrationRepository = userRegistrationRepository;
            _MerchantRegistrationRepository = merchantRegistrationRepository;
        }

        #endregion

        #region Instance Members

        [HttpPostSwagger(template: "/[area]/[action]")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Complete([FromQuery] string userRegistrationId)
        {
            UserRegistration? userRegistration = await _UserRegistrationRepository.GetByIdAsync(new SimpleStringId(userRegistrationId)).ConfigureAwait(false)
                                                 ?? throw new NotFoundException(typeof(UserRegistration));
            MerchantRegistration? merchantRegistration =
                await _MerchantRegistrationRepository.GetByIdAsync(new SimpleStringId(userRegistration!.GetMerchantId()))
                ?? throw new NotFoundException(typeof(MerchantRegistration));

            User user = userRegistration.CreateUser();
            _ = merchantRegistration.CreateMerchant();

            // TODO: Create Store
            // TODO: Create Terminal

            return Created(@Url.Action("Index", "User", user.AsDto())!, user.AsDto());
        }

        #endregion
    }
}