using Play.Core;

namespace Play.Identity.Domain.Services;

public interface IVerifyMobilePhones
{
    #region Instance Members

    public Task<Result> SendVerificationCode(uint code, string mobile);

    #endregion
}