using Play.Accounts.Domain.Aggregates;
using Play.Core;

namespace Play.Identity.Api.Services;

public interface ILoginUsers
{
    #region Instance Members

    public Task<Result> LoginAsync(HttpContext context, User user, string clearTextPassword);

    #endregion
}