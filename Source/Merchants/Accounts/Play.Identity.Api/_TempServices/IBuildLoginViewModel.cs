using Play.Identity.Api.Models;

namespace Play.Identity.Api;

public interface IBuildLoginViewModel
{
    #region Instance Members

    public Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl);
    public Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model);

    #endregion
}