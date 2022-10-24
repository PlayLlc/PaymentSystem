using Play.Core;

using System.ComponentModel.DataAnnotations;

namespace Play.Accounts.Domain.Services;

public interface IVerifyEmailAccounts
{
    #region Instance Members

    public Task<Result> SendVerificationCode(uint verificationCode, [EmailAddress] string email, string? fullName = null);

    #endregion
}