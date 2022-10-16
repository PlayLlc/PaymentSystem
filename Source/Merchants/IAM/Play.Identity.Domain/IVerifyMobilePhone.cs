namespace Play.Identity.Domain;

public interface IVerifyMobilePhone
{
    #region Instance Members

    public Task SendVerificationCode(string email);
    public Task<bool> VerifyConfirmationCode(string email, int code);

    #endregion
}