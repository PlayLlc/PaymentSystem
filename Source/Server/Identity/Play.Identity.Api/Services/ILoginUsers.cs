using Play.Core;
using Play.Identity.Domain.Aggregates;

namespace Play.Identity.Api.Services;

public interface ILoginUsers
{
    #region Instance Members

    public Task<Result> LoginAsync(HttpContext context, User user, string clearTextPassword);

    #endregion
}