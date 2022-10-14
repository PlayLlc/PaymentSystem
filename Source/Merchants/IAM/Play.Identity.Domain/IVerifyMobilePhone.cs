namespace Play.Identity.Api.Identity.Services._Email_Sms_Clientz
{
    public interface IVerifyMobilePhone
    {
        #region Instance Members

        public Task SendVerificationCode(string email);
        public Task<bool> VerifyConfirmationCode(string email, int code);

        #endregion
    }
}