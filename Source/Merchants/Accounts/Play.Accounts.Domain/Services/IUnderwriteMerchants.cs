using Play.Merchants.Onboarding.Domain.Common;
using Play.Merchants.Onboarding.Domain.Enums;
using Play.Merchants.Onboarding.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.Services
{
    public interface IUnderwriteMerchants
    {
        #region Instance Members

        public bool IsMerchantProhibited(Name name, Address address);

        public bool IsIndustryProhibited(MerchantCategoryCodes categoryCodes);

        /// <summary>
        ///     Ensures that the user is not under sanctions, terrorism watch list, money laundering, etc..
        /// </summary>
        /// <returns></returns>
        public bool IsUserProhibited(Address address, ContactInfo contactInfo);

        #endregion
    }
}