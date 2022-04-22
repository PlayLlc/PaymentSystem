using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.Ber.DataElements;

public partial record TerminalType
{
    public record EnvironmentType : EnumObject<byte> { public override EnvironmentType[] GetAll() => _ValueObjectMap.Values.ToArray(); public override bool TryGet(byte value, out EnumObject<byte>? result) { if (_ValueObjectMap.TryGetValue(value, out EnvironmentType? enumResult)) { result = enumResult; return true; } result = null; return false; }
 public EnvironmentType() : base() { } public static readonly EnvironmentType Empty = new(); 
#region Static Metadata

        private static readonly ImmutableSortedDictionary<byte, EnvironmentType> _ValueObjectMap;

        /// <remarks>DecimalValue: 0; HexValue: 0x00</remarks>
        public static readonly EnvironmentType Attended;

        /// <remarks>DecimalValue: 3; HexValue: 0x03</remarks>
        public static readonly EnvironmentType Unattended;

        #endregion

        #region Constructor

        static EnvironmentType()
        {
            const byte attended = 0;
            const byte unattended = 3;

            Attended = new EnvironmentType(attended);
            Unattended = new EnvironmentType(unattended);

            _ValueObjectMap = new Dictionary<byte, EnvironmentType> {{attended, Attended}, {unattended, Unattended}}
                .ToImmutableSortedDictionary(a => a.Key, b => b.Value).ToImmutableSortedDictionary();
        }

        private EnvironmentType(byte value) : base(value)
        { }

        #endregion

        #region Instance Members

        public static bool IsEnvironmentType(byte value, EnvironmentType environmentType) =>
            environmentType == Attended ? value < 4 : value > 3;

        public int CompareTo(EnvironmentType? other)
        {
            if (other is null)
                return 1;

            return _Value.CompareTo(other._Value);
        }

        public static bool TryGet(byte value, out EnvironmentType result) => _ValueObjectMap.TryGetValue(value, out result);

        #endregion

        #region Equality

        public bool Equals(EnvironmentType? x, EnvironmentType? y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return x.Equals(y);
        }

        public override int GetHashCode() => 4679537 * _Value.GetHashCode();
        public int GetHashCode(EnvironmentType obj) => obj.GetHashCode();

        #endregion

        #region Operator Overrides

        public static explicit operator byte(EnvironmentType environmentType) => environmentType._Value;

        #endregion
    }
}