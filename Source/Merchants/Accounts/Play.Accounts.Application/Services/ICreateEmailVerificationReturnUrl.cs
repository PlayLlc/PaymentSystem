namespace Play.Accounts.Application.Services;

public interface ICreateEmailVerificationReturnUrl
{
    #region Instance Members

    public string CreateReturnUrl(string merchantRegistrationId, uint confirmationCode);

    #endregion
}