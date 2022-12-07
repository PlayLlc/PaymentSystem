using Play.Domain.Aggregates;
using Play.Domain.Common.Entities;
using Play.Domain.Common.ValueObjects;
using Play.Identity.Domain.Entities;
using Play.Identity.Domain.Services;

namespace Play.Identity.Domain.Aggregates;

internal class UserRegistrationMustNotBeProhibited : BusinessRule<UserRegistration, SimpleStringId>
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

    public override bool IsBroken() => _IsValid;

    #endregion
}