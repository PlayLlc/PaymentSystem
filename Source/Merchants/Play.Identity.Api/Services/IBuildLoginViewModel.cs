using Play.Identity.Api.Models;

namespace Play.Identity.Api.Services;

public interface IBuildLoginViewModel
{
    #region Instance Members

    public Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl);

    #endregion
}