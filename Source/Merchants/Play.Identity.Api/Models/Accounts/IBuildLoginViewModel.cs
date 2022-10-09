namespace Play.Identity.Api.Models.Accounts;

public interface IBuildLoginViewModel
{
    #region Instance Members

    public Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl);

    #endregion
}