using Play.Domain.Aggregates;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates;

internal class MerchantCannotBeCreatedWhenRegistrationHasExpired : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly TimeSpan _ValidityPeriod = new(7, 0, 0, 0);
    private readonly DateTimeUtc _RegisteredDate;

    public override string Message => "Merchant cannot be created when registration has expired";

    #endregion

    #region Constructor

    public MerchantCannotBeCreatedWhenRegistrationHasExpired(DateTimeUtc registeredDate)
    {
        _RegisteredDate = registeredDate;
    }

    #endregion

    #region Instance Members

    public override MerchantRegistrationHasExpired CreateBusinessRuleViolationDomainEvent(MerchantRegistration aggregate)
    {
        return new MerchantRegistrationHasExpired(aggregate, this);
    }

    public override bool IsBroken()
    {
        return (DateTimeUtc.Now - _RegisteredDate) > _ValidityPeriod;
    }

    #endregion
}