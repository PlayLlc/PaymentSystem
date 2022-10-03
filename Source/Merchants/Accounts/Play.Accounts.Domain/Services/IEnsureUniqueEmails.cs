using Play.Merchants.Onboarding.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.Services;

public interface IEnsureUniqueEmails
{
    #region Instance Members

    public bool IsUnique(Email email);

    #endregion
}