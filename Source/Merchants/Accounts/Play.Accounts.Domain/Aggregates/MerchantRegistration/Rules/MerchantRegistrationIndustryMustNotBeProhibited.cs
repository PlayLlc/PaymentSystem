using Play.Accounts.Domain.Services;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

namespace Play.Accounts.Domain.Aggregates;

public class MerchantRegistrationIndustryMustNotBeProhibited : BusinessRule<MerchantRegistration, string>
{
    #region Instance Values

    private readonly bool _IsProhibited;
    public override string Message => "Merchant cannot be registered if their industry is prohibited";

    #endregion

    #region Constructor

    /// <exception cref="AggregateException"></exception>
    public MerchantRegistrationIndustryMustNotBeProhibited(MerchantCategoryCode merchantCategoryCode, IUnderwriteMerchants underwritingService)
    {
        _IsProhibited = IsProhibited(merchantCategoryCode, underwritingService);
    }

    #endregion

    #region Instance Members

    /// <exception cref="AggregateException"></exception>
    public bool IsProhibited(MerchantCategoryCode merchantCategoryCode, IUnderwriteMerchants underwritingService)
    {
        var merchantCategoryTask = underwritingService.IsIndustryProhibited(merchantCategoryCode);
        Task.WhenAll(merchantCategoryTask);

        // ReSharper disable once RedundantSuppressNullableWarningExpression
        return merchantCategoryTask!.Result;
    }

    public override MerchantRegistrationHasBeenRejected CreateBusinessRuleViolationDomainEvent(MerchantRegistration merchant)
    {
        return new MerchantRegistrationHasBeenRejected(merchant, this);
    }

    public override bool IsBroken()
    {
        return _IsProhibited;
    }

    #endregion
}