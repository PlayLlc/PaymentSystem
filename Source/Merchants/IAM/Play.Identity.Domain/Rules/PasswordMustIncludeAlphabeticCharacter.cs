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
}