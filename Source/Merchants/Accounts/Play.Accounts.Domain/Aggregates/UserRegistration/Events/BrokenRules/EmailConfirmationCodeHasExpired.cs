using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates;

public record EmailConfirmationCodeHasExpired : BrokenBusinessRuleDomainEvent<UserRegistration, string>
{
    #region Constructor

    public EmailConfirmationCodeHasExpired(UserRegistration userRegistration, IBusinessRule rule) : base(userRegistration, rule)
    { }

    #endregion
}