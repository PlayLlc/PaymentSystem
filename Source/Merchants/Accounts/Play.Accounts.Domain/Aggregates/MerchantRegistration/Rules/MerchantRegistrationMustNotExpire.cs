using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates;

internal class MerchantRegistrationMustNotExpire : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly TimeSpan _ValidityPeriod = new(7, 0, 0, 0);
    private readonly bool _IsValid;

    public override string Message => "Merchant cannot be created when registration has expired";

    #endregion

    #region Constructor

    public MerchantRegistrationMustNotExpire(MerchantRegistrationStatus status, DateTimeUtc registeredDate)
    {
        if (status == MerchantRegistrationStatuses.Expired)
        {
            _IsValid = false;

            return;
        }

        if ((DateTimeUtc.Now - registeredDate) > _ValidityPeriod)
        {
            _IsValid = false;

            return;
        }

        _IsValid = true;
    }

    #endregion

    #region Instance Members

    public override MerchantRegistrationHasExpired CreateBusinessRuleViolationDomainEvent(MerchantRegistration merchant)
    {
        return new MerchantRegistrationHasExpired(merchant, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}