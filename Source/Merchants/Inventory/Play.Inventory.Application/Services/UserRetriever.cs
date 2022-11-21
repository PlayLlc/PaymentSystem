using Play.Domain.Exceptions;
using Play.Identity.Contracts.Dtos;
using Play.Inventory.Domain.Entities;
using Play.Inventory.Domain.Services;

using System.Net;

using Play.Identity.Api.Client;
using Play.Restful.Clients;

namespace Play.Inventory.Application.Services;

public class UserRetriever : IRetrieveUsers
{
    #region Instance Values

    private readonly IUserApi _UserApi;

    #endregion

    #region Constructor

    public UserRetriever(IUserApi userApi)
    {
        _UserApi = userApi;
    }

    #endregion

    #region Instance Members

    /// <exception cref="ApiException"></exception>
    public async Task<User> GetByIdAsync(string id)
    {
        try
        {
            UserDto dto = await _UserApi.GetUserAsync(id).ConfigureAwait(false) ?? throw new NotFoundException(typeof(User));

            return new User(dto.Id, dto.MerchantId, dto.IsActive);
        }

        catch (Exception e)
        {
            throw new ApiException(HttpStatusCode.InternalServerError, e);
        }
    }

    /// <exception cref="ApiException"></exception>
    public User GetById(string id)
    {
        try
        {
            UserDto dto = _UserApi.GetUser(id) ?? throw new NotFoundException(typeof(User));

            return new User(dto.Id, dto.MerchantId, dto.IsActive);
        }

        catch (Exception e)
        {
            throw new ApiException(HttpStatusCode.InternalServerError, e);
        }
    }

    #endregion
}