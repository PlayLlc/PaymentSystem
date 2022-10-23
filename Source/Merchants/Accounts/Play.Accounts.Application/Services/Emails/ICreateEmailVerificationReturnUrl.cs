namespace Play.Accounts.Application.Services.Emails;

public interface ICreateEmailVerificationReturnUrl
{
    #region Instance Members

    public string CreateReturnUrl(string merchantRegistrationId, uint confirmationCode);

    #endregion
}