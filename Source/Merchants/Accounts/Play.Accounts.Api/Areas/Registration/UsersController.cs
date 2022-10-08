using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Api.Models;
using Play.Accounts.Api.Services;
using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Dtos;
using Play.Domain.Repositories;
using Play.Merchants.Onboarding.Domain.Aggregates;
using Play.Merchants.Onboarding.Domain.Services;

namespace Play.Accounts.Api.Areas.Registration
{
    [Area($"{nameof(Registration)}")]
    [Route("[area]/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        #region Instance Values

        private readonly IRepository<UserRegistration, UserRegistrationId> _UserRegistrationRepository;

        private readonly IIdentityService _IdentityService;

        #endregion

        #region Constructor

        public UsersController(IRepository<UserRegistration, UserRegistrationId> userRegistrationRepository)
        {
            _UserRegistrationRepository = userRegistrationRepository;
        }

        #endregion

        #region Instance Members

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response<UserRegistrationDto>> RegisterUser([FromBody] RegisterUserRequest registerUserRequest)
        {
            Response<UserRegistrationDto> response = new Response<UserRegistrationDto>();

            if (ModelState.IsValid)

                try
                {
                    await _IdentityService.RegisterUserAsync(registerUserRequest).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    response.Errored = true;
                    response.ErrorMessage = e.Message;

                    return response;
                }

            return response;
        }

        public async Task<Response> ValidateEmail(ValidateEmailRequest request)
        {
            Response response = new Response();

            try
            {
                await _IdentityService.ValidateUsername(request).ConfigureAwait(false);

                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.ErrorMessage = e.Message;

                return response;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Response> VerifyEmail(VerifyUserEmailRequest request)
        {
            Response<UserRegistrationDto> response = new Response<UserRegistrationDto>();

            try
            {
                // Logic

                return response;
            }
            catch (Exception e)
            {
                response.Success = false;
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
                response.Success = false;
                response.ErrorMessage = e.Message;

                return response;
            }
        }

        #endregion
    }
}