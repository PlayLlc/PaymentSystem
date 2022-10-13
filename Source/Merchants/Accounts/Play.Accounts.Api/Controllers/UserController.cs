using System.Net;

using Microsoft.AspNetCore.Mvc;

using Play.Accounts.Api.Models;
using Play.Accounts.Contracts.Dtos;
using Play.Accounts.Domain.Aggregates.Users;
using Play.Domain.Repositories;

namespace Play.Accounts.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        #region Instance Values

        private readonly IRepository<User, string> _UserRepository;

        #endregion

        #region Constructor

        public UserController(IRepository<User, string> userRegistrationRepository)
        {
            _UserRepository = userRegistrationRepository;
        }

        #endregion

        #region Instance Members

        [HttpGet("User/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<Result<UserDto>> GetUser(string id)
        {
            Result<UserDto> result = new Result<UserDto>();

            try
            {
                User? user = await _UserRepository.GetByIdAsync(new UserId(id)).ConfigureAwait(false);

                if (user != null)
                {
                    result.Object = user.AsDto();

                    return result;
                }

                result.Object = new UserDto();
                HttpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;

                result.Success = false;
                result.ErrorMessage = $"No {nameof(User)} could be found with the {nameof(id)}: [id]";

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