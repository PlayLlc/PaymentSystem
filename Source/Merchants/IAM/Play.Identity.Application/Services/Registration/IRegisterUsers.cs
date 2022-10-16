namespace Play.Identity.Application.Services.Registration;

public interface IRegisterUsers
{
    #region Instance Members

    public Task<bool> IsUsernameUnique(string username);
    public Task<Result> ValidatePasswordPolicies(string password);

    #endregion
}