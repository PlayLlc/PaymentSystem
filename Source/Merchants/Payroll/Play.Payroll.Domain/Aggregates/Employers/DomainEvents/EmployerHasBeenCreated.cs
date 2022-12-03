using Play.Domain.Events;
using Play.Globalization.Currency;
using Play.Payroll.Domain.Aggregates.Employers;

namespace Play.Loyalty.Domain.Aggregates;

public record EmployerHasBeenCreated : DomainEvent
{
    #region Instance Values

    public readonly Employer Employer;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public EmployerHasBeenCreated(Employer employer, string merchantId, string userId) : base(
        $"The {nameof(Employer)} with the ID: [{employer.Id}] has been created for the Merchant with the ID: [{merchantId}];")
    {
        Employer = employer;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}

public record EmployerHasBeenRemoved : DomainEvent
{
    #region Instance Values

    public readonly Employer Employer;
    public readonly string MerchantId;
    public readonly string UserId;

    #endregion

    #region Constructor

    public EmployerHasBeenRemoved(Employer employer, string merchantId, string userId) : base(
        $"The {nameof(Employer)} with the ID: [{employer.Id}] has been removed for the Merchant with the ID: [{merchantId}];")
    {
        Employer = employer;
        MerchantId = merchantId;
        UserId = userId;
    }

    #endregion
}