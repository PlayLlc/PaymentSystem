namespace Play.Identity.Api.Identity.Services._Email_Sms_Clientz;

public interface IVerifyEmailAccount
{
    #region Instance Members

    public Task<RestfulResult> SendVerificationCode(string emailAddress, Func<int, string> returnUrl);
    public Task<bool> VerifyConfirmationCode(string email, int code);

    #endregion
}