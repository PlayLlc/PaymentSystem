using Play.Accounts.Domain.ValueObjects;
using Play.Domain.Aggregates;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Accounts.Domain.Entities;
using Play.Accounts.Domain.Services;

namespace Play.Accounts.Domain.Aggregates
{
    internal class UserMustNotBeProhibitedFromRegistering : BusinessRule<UserRegistration, string>
    {
        #region Instance Values

        private readonly bool _IsValid;

        public override string Message => "User must not be prohibited from registering by regulatory or government entities";

        #endregion

        #region Constructor

        internal UserMustNotBeProhibitedFromRegistering(
            IUnderwriteMerchants merchantUnderwriter, PersonalInfo personalInfo, Address address, ContactInfo contactInfo)
        {
            _IsValid = !merchantUnderwriter.IsUserProhibited(personalInfo, address, contactInfo);
        }

        #endregion

        #region Instance Members

        public override UserIsProhibitedFromRegistering CreateBusinessRuleViolationDomainEvent(UserRegistration aggregate)
        {
            return new UserIsProhibitedFromRegistering(aggregate, this);
        }

        public override bool IsBroken()
        {
            return _IsValid;
        }

        #endregion
    }
}