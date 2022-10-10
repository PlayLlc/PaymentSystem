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
        public async Task<Response<UserDto>> GetUser(string id)
        {
            Response<UserDto> response = new Response<UserDto>();

            try
            {
                User? user = await _UserRepository.GetByIdAsync(new UserId(id)).ConfigureAwait(false);

                if (user != null)
                {
                    response.Object = user.AsDto();

                    return response;
                }

                response.Object = new UserDto();
                HttpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;

                response.Success = false;
                response.ErrorMessage = $"No {nameof(User)} could be found with the {nameof(id)}: [id]";

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