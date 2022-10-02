using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain;

namespace Play.Merchants.Onboarding.Domain.UserRegistration.Rules
{
    internal class UserRegistrationCanNotExpireMoreThanOnce : IBusinessRule
    {
        #region Instance Values

        private readonly UserRegistrationStatus _ActualRegistrationStatus;

        public string Message => "User Registration cannot be confirmed because it is expired";

        #endregion

        #region Constructor

        public UserRegistrationCanNotExpireMoreThanOnce(UserRegistrationStatus actualRegistrationStatus)
        {
            _ActualRegistrationStatus = actualRegistrationStatus;
        }

        #endregion

        #region Instance Members

        public bool IsBroken()
        {
            return _ActualRegistrationStatus == UserRegistrationStatus.Expired;
        }

        #endregion
    }
}