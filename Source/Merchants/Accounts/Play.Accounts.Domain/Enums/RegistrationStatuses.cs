using System.Collections.Immutable;

using Play.Core;

namespace Play.Accounts.Domain.Enums
{
    public record RegistrationStatuses : EnumObjectString
    {
        #region Static Metadata

        private static readonly ImmutableSortedDictionary<string, RegistrationStatuses> _ValueObjectMap;
        public static RegistrationStatuses WaitingForConfirmation;

        public static RegistrationStatuses Confirmed;

        public static RegistrationStatuses Expired;

        public static RegistrationStatuses Rejected;

        #endregion

        #region Constructor

        private RegistrationStatuses(string value) : base(value)
        { }

        static RegistrationStatuses()
        {
            WaitingForConfirmation = new RegistrationStatuses(nameof(WaitingForConfirmation));
            Confirmed = new RegistrationStatuses(nameof(Confirmed));
            Expired = new RegistrationStatuses(nameof(Expired));
            Rejected = new RegistrationStatuses(nameof(Rejected));

            _ValueObjectMap = new Dictionary<string, RegistrationStatuses>
            {
                {WaitingForConfirmation, WaitingForConfirmation},
                {Confirmed, Confirmed},
                {Expired, Expired},
                {Rejected, Rejected}
            }.ToImmutableSortedDictionary();
        }

        #endregion

        #region Instance Members

        public override RegistrationStatuses[] GetAll()
        {
            return _ValueObjectMap.Values.ToArray();
        }

        public override bool TryGet(string value, out EnumObjectString? result)
        {
            if (_ValueObjectMap.TryGetValue(value, out RegistrationStatuses? enumResult))
            {
                result = enumResult;

                return true;
            }

            result = null;

            return false;
        }

        #endregion
    }
}