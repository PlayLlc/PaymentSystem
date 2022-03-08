using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.DataElements;

public partial record TerminalType
{
    public record CommunicationType : EnumObject<byte>
    {
        #region Static Metadata

        private static readonly ImmutableSortedDictionary<byte, CommunicationType> _ValueObjectMap;

        /// <remarks>DecimalValue: 3; HexValue: 0x3</remarks>
        public static readonly CommunicationType OfflineOnly;

        /// <remarks>DecimalValue: 2; HexValue: 0x2</remarks>
        public static readonly CommunicationType OnlineAndOfflineCapable;

        /// <remarks>DecimalValue: 1; HexValue: 0x1</remarks>
        public static readonly CommunicationType OnlineOnly;

        #endregion

        #region Constructor

        static CommunicationType()
        {
            const byte onlineOnly = 1;
            const byte offlineOnly = 3;
            const byte onlineAndOfflineCapable = 2;

            OnlineOnly = new CommunicationType(onlineOnly);
            OfflineOnly = new CommunicationType(offlineOnly);
            OnlineAndOfflineCapable = new CommunicationType(onlineAndOfflineCapable);

            _ValueObjectMap = new Dictionary<byte, CommunicationType>
            {
                {onlineOnly, OnlineOnly}, {offlineOnly, OfflineOnly}, {onlineAndOfflineCapable, OnlineAndOfflineCapable}
            }.ToImmutableSortedDictionary(a => a.Key, b => b.Value).ToImmutableSortedDictionary();
        }

        private CommunicationType(byte value) : base(value)
        { }

        #endregion

        #region Instance Members

        public int CompareTo(CommunicationType? other)
        {
            if (other is null)
                return 1;

            return _Value.CompareTo(other._Value);
        }

        public static bool TryGet(byte value, out CommunicationType result) => _ValueObjectMap.TryGetValue(value, out result);

        #endregion

        #region Equality

        public bool Equals(CommunicationType? x, CommunicationType? y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return x.Equals(y);
        }

        public override int GetHashCode() => 4679537 * _Value.GetHashCode();
        public int GetHashCode(CommunicationType obj) => obj.GetHashCode();

        #endregion

        #region Operator Overrides

        public static explicit operator byte(CommunicationType communicationType) => communicationType._Value;

        public static explicit operator CommunicationType(byte communicationType)
        {
            if (!TryGet(communicationType, out CommunicationType result))
            {
                throw new ArgumentOutOfRangeException(nameof(CommunicationType),
                    $"The {nameof(CommunicationType)} could not be found from the number supplied to the argument: {nameof(communicationType)}");
            }

            return result;
        }

        #endregion
    }
}