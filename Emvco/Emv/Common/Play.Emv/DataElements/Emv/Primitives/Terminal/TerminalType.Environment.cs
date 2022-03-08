using System.Collections.Generic;
using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.DataElements;

public partial record TerminalType
{
    public record Environment : EnumObject<byte>
    {
        #region Static Metadata

        private static readonly ImmutableSortedDictionary<byte, Environment> _ValueObjectMap;

        /// <remarks>DecimalValue: 0; HexValue: 0x00</remarks>
        public static readonly Environment Attended;

        /// <remarks>DecimalValue: 3; HexValue: 0x03</remarks>
        public static readonly Environment Unattended;

        #endregion

        #region Constructor

        static Environment()
        {
            const byte attended = 0;
            const byte unattended = 3;

            Attended = new Environment(attended);
            Unattended = new Environment(unattended);

            _ValueObjectMap = new Dictionary<byte, Environment> {{attended, Attended}, {unattended, Unattended}}
                .ToImmutableSortedDictionary(a => a.Key, b => b.Value).ToImmutableSortedDictionary();
        }

        private Environment(byte value) : base(value)
        { }

        #endregion

        #region Instance Members

        public int CompareTo(Environment? other)
        {
            if (other is null)
                return 1;

            return _Value.CompareTo(other._Value);
        }

        public static bool TryGet(byte value, out Environment result) => _ValueObjectMap.TryGetValue(value, out result);

        #endregion

        #region Equality

        public bool Equals(Environment? x, Environment? y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return x.Equals(y);
        }

        public override int GetHashCode() => 4679537 * _Value.GetHashCode();
        public int GetHashCode(Environment obj) => obj.GetHashCode();

        #endregion

        #region Operator Overrides

        public static explicit operator byte(Environment environment) => environment._Value;

        #endregion
    }
}