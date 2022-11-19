using Play.Domain.Aggregates;
using Play.Identity.Domain.Services;
using Play.Identity.Domain.ValueObjects;

namespace Play.Identity.Domain.Aggregates;

internal class MerchantCategoryCodeMustNotBeProhibited : BusinessRule<Merchant, string>
{
    #region Instance Values

    private readonly bool _IsValid;

    public override string Message => $"A Merchant account cannot be created until the {nameof(Merchant)} has been approved";

    #endregion

    #region Constructor

    /// <exception cref="AggregateException"></exception>
    internal MerchantCategoryCodeMustNotBeProhibited(IUnderwriteMerchants merchantUnderwriter, MerchantCategoryCode categoryCode)
    {
        Task<bool> result = merchantUnderwriter.IsIndustryProhibited(categoryCode);
        Task.WhenAll(result);
        _IsValid = result.Result;
    }

    #endregion

    #region Instance Members

    public override MerchantCategoryCodeIsProhibited CreateBusinessRuleViolationDomainEvent(Merchant merchant)
    {
        return new MerchantCategoryCodeIsProhibited(merchant, this);
    }

    public override bool IsBroken()
    {
        return !_IsValid;
    }

    #endregion
}