using Play.Accounts.Domain.Enums;
using Play.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.ValueObjects;

namespace Play.Accounts.Domain.Aggregates
{
    internal class MerchantCannotBeCreatedWithoutApproval : BusinessRule<MerchantRegistration, string>
    {
        #region Instance Values

        private readonly MerchantRegistrationStatus _Status;

        public override string Message => $"A Merchant account cannot be created until the {nameof(MerchantRegistration)} has been approved";

        #endregion

        #region Constructor

        internal MerchantCannotBeCreatedWithoutApproval(MerchantRegistrationStatus status)
        {
            _Status = status;
        }

        #endregion

        #region Instance Members

        public override MerchantRegistrationHasNotBeenApproved CreateBusinessRuleViolationDomainEvent(MerchantRegistration aggregate)
        {
            return new MerchantRegistrationHasNotBeenApproved(aggregate, this);
        }

        public override bool IsBroken()
        {
            return _Status != MerchantRegistrationStatuses.Approved;
        }

        #endregion
    }
}