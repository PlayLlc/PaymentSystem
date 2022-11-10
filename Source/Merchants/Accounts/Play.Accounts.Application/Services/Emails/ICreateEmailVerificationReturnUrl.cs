namespace Play.Accounts.Application.Services;

public interface ICreateEmailVerificationReturnUrl
{
    #region Instance Members

    public string CreateReturnUrl(string userRegistrationId, uint confirmationCode);

    #endregion
}