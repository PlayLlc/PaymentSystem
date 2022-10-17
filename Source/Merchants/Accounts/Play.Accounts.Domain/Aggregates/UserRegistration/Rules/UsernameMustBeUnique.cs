using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class UsernameMustBeUnique : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "Username must be unique";

    #endregion

    #region Constructor

    internal UsernameMustBeUnique(IEnsureUniqueEmails uniqueEmailChecker, Email email)
    {
        _IsValid = uniqueEmailChecker.IsUnique(email);
    }

    #endregion

    #region Instance Members

    public override UsernameWasNotAValidEmail CreateBusinessRuleViolationDomainEvent(UserRegistration aggregate)
    {
        return new UsernameWasNotAValidEmail(aggregate, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}