using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Services;

namespace Play.Identity.Domain.Aggregates.Rules;

internal class LastFourUserPasswordsMustBeUnique : BusinessRule<User, SimpleStringId>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "The user's last four passwords must be unique";

    #endregion

    #region Constructor

    /// <exception cref="AggregateException"></exception>
    internal LastFourUserPasswordsMustBeUnique(IEnsureUniquePasswordHistory uniquePasswordChecker, string userId, string hashedPassword)
    {
        Task<bool> a = uniquePasswordChecker.AreLastFourPasswordsUnique(userId, hashedPassword);
        Task.WhenAll(a);
        _IsValid = a.Result;
    }

    #endregion

    #region Instance Members

    public override UserPasswordWasTooWeak CreateBusinessRuleViolationDomainEvent(User user) => new UserPasswordWasTooWeak(user, this);

    public override bool IsBroken() => _IsValid;

    #endregion
}