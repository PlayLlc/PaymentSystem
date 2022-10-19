using Play.Accounts.Domain.ValueObjects;
using Play.Core;

namespace Play.Accounts.Domain.Services;

public interface IVerifyMobilePhones
{
    #region Instance Members

    public Task<Result> SendVerificationCode(uint code, Phone mobile);

    #endregion
}