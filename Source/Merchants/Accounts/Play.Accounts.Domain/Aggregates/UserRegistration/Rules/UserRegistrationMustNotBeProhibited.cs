using Play.Domain.Aggregates;
using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Services;

namespace Play.Accounts.Domain.Aggregates;

internal class UserRegistrationMustNotBeProhibited : BusinessRule<UserRegistration, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => "User must not be prohibited from registering by regulatory or government entities";

    #endregion

    #region Constructor

    /// <exception cref="AggregateException"></exception>
    internal UserRegistrationMustNotBeProhibited(IUnderwriteMerchants merchantUnderwriter, PersonalDetail personalDetail, Address address, Contact contact)
    {
        Task<bool> isUserProhibited = merchantUnderwriter.IsUserProhibited(personalDetail, address, contact);
        Task.WhenAll(isUserProhibited);
        _IsValid = isUserProhibited.Result;
    }

    #endregion

    #region Instance Members

    public override UserRegistrationHasBeenRejected CreateBusinessRuleViolationDomainEvent(UserRegistration merchant)
    {
        return new UserRegistrationHasBeenRejected(merchant, this);
    }

    public override bool IsBroken()
    {
        return _IsValid;
    }

    #endregion
}