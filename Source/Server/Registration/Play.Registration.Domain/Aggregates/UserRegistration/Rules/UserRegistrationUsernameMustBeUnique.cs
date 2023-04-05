using Play.Domain.Aggregates;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Services;

namespace Play.Identity.Domain.Aggregates;

internal class UserRegistrationUsernameMustBeUnique : BusinessRule<UserRegistration>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "Username must be unique";

    #endregion

    #region Constructor

    /// <exception cref="AggregateException"></exception>
    internal UserRegistrationUsernameMustBeUnique(IEnsureUniqueEmails uniqueEmailChecker, string email)
    {
        Task<bool> taskResult = uniqueEmailChecker.IsUnique(email);
        Task.WhenAll(taskResult);
        _IsValid = taskResult.Result;
    }

    #endregion

    #region Instance Members

    public override UserRegistrationUsernameWasInvalid CreateBusinessRuleViolationDomainEvent(UserRegistration merchant) => new(merchant, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}