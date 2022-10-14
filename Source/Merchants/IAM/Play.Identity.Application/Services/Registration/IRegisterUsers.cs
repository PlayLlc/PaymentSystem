namespace Play.Identity.Api.Identity;

public interface IRegisterUsers
{
    #region Instance Members

    public Task<bool> IsUsernameUnique(string username);
    public Task<Result> ValidatePasswordPolicies(string password);

    #endregion
}