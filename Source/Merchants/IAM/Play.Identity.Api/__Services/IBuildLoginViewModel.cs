using Play.Identity.Api.Models;

namespace Play.Identity.Api.__Services;

public interface IBuildLoginViewModel
{
    #region Instance Members

    public Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl);
    public Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model);

    #endregion
}