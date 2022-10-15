using Play.Globalization.Time;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Play.Codecs;
using Play.Domain.Aggregates;

namespace Play.Identity.Domain.Rules
{
    internal class PasswordMustIncludeAlphabeticCharacter : IBusinessRule
    {
        #region Instance Values

        private readonly bool _AlphabeticCharacterExists;
        public string Message => "Password cannot be created when it doesn't contain an alphabetic character"; //...

        #endregion

        #region Constructor

        public PasswordMustIncludeAlphabeticCharacter(string password)
        {
            if (!AlphabeticCodec.AlphabeticCodec.IsValid(password))
                _AlphabeticCharacterExists = false;

            _AlphabeticCharacterExists = true;
        }

        #endregion

        #region Instance Members

        public bool IsBroken()
        {
            return _AlphabeticCharacterExists;
        }

        #endregion
    }

    internal class UserCannotBeCreatedWhenRegistrationHasExpired : IBusinessRule
    {
        #region Instance Values

        private readonly TimeSpan _ValidityPeriod = TimeSpan.FromDays(1);
        private readonly DateTimeUtc _RegisteredDate;

        public string Message => "User cannot be created when registration has expired";

        #endregion

        #region Constructor

        public UserCannotBeCreatedWhenRegistrationHasExpired(DateTimeUtc registeredDate)
        {
            _RegisteredDate = registeredDate;
        }

        #endregion

        #region Instance Members

        public bool IsBroken()
        {
            return (DateTimeUtc.Now - _RegisteredDate) > _ValidityPeriod;
        }

        #endregion
    }
}