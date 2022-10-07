using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Api.Models;
using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Dtos;
using Play.Domain.Repositories;
using Play.Merchants.Onboarding.Domain.Aggregates;
using Play.Merchants.Onboarding.Domain.Services;

namespace Play.Accounts.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRegistrationController : Controller
    {
        #region Instance Values

        private readonly IEnsureUniqueEmails _UniqueEmailChecker;
        private readonly IRepository<UserRegistration, UserRegistrationId> _UserRegistrationRepository;

        #endregion

        #region Constructor

        public UserRegistrationController(IEnsureUniqueEmails uniqueEmailChecker, IRepository<UserRegistration, UserRegistrationId> userRegistrationRepository)
        {
            _UniqueEmailChecker = uniqueEmailChecker;
            _UserRegistrationRepository = userRegistrationRepository;
        }

        #endregion

        #region Instance Members

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response<UserRegistrationDto>> RegisterUser(RegisterUserRequest registerUserRequest)
        {
            Response<UserRegistrationDto> response = new Response<UserRegistrationDto>();

            try
            {
                UserRegistration userRegistration = UserRegistration.CreateNewUserRegistration(registerUserRequest, _UniqueEmailChecker);
                await _UserRegistrationRepository.SaveAsync(userRegistration).ConfigureAwait(false);
                response.Object = userRegistration.AsDto();

                return response;
            }
            catch (Exception e)
            {
                response.Errored = true;
                response.ErrorMessage = e.Message;

                return response;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response> VerifyEmail(VerifyUserEmailRequest verifyUserEmailRequest)
        {
            Response<UserRegistrationDto> response = new Response<UserRegistrationDto>();

            try
            {
                // Logic

                return response;
            }
            catch (Exception e)
            {
                response.Errored = true;
                response.ErrorMessage = e.Message;

                return response;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response> VerifyMobile(VerifyUserMobileRequest verifyUserEmailRequest)
        {
            Response<UserRegistrationDto> response = new Response<UserRegistrationDto>();

            try
            {
                // Logic

                return response;
            }
            catch (Exception e)
            {
                response.Errored = true;
                response.ErrorMessage = e.Message;

                return response;
            }
        }

        #endregion
    }
}