using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record UserMustUpdatePassword : BrokenBusinessRuleDomainEvent<User, string>
{
    #region Constructor

    public UserMustUpdatePassword(User user, IBusinessRule rule) : base(user, rule)
    { }

    #endregion
}