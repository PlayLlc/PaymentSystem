using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Api.Models;
using Play.Accounts.Api.Services;
using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates.UserRegistration;
using Play.Domain.Repositories;

using Result = Play.Accounts.Api.Models.Result;

namespace Play.Accounts.Api.Areas.Registration
{
    [Area($"{nameof(Registration)}")]
    [Route("[area]/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        #region Instance Values

        private readonly IRepository<UserRegistration, string> _UserRegistrationRepository;

        private readonly IIdentityService _IdentityService;

        #endregion

        #region Constructor

        public UsersController(IRepository<UserRegistration, string> userRegistrationRepository)
        {
            _UserRegistrationRepository = userRegistrationRepository;
        }

        #endregion

        #region Instance Members

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Result<UserRegistrationDto>> RegisterUser([FromBody] RegisterUserRequest registerUserRequest)
        {
            Result<UserRegistrationDto> result = new Result<UserRegistrationDto>();

            if (ModelState.IsValid)

                try
                {
                    await _IdentityService.RegisterUserAsync(registerUserRequest).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    result.Success = false;
                    result.ErrorMessage = e.Message;

                    return result;
                }

            return result;
        }

        public async Task<Result> ValidateEmail(ValidateEmailRequest request)
        {
            Result result = new Result();

            try
            {
                await _IdentityService.ValidateUsername(request).ConfigureAwait(false);

                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorMessage = e.Message;

                return result;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Result> VerifyEmail(VerifyUserEmailRequest request)
        {
            Result<UserRegistrationDto> result = new Result<UserRegistrationDto>();

            try
            {
                // Logic

                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorMessage = e.Message;

                return result;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Result> VerifyMobile(VerifyUserMobileRequest verifyUserEmailRequest)
        {
            Result<UserRegistrationDto> result = new Result<UserRegistrationDto>();

            try
            {
                // Logic

                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.ErrorMessage = e.Message;

                return result;
            }
        }

        #endregion
    }
}