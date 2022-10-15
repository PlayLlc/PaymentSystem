using Play.Accounts.Domain.Aggregates.Merchants;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;
using Play.Domain.Events;
using Play.Globalization.Time;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

public record MerchantRegistrationHasExpired : BusinessRuleViolationDomainEvent<MerchantRegistration, string>
{
    #region Constructor

    public MerchantRegistrationHasExpired(MerchantRegistration merchantRegistration, IBusinessRule rule) : base(merchantRegistration, rule)
    { }

    #endregion
}
 

internal class MerchantCannotBeCreatedWhenRegistrationHasExpired : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly TimeSpan _ValidityPeriod = TimeSpan.FromDays(1);
    private readonly DateTimeUtc _RegisteredDate;

    public override string Message => "User cannot be created when registration has expired";

    #endregion

    #region Constructor

    public MerchantCannotBeCreatedWhenRegistrationHasExpired(DateTimeUtc registeredDate)
    {
        _RegisteredDate = registeredDate;
    }

    #endregion

    #region Instance Members

    public override bool IsBroken()
    {
        return (DateTimeUtc.Now - _RegisteredDate) > _ValidityPeriod;
    }

    public override MerchantRegistrationHasExpired CreateBusinessRuleViolationDomainEvent()
    {
        return new MerchantRegistrationHasExpired()
    }

    #endregion
}