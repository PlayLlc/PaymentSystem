using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;
using Play.Merchants.Onboarding.Domain.UserRegistration;

namespace Play.Merchants.Onboarding.Domain.Users.Rules
{
    internal class UserCannotBeCreatedWhenRegistrationIsNotConfirmed : IBusinessRule
    {
        #region Instance Values

        private readonly UserRegistrationStatus _RegistrationStatus;

        public string Message => "User cannot be created when registration is not confirmed";

        #endregion

        #region Constructor

        internal UserCannotBeCreatedWhenRegistrationIsNotConfirmed(UserRegistrationStatus registrationStatus)
        {
            _RegistrationStatus = registrationStatus;
        }

        #endregion

        #region Instance Members

        public bool IsBroken()
        {
            return _RegistrationStatus != UserRegistrationStatus.Confirmed;
        }

        #endregion
    }
}