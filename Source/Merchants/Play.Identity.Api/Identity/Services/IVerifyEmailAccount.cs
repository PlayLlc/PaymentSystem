namespace Play.Identity.Api.Identity.Services
{
    public interface IVerifyEmailAccount
    {
        #region Instance Members

        public Task SendVerificationCode(string email);
        public Task<bool> VerifyConfirmationCode(string email, int code);

        #endregion
    }
}