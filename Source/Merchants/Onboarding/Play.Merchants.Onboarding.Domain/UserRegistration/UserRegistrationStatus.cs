using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Domain.ValueObjects;

namespace Play.Merchants.Onboarding.Domain.UserRegistration
{
    public record UserRegistrationStatus : ValueObject
    {
        #region Instance Values

        public static UserRegistrationStatus WaitingForConfirmation => new(nameof(WaitingForConfirmation));

        public static UserRegistrationStatus Confirmed => new(nameof(Confirmed));

        public static UserRegistrationStatus Expired => new(nameof(Expired));

        public string Value { get; }

        #endregion

        #region Constructor

        private UserRegistrationStatus(string value)
        {
            Value = value;
        }

        #endregion
    }
}