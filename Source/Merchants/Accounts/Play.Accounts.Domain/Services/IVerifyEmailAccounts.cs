using Play.Core;

namespace Play.Accounts.Domain.Services;

public interface IVerifyEmailAccounts
{
    #region Instance Members

    public Task<Result> SendVerificationCode(uint verificationCode, string email);

    #endregion
}