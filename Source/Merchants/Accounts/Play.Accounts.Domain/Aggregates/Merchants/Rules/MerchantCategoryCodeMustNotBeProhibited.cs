﻿using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Aggregates.Merchants.Events;
using Play.Accounts.Domain.Services;

namespace Play.Accounts.Domain.Aggregates.Merchants.Rules
{
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
            var result = merchantUnderwriter.IsIndustryProhibited(categoryCode);
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
}