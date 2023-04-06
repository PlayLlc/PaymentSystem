using Play.Domain.Aggregates;
using Play.Domain.Common.Entities;
using Play.Registration.Domain.Aggregates.UserRegistration.DomainEvents.Rules;
using Play.Registration.Domain.Entities;
using Play.Registration.Domain.Services;

namespace Play.Registration.Domain.Aggregates.UserRegistration.Rules;

internal class UserRegistrationMustNotBeProhibited : BusinessRule<UserRegistration>
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

    public override UserRegistrationHasBeenRejected CreateBusinessRuleViolationDomainEvent(UserRegistration merchant) => new(merchant, this);

    public override bool IsBroken() => !_IsValid;

    #endregion
}