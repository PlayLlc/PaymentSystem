using Play.Accounts.Domain.Aggregates.MerchantRegistration.Events;
using Play.Accounts.Domain.Enums;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

internal class MerchantCannotBeCreatedWhenRegistrationIsNotVerified : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly RegistrationStatuses _RegistrationStatus;

    public override string Message => "Merchant Account cannot be created when registration is not verified";

    #endregion

    #region Constructor

    internal MerchantCannotBeCreatedWhenRegistrationIsNotVerified(RegistrationStatuses registrationStatus)
    {
        _RegistrationStatus = registrationStatus;
    }

    #endregion

    #region Instance Members

    public override MerchantRegistrationHasNotBeenVerified CreateBusinessRuleViolationDomainEvent(MerchantRegistration aggregate)
    {
        return new MerchantRegistrationHasNotBeenVerified(aggregate, this);
    }

    public override bool IsBroken()
    {
        return _RegistrationStatus != RegistrationStatuses.Confirmed;
    }

    #endregion
}