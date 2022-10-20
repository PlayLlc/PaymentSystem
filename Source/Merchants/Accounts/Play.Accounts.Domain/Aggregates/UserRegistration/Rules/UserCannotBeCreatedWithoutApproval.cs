using Play.Accounts.Domain.Enums;
using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Accounts.Domain.Aggregates
{
    internal class UserCannotBeCreatedWithoutApproval : BusinessRule<UserRegistration, string>
    {
        #region Instance Values

        private readonly UserRegistrationStatus _Status;

        public override string Message => $"A User account cannot be created until the {nameof(UserRegistration)} has been approved";

        #endregion

        #region Constructor

        internal UserCannotBeCreatedWithoutApproval(UserRegistrationStatus status)
        {
            _Status = status;
        }

        #endregion

        #region Instance Members

        public override UserRegistrationHasNotBeenApproved CreateBusinessRuleViolationDomainEvent(UserRegistration aggregate)
        {
            return new UserRegistrationHasNotBeenApproved(aggregate, this);
        }

        public override bool IsBroken()
        {
            return _Status != UserRegistrationStatuses.Approved;
        }

        #endregion
    }
}