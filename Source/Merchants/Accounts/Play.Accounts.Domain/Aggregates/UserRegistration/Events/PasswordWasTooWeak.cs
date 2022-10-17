using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates
{
    public record PasswordWasTooWeak : BusinessRuleViolationDomainEvent<UserRegistration, string>
    {
        #region Constructor

        public PasswordWasTooWeak(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
        { }

        #endregion
    }
}