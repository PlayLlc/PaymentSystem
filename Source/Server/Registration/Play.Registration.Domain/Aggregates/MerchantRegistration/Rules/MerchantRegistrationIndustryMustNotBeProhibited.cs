using Play.Domain.Aggregates;
using Play.Registration.Domain.Aggregates.MerchantRegistration.DomainEvents.Rules;
using Play.Registration.Domain.Services;
using Play.Registration.Domain.ValueObjects;

namespace Play.Registration.Domain.Aggregates.MerchantRegistration.Rules;

public class MerchantRegistrationIndustryMustNotBeProhibited : BusinessRule<MerchantRegistration>
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
        Task<bool> merchantCategoryTask = underwritingService.IsIndustryProhibited(merchantCategoryCode);
        Task.WhenAll(merchantCategoryTask);

        // ReSharper disable once RedundantSuppressNullableWarningExpression
        return merchantCategoryTask!.Result;
    }

    public override MerchantRegistrationHasBeenRejected CreateBusinessRuleViolationDomainEvent(MerchantRegistration merchant) => new(merchant, this);

    public override bool IsBroken() => _IsProhibited;

    #endregion
}