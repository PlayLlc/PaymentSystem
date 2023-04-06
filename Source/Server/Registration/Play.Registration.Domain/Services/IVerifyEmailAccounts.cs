using System.ComponentModel.DataAnnotations;

using Play.Core;

namespace Play.Registration.Domain.Services;

public interface IVerifyEmailAccounts
{
    #region Instance Members

    public Task<Result> SendVerificationCode(uint verificationCode, [EmailAddress] string email, string? fullName = null);

    #endregion
}