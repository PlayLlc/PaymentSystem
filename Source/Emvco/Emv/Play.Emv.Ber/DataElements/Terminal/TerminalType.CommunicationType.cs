using System.Collections.Immutable;

using Play.Core;
using Play.Emv.Ber.Exceptions;

namespace Play.Emv.Ber.DataElements;

public partial record TerminalType
{
    public record CommunicationType : EnumObject<byte>
    {
        #region Static Metadata

        public static readonly CommunicationType Empty = new();
        private static readonly ImmutableSortedDictionary<byte, CommunicationType> _ValueObjectMap;

        /// <remarks>DecimalValue: 3; HexValue: 0x3</remarks>
        public static readonly CommunicationType OfflineOnly;

        /// <remarks>DecimalValue: 2; HexValue: 0x2</remarks>
        public static readonly CommunicationType OnlineAndOfflineCapable;

        /// <remarks>DecimalValue: 1; HexValue: 0x1</remarks>
        public static readonly CommunicationType OnlineOnly;

        #endregion

        #region Constructor

        public CommunicationType() : base()
        { }

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

        public override CommunicationType[] GetAll() => _ValueObjectMap.Values.ToArray();

        public override bool TryGet(byte value, out EnumObject<byte>? result)
        {
            if (_ValueObjectMap.TryGetValue(value, out CommunicationType? enumResult))
            {
                result = enumResult;

                return true;
            }

            result = null;

            return false;
        }

        public static bool IsCommunicationType(byte value, CommunicationType communicationType) => ClearUnusedDigits(value) == communicationType;

        public static byte ClearUnusedDigits(byte value)
        {
            byte result = (byte) (value % 10);

            if (result > 3)
                return (byte) (result - 3);

            return result;
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

        public int CompareTo(CommunicationType? other)
        {
            if (other is null)
                return 1;

            return _Value.CompareTo(other._Value);
        }

        #endregion

        #region Operator Overrides

        public static explicit operator byte(CommunicationType communicationType) => communicationType._Value;

        /// <exception cref="DataElementParsingException"></exception>
        public static explicit operator CommunicationType(byte communicationType)
        {
            if (!TryGet(communicationType, out CommunicationType result))
            {
                throw new DataElementParsingException(nameof(CommunicationType),
                    $"The {nameof(CommunicationType)} could not be found from the number supplied to the argument: {nameof(communicationType)}");
            }

            return result;
        }

        #endregion
    }
}