using Play.Accounts.Domain.Enums;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

internal class MerchantRegistrationCannotCompleteWithoutVerification : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly UserRegistrationStatuses _UserRegistrationStatus;

    public override string Message => "Merchant Account cannot be created when registration is not verified";

    #endregion

    #region Constructor

    internal MerchantRegistrationCannotCompleteWithoutVerification(UserRegistrationStatuses userRegistrationStatus)
    {
        _UserRegistrationStatus = userRegistrationStatus;
    }

    #endregion

    #region Instance Members

    public override MerchantRegistrationHasNotBeenVerified CreateBusinessRuleViolationDomainEvent(MerchantRegistration aggregate)
    {
        return new MerchantRegistrationHasNotBeenVerified(aggregate, this);
    }

    public override bool IsBroken()
    {
        return _UserRegistrationStatus != UserRegistrationStatuses.Approved;
    }

    #endregion
}