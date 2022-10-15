using Play.Accounts.Domain.Aggregates.Merchants;
using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;
using Play.Domain.Events;

namespace Play.Accounts.Domain.Aggregates.MerchantRegistration;

public record MerchantRegistrationHasNotBeenVerified : BusinessRuleViolationDomainEvent<MerchantRegistration>
{
    #region Constructor

    public MerchantRegistrationHasNotBeenVerified(string merchantId, Name companyName, IBusinessRule rule) : base(rule,
        $"Id: {merchantId}; {nameof(Merchant)}: [{companyName}];")
    { }

    #endregion
}

internal class MerchantCannotBeCreatedWhenRegistrationIsNotVerified : IBusinessRule
{
    #region Instance Values

    private readonly RegistrationStatuses _RegistrationStatus;

    public string Message => "User cannot be created when registration is not confirmed";

    #endregion

    #region Constructor

    internal MerchantCannotBeCreatedWhenRegistrationIsNotVerified(RegistrationStatuses registrationStatus)
    {
        _RegistrationStatus = registrationStatus;
    }

    #endregion

    #region Instance Members

    public bool IsBroken()
    {
        return _RegistrationStatus != RegistrationStatuses.Confirmed;
    }

    #endregion
}