namespace Play.Identity.Api.Identity.Services;

public class MobilePhoneVerifier : IVerifyMobilePhone
{
    #region Instance Members

    public Task SendVerificationCode(string email)
    {
        return Task.CompletedTask;
    }

    public Task<bool> VerifyConfirmationCode(string email, int code)
    {
        return (Task<bool>) Task.CompletedTask;
    }

    #endregion
}