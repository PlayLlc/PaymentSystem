namespace Play.Registration.Application.Services.Emails;

public interface ICreateEmailVerificationReturnUrl
{
    #region Instance Members

    public string CreateReturnUrl(string userRegistrationId, uint confirmationCode);

    #endregion
}