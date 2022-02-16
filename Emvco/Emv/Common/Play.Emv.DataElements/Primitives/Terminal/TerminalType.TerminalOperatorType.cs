using System.Collections.Immutable;

using Play.Core;

namespace Play.Emv.DataElements;

public partial record TerminalType
{
    public record TerminalOperatorType : EnumObject<byte>
    {
        #region Static Metadata

        private static readonly ImmutableSortedDictionary<byte, TerminalOperatorType> _ValueObjectMap;

        /// <remarks>DecimalValue: 10; HexValue: 0x0A</remarks>
        public static readonly TerminalOperatorType FinancialInstitution;

        /// <remarks>DecimalValue: 20; HexValue: 0x14</remarks>
        public static readonly TerminalOperatorType Merchant;

        private const byte _FinancialInstitution = 10;
        private const byte _Merchant = 20;

        #endregion

        #region Constructor

        /// <exception cref="TypeInitializationException"></exception>
        static TerminalOperatorType()
        {
            FinancialInstitution = new TerminalOperatorType(_FinancialInstitution);
            Merchant = new TerminalOperatorType(_Merchant);

            _ValueObjectMap = GetValues(typeof(TerminalOperatorType))
                .ToImmutableSortedDictionary(a => a.Key, b => (TerminalOperatorType) b.Value);
        }

        private TerminalOperatorType(byte value) : base(value)
        { }

        #endregion

        #region Instance Members

        public int CompareTo(TerminalOperatorType? other)
        {
            if (other is null)
                return 1;

            return _Value.CompareTo(other._Value);
        }

        public static bool TryGet(byte value, out TerminalOperatorType result) => _ValueObjectMap.TryGetValue(value, out result);

        #endregion

        #region Equality

        public bool Equals(TerminalOperatorType x, TerminalOperatorType y)
        {
            if (x is null)
                return y is null;

            if (y is null)
                return false;

            return x.Equals(y);
        }

        public override int GetHashCode() => 47533 * _Value.GetHashCode();
        public int GetHashCode(TerminalOperatorType obj) => obj.GetHashCode();

        #endregion

        #region Operator Overrides

        public static bool operator ==(TerminalOperatorType left, byte right)
        {
            if (left is null)
                return false;

            return left._Value == right;
        }

        public static bool operator ==(byte left, TerminalOperatorType right)
        {
            if (right is null)
                return false;

            return right._Value == left;
        }

        public static explicit operator byte(TerminalOperatorType terminalOperatorType) => terminalOperatorType._Value;

        public static explicit operator TerminalOperatorType(byte terminalOperatorType)
        {
            if (!TryGet(terminalOperatorType, out TerminalOperatorType result))
            {
                throw new ArgumentOutOfRangeException(nameof(TerminalOperatorType),
                    $"The {nameof(TerminalOperatorType)} could not be found from the number supplied to the argument: {nameof(terminalOperatorType)}");
            }

            return result;
        }

        public static bool operator !=(TerminalOperatorType left, byte right) => !(left == right);
        public static bool operator !=(byte left, TerminalOperatorType right) => !(left == right);

        #endregion
    }
}