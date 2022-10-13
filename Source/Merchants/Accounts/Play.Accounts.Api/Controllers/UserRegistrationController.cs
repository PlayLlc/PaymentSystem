using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Api.Models;
using Play.Accounts.Contracts.Commands;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates.UserRegistration;
using Play.Accounts.Domain.Services;
using Play.Domain.Repositories;

namespace Play.Accounts.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRegistrationController : Controller
    {
        #region Instance Values

        private readonly IEnsureUniqueEmails _UniqueEmailChecker;
        private readonly IRepository<UserRegistration, string> _UserRegistrationRepository;

        #endregion

        #region Constructor

        public UserRegistrationController(IEnsureUniqueEmails uniqueEmailChecker, IRepository<UserRegistration, string> userRegistrationRepository)
        {
            _UniqueEmailChecker = uniqueEmailChecker;
            _UserRegistrationRepository = userRegistrationRepository;
        }

        #endregion

        #region Instance Members

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<Result<UserRegistrationDto>> RegisterUser(RegisterUserRequest registerUserRequest)
        {
            Result<UserRegistrationDto> result = new Result<UserRegistrationDto>();

            try
            {
                UserRegistration userRegistration = UserRegistration.CreateNewUserRegistration(registerUserRequest, _UniqueEmailChecker);
                await _UserRegistrationRepository.SaveAsync(userRegistration).ConfigureAwait(false);
                result.Object = userRegistration.AsDto();

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
        public async Task<Result> VerifyEmail(VerifyUserEmailRequest verifyUserEmailRequest)
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