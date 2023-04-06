using Play.Domain.Aggregates;
using Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents.Rules;
using Play.Registration.Domain.Services;

namespace Play.Registration.Domain.Aggregates.UserRegistration.Rules;

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